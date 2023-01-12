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

using Imagenic.Core.Enums;
using System;
using System.Drawing;

namespace Imagenic.Core.Entities;

/// <summary>
/// Encapsulates creation of a <see cref="SolidTriangle"/>.
/// </summary>
public sealed class SolidTriangle : Triangle
{
    #region Fields and Properties

    // Vertices
    //public Vertex P1, P2, P3;
    //public Vector3D T1, T2, T3;

    // Appearance
    public Color Colour { get; set; } = Properties.Settings.Default.FaceColour;

    private Color frontColour, backColour;
    public Color FrontColour
    {
        get => frontColour;
        set
        {
            if (value == frontColour) return;
            frontColour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }
    public Color BackColour
    {
        get => backColour;
        set
        {
            if (value == backColour) return;
            backColour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #endregion

    #region Constructors

    public SolidTriangle(Vertex p1,
                         Vertex p2,
                         Vertex p3,
                         Color colour)
        : this(p1, p2, p3, colour, colour)
    { }

    public SolidTriangle(Vertex p1,
                         Vertex p2,
                         Vertex p3,
                         Color frontColour,
                         Color backColour)
        : base(p1, p2, p3
            #if DEBUG
            , MessageBuilder<SolidTriangleCreatedMessage>.Instance()
            #endif
            )
    {
        FrontColour = frontColour;
        BackColour = backColour;
    }

    internal SolidTriangle(Vector4D p1, Vector4D p2, Vector4D p3) => (P1, P2, P3) = (p1, p2, p3);

    public SolidTriangle(Vertex modelP1, Vertex modelP2, Vertex modelP3) => (P1, P2, P3) = (modelP1, modelP2, modelP3);

    #endregion

    #region Methods

    internal override void Interpolator(RenderingEntity renderingObject, Action<object, int, int, float> bufferAction)
    {
        // Round the vertices
        int x1 = P1.x.RoundToInt();
        int y1 = P1.y.RoundToInt();
        float z1 = P1.z;
        int x2 = P2.x.RoundToInt();
        int y2 = P2.y.RoundToInt();
        float z2 = P2.z;
        int x3 = P3.x.RoundToInt();
        int y3 = P3.y.RoundToInt();
        float z3 = P3.z;

        // Sort the vertices by their y-co-ordinate
        NumericManipulation.SortByY
        (
            ref x1, ref y1, ref z1,
            ref x2, ref y2, ref z2,
            ref x3, ref y3, ref z3
        );

        // Interpolate each point in the triangle
        RenderingEntity.InterpolateSolidTriangle
        (
            bufferAction, Colour,
            x1, y1, z1,
            x2, y2, z2,
            x3, y3, z3
        );
    }

    internal override void ResetVertices() => (P1, P2, P3) = (P1.Point, P2.Point, P3.Point);

    #endregion
}