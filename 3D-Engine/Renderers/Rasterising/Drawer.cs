using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using System;

namespace Imagenic.Core.Renderers.Rasterising;

internal class Drawer
{
    // Interpolation (source!)
    internal static void InterpolateSolidStyle(
        Action<object, int, int, float> action,
        object @object,
        int x1, int y1, float z1,
        int x2, int y2, float z2,
        int x3, int y3, float z3)
    {
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
                    NumericManipulation.Swap(ref sx, ref ex);
                    NumericManipulation.Swap(ref sz, ref ez);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    float z = sz + t * (ez - sz);
                    action(@object, x, y, z);

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
                    NumericManipulation.Swap(ref sx, ref ex);
                    NumericManipulation.Swap(ref sz, ref ez);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    float z = sz + t * (ez - sz);
                    action(@object, x, y, z);

                    t += tStep;
                }
            }
        }
    }

    internal static void InterpolateTextureStyle(
            Bitmap texture,
            int x1, int y1, float tx1, float ty1, float tz1,
            int x2, int y2, float tx2, float ty2, float tz2,
            int x3, int y3, float tx3, float ty3, float tz3)
    {
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
                    NumericManipulation.Swap(ref sx, ref ex);
                    NumericManipulation.Swap(ref stx, ref etx);
                    NumericManipulation.Swap(ref sty, ref ety);
                    NumericManipulation.Swap(ref stz, ref etz);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    int tx = (stx + t * (etx - stx)).RoundToInt();
                    int ty = (sty + t * (ety - sty)).RoundToInt();
                    float tz = stz + t * (etz - stz);

                    TextureAddPointToBuffers(texture, x, y, tz, tx, ty);

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
                    NumericManipulation.Swap(ref sx, ref ex);
                    NumericManipulation.Swap(ref stx, ref etx);
                    NumericManipulation.Swap(ref sty, ref ety);
                    NumericManipulation.Swap(ref stz, ref etz);
                }

                float t = 0, tStep = 1f / (ex - sx);
                for (int x = sx; x <= ex; x++)
                {
                    int tx = (stx + t * (etx - stx)).RoundToInt();
                    int ty = (sty + t * (ety - sty)).RoundToInt();
                    float tz = stz + t * (etz - stz);

                    TextureAddPointToBuffers(texture, x, y, tz, tx, ty);

                    t += tStep;
                }
            }
        }
    }

    internal static void InterpolateGradientStyle(
        int x1, int y1, float z1,
        int x2, int y2, float z2,
        int x3, int y3, float z3,
        GradientStyle style)
    {

    }
}