using Imagenic.Core.Entities;
using System;
using System.Drawing;
using static Imagenic.Core.Maths.NumericManipulation;

namespace Imagenic.Core.Renderers.Rasterising;

/// <summary>
/// RenderTriangles are the final triangles that are drawn, i.e. post-transformation triangles.
/// </summary>
internal abstract class RenderTriangle
{
    #region Fields and Properties

    public Vector4D P1, P2, P3;
    public abstract FaceStyle FaceStyle { get; }

    public RenderTriangle(Vector4D p1, Vector4D p2, Vector4D p3)
    {
        P1 = p1; P2 = p2; P3 = p3;
    }

    #endregion

    #region Methods

    public void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);

    public abstract void Interpolate(Buffer2D<Color> colourBuffer, Buffer2D<float> zBuffer);
    //public abstract void OnInterpolation(Buffer2D<Color> colourBuffer, int x, int y, float z);

    #endregion
}

internal sealed class SolidRenderTriangle : RenderTriangle
{
    #region Fields and Properties

    public override SolidStyle FaceStyle { get; }

    #endregion

    #region Constructors

    public SolidRenderTriangle(Vector4D p1, Vector4D p2, Vector4D p3, SolidStyle solidStyle)
        : base(p1, p2, p3)
    {
        FaceStyle = solidStyle;
    }

    #endregion

    #region Methods

    public override void Interpolate(Buffer2D<Color> colourBuffer, Buffer2D<float> zBuffer)
    {
        // Extract values
        int x1 = P1.x.RoundToInt();
        int y1 = P1.y.RoundToInt();
        float z1 = P1.z;
        int x2 = P2.x.RoundToInt();
        int y2 = P2.y.RoundToInt();
        float z2 = P2.z;
        int x3 = P3.x.RoundToInt();
        int y3 = P3.y.RoundToInt();
        float z3 = P3.z;

        // Create steps
        float dyStep1 = y1 - y2;
        float dyStep2 = y1 - y3;
        float dyStep3 = y2 - y3;

        float xStep1 = 0, zStep1 = 0;
        float xStep3 = 0, zStep3 = 0;

        if (dyStep1 != 0)
        {
            xStep1 = (x1 - x2) / dyStep1; // dx from point 1 to point 2
            zStep1 = (z1 - z2) / dyStep1; // dz from point 1 to point 2
        }
        float xStep2 = (x1 - x3) / dyStep2; // dx from point 1 to point 3
        float zStep2 = (z1 - z3) / dyStep2; // dz from point 1 to point 3
        if (dyStep3 != 0)
        {
            xStep3 = (x2 - x3) / dyStep3; // dx from point 2 to point 3
            zStep3 = (z2 - z3) / dyStep3; // dz from point 2 to point 3
        }

        // Draw a flat-bottom triangle
        if (dyStep1 != 0)
        {
            for (int y = y2; y <= y1; y++)
            {
                int sx = ((y - y2) * xStep1 + x2).RoundToInt();
                float sz = (y - y2) * zStep1 + z2;

                int ex = ((y - y3) * xStep2 + x3).RoundToInt();
                float ez = (y - y3) * zStep2 + z3;

                if (sx > ex)
                {
                    Swap(ref sx, ref ex);
                    Swap(ref sz, ref ez);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    float z = sz + t * (ez - sz);
                    OnInterpolation(colourBuffer, x, y, z);

                    t += tStep;
                }
            }
        }

        // Draw a flat-top triangle
        if (dyStep3 != 0)
        {
            for (int y = y3; y <= y2; y++)
            {
                int sx = ((y - y3) * xStep3 + x3).RoundToInt();
                float sz = (y - y3) * zStep3 + z3;

                int ex = ((y - y3) * xStep2 + x3).RoundToInt();
                float ez = (y - y3) * zStep2 + z3;

                if (sx > ex)
                {
                    Swap(ref sx, ref ex);
                    Swap(ref sz, ref ez);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    float z = sz + t * (ez - sz);
                    OnInterpolation(colourBuffer, x, y, z);

                    t += tStep;
                }
            }
        }
    }

    public void OnInterpolation(Buffer2D<Color> colourBuffer, Buffer2D<float> zBuffer, int x, int y, float z)
    {
        if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
        {
            zBuffer.Values[x][y] = z;
            colourBuffer.Values[x][y] = FaceStyle.Colour;
        }
    }

    #endregion
}

internal sealed class TextureRenderTriangle : RenderTriangle
{
    #region Fields and Properties

    public Vector3D T1, T2, T3;
    public override TextureStyle FaceStyle { get; }

    #endregion

    #region Constructors

    public TextureRenderTriangle(Vector4D p1, Vector4D p2, Vector4D p3,
                                 Vector3D t1, Vector3D t2, Vector3D t3,
                                 TextureStyle textureStyle)
        : base(p1, p2, p3)
    {
        T1 = t1; T2 = t2; T3 = t3;
        FaceStyle = textureStyle;
    }

    #endregion

    #region Methods

    public override void Interpolate(Buffer2D<Color> colourBuffer, Buffer2D<float> zBuffer)
    {
        // Extract values
        int x1 = P1.x.RoundToInt();
        int y1 = P1.y.RoundToInt();
        float z1 = P1.z;
        int x2 = P2.x.RoundToInt();
        int y2 = P2.y.RoundToInt();
        float z2 = P2.z;
        int x3 = P3.x.RoundToInt();
        int y3 = P3.y.RoundToInt();
        float z3 = P3.z;
        float tx1 = T1.x, ty1 = T1.y, tz1 = T1.z;
        float tx2 = T2.x, ty2 = T2.y, tz2 = T2.z;
        float tx3 = T3.x, ty3 = T3.y, tz3 = T3.z;

        // Create steps
        float dyStep1 = y1 - y2;
        float dyStep2 = y1 - y3;
        float dyStep3 = y2 - y3;

        float xStep1 = 0, txStep1 = 0, tyStep1 = 0, tzStep1 = 0;
        float xStep3 = 0, txStep3 = 0, tyStep3 = 0, tzStep3 = 0;

        if (dyStep1 != 0)
        {
            xStep1 = (x1 - x2) / dyStep1; // dx from point 1 to point 2
            txStep1 = (tx1 - tx2) / dyStep1; // dtx from point 1 to point 2
            tyStep1 = (ty1 - ty2) / dyStep1; // dty from point 1 to point 2
            tzStep1 = (tz1 - tz2) / dyStep1; // dtz from point 1 to point 2
        }
        float xStep2 = (x1 - x3) / dyStep2; // dx from point 1 to point 3
        float txStep2 = (tx1 - tx3) / dyStep2; // dtx from point 1 to point 3
        float tyStep2 = (ty1 - ty3) / dyStep2; // dty from point 1 to point 3
        float tzStep2 = (tz1 - tz3) / dyStep2; // dtz from point 1 to point 3
        if (dyStep3 != 0)
        {
            xStep3 = (x2 - x3) / dyStep3; // dx from point 2 to point 3
            txStep3 = (tx2 - tx3) / dyStep3; // dtx from point 2 to point 3
            tyStep3 = (ty2 - ty3) / dyStep3; // dty from point 2 to point 3
            tzStep3 = (tz2 - tz3) / dyStep3; // dtz from point 2 to point 3
        }

        // Draw a flat-bottom triangle
        if (dyStep1 != 0)
        {
            for (int y = y2; y <= y1; y++)
            {
                int sx = ((y - y2) * xStep1 + x2).RoundToInt();
                int ex = ((y - y3) * xStep2 + x3).RoundToInt();

                float stx = (y - y2) * txStep1 + tx2;
                float sty = (y - y2) * tyStep1 + ty2;
                float stz = (y - y2) * tzStep1 + tz2;

                float etx = (y - y3) * txStep2 + tx3;
                float ety = (y - y3) * tyStep2 + ty3;
                float etz = (y - y3) * tzStep2 + tz3;

                // ?
                if (sx > ex)
                {
                    Swap(ref sx, ref ex);
                    Swap(ref stx, ref etx);
                    Swap(ref sty, ref ety);
                    Swap(ref stz, ref etz);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    int tx = (stx + t * (etx - stx)).RoundToInt();
                    int ty = (sty + t * (ety - sty)).RoundToInt();
                    float tz = stz + t * (etz - stz);

                    OnInterpolation(colourBuffer, zBuffer, x, y, tz, tx, ty);

                    //TextureAddPointToBuffers(texture, x, y, tz, tx, ty);

                    t += tStep;
                }
            }
        }

        // Draw a flat-top triangle
        if (dyStep3 != 0)
        {
            for (int y = y3; y <= y2; y++)
            {
                int sx = ((y - y3) * xStep3 + x3).RoundToInt();
                int ex = ((y - y3) * xStep2 + x3).RoundToInt();

                float stx = (y - y3) * txStep3 + tx3;
                float sty = (y - y3) * tyStep3 + ty3;
                float stz = (y - y3) * tzStep3 + tz3;

                float etx = (y - y3) * txStep2 + tx3;
                float ety = (y - y3) * tyStep2 + ty3;
                float etz = (y - y3) * tzStep2 + tz3;

                if (sx > ex)
                {
                    Swap(ref sx, ref ex);
                    Swap(ref stx, ref etx);
                    Swap(ref sty, ref ety);
                    Swap(ref stz, ref etz);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    int tx = (stx + t * (etx - stx)).RoundToInt();
                    int ty = (sty + t * (ety - sty)).RoundToInt();
                    float tz = stz + t * (etz - stz);

                    OnInterpolation(colourBuffer, zBuffer, x, y, tz, tx, ty);
                    //TextureAddPointToBuffers(texture, x, y, tz, tx, ty);

                    t += tStep;
                }
            }
        }
    }

    public void OnInterpolation(Buffer2D<Color> colourBuffer, Buffer2D<float> zBuffer, int x, int y, float z, float tx, float ty)
    {
        if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
        {
            zBuffer.Values[x][y] = z;
            var texture = FaceStyle.DisplayTexture.File;
            colourBuffer.Values[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
        }
    }

    #endregion
}