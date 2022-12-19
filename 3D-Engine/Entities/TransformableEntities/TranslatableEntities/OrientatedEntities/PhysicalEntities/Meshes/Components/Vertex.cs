/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Vertex, representing a point in 3D space.
 */

using Imagenic.Core.Enums;

namespace Imagenic.Core.Entities;

/// <summary>
/// A point typically used to form the corners of polygons and the starts and ends of edges.
/// </summary>
public sealed class Vertex : TranslatableEntity
{
    #region Fields and Properties

    private Vector3D? normal;
    public Vector3D? Normal
    {
        get => normal;
        set
        {
            if (value == normal) return;
            normal = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<VertexCreatedMessage>? MessageBuilder => (IMessageBuilder<VertexCreatedMessage>?)base.MessageBuilder; 

    #endif

    #endregion

    #region Constructors

    #if DEBUG

    public Vertex(Vector3D worldOrigin, Vector3D? normal = null) : base(worldOrigin, MessageBuilder<VertexCreatedMessage>.Instance())
    {
        NonDebugConstructorBody(normal);
    }

    #else

    public Vertex(Vector3D worldOrigin, Vector3D? normal = null) : base(worldOrigin)
    {
        NonDebugConstructorBody(normal);
    }

    #endif

    private void NonDebugConstructorBody(Vector3D? normal)
    {
        Normal = normal;
    }

    #endregion
}