using Imagenic.Core.Entities;

namespace Imagenic.Core.Renderers.Rasterising;
/*
internal sealed class DrawableTriangle
{
    #region Fields and Properties

    public float x1, y1, z1;
    public float x2, y2, z2;
    public float x3, y3, z3;

    public FaceStyle faceStyle;

    #endregion

    public DrawableTriangle(float x1, float y1, float z1,
                            float x2, float y2, float z2,
                            float x3, float y3, float z3,
                            FaceStyle faceStyle)
    {
        this.x1 = x1; this.y1 = y1; this.z1 = z1;
        this.x1 = x2; this.y2 = y2; this.z2 = z2;
        this.x1 = x3; this.y3 = y3; this.z3 = z3;
        this.faceStyle = faceStyle;
    }
}

internal class DrawableTriangle2
{
    #region Fields and Properties

    /*public int x1, y1;
    public int x2, y2;
    public int x3, y3;*/

    //public float z1, z2, z3;
    /*
    public Vector4D P1 { get; set; }
    public Vector4D P2 { get; set; }
    public Vector4D P3 { get; set; }

    public FaceStyle faceStyleToBeDrawn;

    #endregion

    #region Constructors

    public DrawableTriangle(Vector4D p1, Vector4D p2, Vector4D p3, FaceStyle faceStyleToBeDrawn)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        this.faceStyleToBeDrawn = faceStyleToBeDrawn;
    }

    /*
    public DrawableTriangle(int x1, int y1, float z1,
                            int x2, int y2, float z2,
                            int x3, int y3, float z3,
                            FaceStyle faceStyleToBeDrawn)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
        this.x2 = x2;
        this.y2 = y2;
        this.z2 = z2;
        this.x3 = x3;
        this.y3 = y3;
        this.z3 = z3;
        this.faceStyleToBeDrawn = faceStyleToBeDrawn;
    }*/
    /*
    #endregion

    #region Methods

    internal void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);

    #endregion
}*/