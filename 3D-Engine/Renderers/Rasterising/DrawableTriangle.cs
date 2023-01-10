using Imagenic.Core.Entities;

namespace Imagenic.Core.Renderers.Rasterising;

internal class DrawableTriangle
{
    #region Fields and Properties

    public int x1, y1;
    public int x2, y2;
    public int x3, y3;

    public float z1, z2, z3;

    public FaceStyle faceStyleToBeDrawn;

    #endregion

    #region Constructors

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
    }

    #endregion
}