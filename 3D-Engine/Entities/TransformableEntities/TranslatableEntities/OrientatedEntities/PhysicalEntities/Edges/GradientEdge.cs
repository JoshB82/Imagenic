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

using Imagenic.Core.Entities.SceneObjects.Meshes.Components;

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

    #if DEBUG

    public GradientEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2, MessageBuilder<GradientEdgeCreatedMessage>.Instance())
    {
        NonDebugConstructorBody();
    }

    #else

    public GradientEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2)
    {
        NonDebugConstructorBody();
    }

    #endif

    private void NonDebugConstructorBody()
    {

    }

    #endregion
}