/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a line mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;

namespace Imagenic.Core.Entities;

/// <summary>
/// A one-dimensional mesh representing a line.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description><strong>Two</strong> vertices;</description></item>
/// <item><description><strong>One</strong> edge.</description></item>
/// </list>
/// </remarks>
public sealed class Line : Mesh
{
    #region Fields and Properties

    public override Vector3D WorldOrigin
    {
        get => base.WorldOrigin;
        set
        {
            base.WorldOrigin = value;
            Scaling = endPosition - value;
        }
    }

    public override Orientation WorldOrientation
    {
        get => base.WorldOrientation;
        set
        {
            base.WorldOrientation = value;
            Scaling = value.DirectionForward * Length;
            endPosition = Scaling + WorldOrigin;
        }
    }

    private Vector3D endPosition;

    /// <summary>
    /// A point where the line terminates.
    /// </summary>
    public Vector3D EndPosition
    {
        get => endPosition;
        set
        {
            endPosition = value;
            Scaling = endPosition - WorldOrigin;
            length = Scaling.Magnitude();
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    private float length;

    /// <summary>
    /// The length of the line.
    /// </summary>
    public float Length
    {
        get => length;
        set
        {
            length = value;
            Scaling = WorldOrientation.DirectionForward * length;
            endPosition = Scaling + WorldOrigin;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a line.
    /// </summary>
    /// <param name="worldOrigin">The initial location of the <see cref="Line"/>; also one of two endpoints.</param>
    /// <param name="worldOrientation">The initial orientation of the <see cref="Line"/>.</param>
    /// <param name="length">The length of the <see cref="Line"/>.</param>
    public Line(Vector3D worldOrigin,
                Orientation worldOrientation,
                float length)
        : base(worldOrigin, worldOrientation, GenerateStructure()
        #if DEBUG
              , MessageBuilder<LineCreatedMessage>.Instance()
        #endif
              )
    {
        Length = length;
    }

    /// <summary>
    /// Creates a line.
    /// </summary>
    /// <param name="worldOrigin">The initial location of the <see cref="Line"/>; also one of two endpoints.</param>
    /// <param name="endPosition">One of two endpoints.</param>
    public Line(Vector3D worldOrigin,
                Vector3D endPosition)
        : this(worldOrigin, Orientation.OrientationZY, (endPosition - worldOrigin).Magnitude())
    {
        DrawFaces = false;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure()
    {
        EventList<Vertex> vertices = GenerateVertices();
        EventList<Edge> edges = GenerateEdges();

        return new MeshStructure(Dimension.One, vertices, edges, null, null, null);
    }

    private static EventList<Vertex> GenerateVertices()
    {
        return new EventList<Vertex>(HardcodedMeshData.LineVertices);
    }

    private static EventList<Edge> GenerateEdges()
    {
        return new EventList<Edge>(HardcodedMeshData.LineEdges);
    }

    #endregion
}