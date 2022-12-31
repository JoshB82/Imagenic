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
using System.Collections.Generic;
using System.Drawing;

namespace Imagenic.Core.Entities;

public sealed class GradientTriangle : Triangle
{
    #region Fields and Properties

    public Gradient FrontGradient { get; set; }
    public Gradient BackGradient { get; set; }

    #endregion

    #region Constructors

    public GradientTriangle(Vertex p1,
                            Vertex p2,
                            Vertex p3,
                            Gradient frontGradient,
                            Gradient backGradient)
        : base(p1, p2, p3
            #if DEBUG
            , MessageBuilder<GradientTriangleCreatedMessage>.Instance()
            #endif
            )
    {
        FrontGradient = frontGradient;
        BackGradient = backGradient;
    }

    #endregion

    #region Methods

    internal override void Interpolator(RenderingEntity renderingObject, Action<object, int, int, float> bufferAction)
    {

    }

    internal override void ResetVertices() => (P1, P2, P3) = (ModelP1.Point, ModelP2.Point, ModelP3.Point);

    #endregion
}
