/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a GradientEdge, representing a edge with a gradient colour.
 */

namespace Imagenic.Core.Entities;

public sealed class GradientEdge : Edge
{
    #region Fields and Properties

    // ...

    #if DEBUG

    private protected override IMessageBuilder<GradientEdgeCreatedMessage>? MessageBuilder => (IMessageBuilder<GradientEdgeCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    public GradientEdge(Vertex modelP1, Vertex modelP2)
        : base(modelP1, modelP2
        #if DEBUG
            , MessageBuilder<GradientEdgeCreatedMessage>.Instance()
        #endif
        )
    { }

    #endregion
}