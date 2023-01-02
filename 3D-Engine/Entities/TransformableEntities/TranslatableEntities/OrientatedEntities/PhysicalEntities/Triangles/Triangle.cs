/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public class Triangle : Entity
{
    #region Fields and Properties

    private FaceStyle frontStyle, backStyle;
    public FaceStyle FrontStyle
    {
        get => frontStyle;
        set
        {
            frontStyle = value;
        }
    }
    public FaceStyle BackStyle
    {
        get => backStyle;
        set
        {
            backStyle = value;
        }
    }

    // Appearance
    //public bool DrawOutline { get; set; } = false;
    //public bool Visible { get; set; } = true;

    // Model space values
    public Vertex P1 { get; set; }
    public Vertex P2 { get; set; }
    public Vertex P3 { get; set; }

    /*
    // Calculation values
    internal Vector4D P1 { get; set; }
    internal Vector4D P2 { get; set; }
    internal Vector4D P3 { get; set; }
    */

    #endregion

    #region Constructors

    public Triangle([DisallowNull] FaceStyle frontStyle,
                    [DisallowNull] FaceStyle backStyle,
                    [DisallowNull] Vertex p1,
                    [DisallowNull] Vertex p2,
                    [DisallowNull] Vertex p3)
        : base(MessageBuilder<TriangleCreatedMessage>.Instance())
    {
        ThrowIfNull(frontStyle, backStyle);
        FrontStyle = frontStyle;
        BackStyle = backStyle;

        ThrowIfNull(p1, p2, p3);
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    #endregion

    #region Methods

    internal void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);

    internal static (Vector4D p1, Vector4D p2, Vector4D p3) ApplyMatrix(Matrix4x4 matrix, (Vector4D p1, Vector4D p2, Vector4D p3))
    {
        return (matrix * p1, matrix * p2, matrix * p3);
    }

    internal abstract void Interpolator(RenderingEntity renderingObject, Action<object, int, int, float> bufferAction);
    internal abstract void ResetVertices();

    #endregion
}

public struct PlanePoints
{
    public Vector3D P1 { get; private set; }
    public Vector3D P2 { get; private set; }
    public Vector3D P3 { get; private set; }

    public PlanePoints()
    {
        P1 = Vector3D.Zero;
        P2 = Vector3D.One;
        P3 = Vector3D.UnitX;
    }

    public PlanePoints(Vector3D p1, Vector3D p2, Vector3D p3)
    {
        if (p1 == p2 || p2 == p3 || p1 == p3)
        {
            // Cannot define plane, therefore throw exception
        }

        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    //internal void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);
}