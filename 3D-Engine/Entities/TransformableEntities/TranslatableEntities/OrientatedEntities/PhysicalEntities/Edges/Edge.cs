/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an edge.
 */

using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Utilities.Messages;

namespace Imagenic.Core.Entities;

/// <summary>
/// Encapsulates creation of an <see cref="Edge"/>.
/// </summary>
public abstract class Edge : PhysicalEntity
{
    #region Fields and Properties

    // Appearance
    //public bool Visible { get; set; } = true;

    // Vertices
    private Vertex p1, p2;
    internal Vertex P1
    {
        get => p1;
        set
        {
            if (value == p1) return;
            ThrowIfNull(value);
            p1 = value;
            InvokeRenderEvent(RenderUpdate.NewRender | RenderUpdate.NewShadowMap);
        }
    }
    internal Vertex P2
    {
        get => p2;
        set
        {
            if (value == p2) return;
            ThrowIfNull(value);
            p2 = value;
            InvokeRenderEvent(RenderUpdate.NewRender | RenderUpdate.NewShadowMap);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<EdgeCreatedMessage>? MessageBuilder => (IMessageBuilder<EdgeCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    #if DEBUG

    private protected Edge(Vertex modelP1, Vertex modelP2, IMessageBuilder<EdgeCreatedMessage> mb) : base(modelP1.WorldOrigin, Orientation.OrientationXY, mb)
    {
        NonDebugConstructorBody(modelP1, modelP2);
    }

    #endif

    #if !DEBUG

    /// <summary>
    /// Creates an <see cref="Edge"/>.
    /// </summary>
    /// <param name="modelP1">The position of the first point on the <see cref="Edge"/>.</param>
    /// <param name="modelP2">The position of the second point on the <see cref="Edge"/>.</param>
    public Edge(Vertex modelP1, Vertex modelP2) : base(modelP1.WorldOrigin, Orientation.OrientationXY)
    {
        NonDebugConstructorBody(modelP1, modelP2);
    }

    #endif

    private void NonDebugConstructorBody(Vertex modelP1, Vertex modelP2)
    {
        P1 = modelP1;
        P2 = modelP2;
    }

    #endregion
}