/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a point Mesh called a WorldPoint which consists of a single Vertex.
 */

using Imagenic.Core.Enums;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A zero-dimensional <see cref="Mesh"/>.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description><strong>One</strong> vertex.</description></item>
/// </list>
/// </remarks>
public sealed class WorldPoint : Mesh
{
    #region Fields and Properties

    #if DEBUG

    private protected override IMessageBuilder<WorldPointCreatedMessage>? MessageBuilder => (IMessageBuilder<WorldPointCreatedMessage>?)base.MessageBuilder;

    #endif

    public static readonly WorldPoint ZeroOrigin = new(Vector3D.Zero);

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="WorldPoint"/> mesh at the specified origin.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="WorldPoint"/> in world space.</param>
    public WorldPoint(Vector3D worldOrigin) : this(worldOrigin, Orientation.ModelOrientation) { }

    /// <summary>
    /// Creates a <see cref="WorldPoint"/> mesh at the specified origin and with the specified orientation.
    /// </summary>
    /// <param name="worldOrigin">The initial location of the <see cref="WorldPoint"/>.</param>
    /// <param name="worldOrientation">The initial orientation of the <see cref="WorldPoint"/>.</param>
    public WorldPoint(Vector3D worldOrigin,
                      Orientation worldOrientation)
        : base(worldOrigin, worldOrientation, GenerateStructure()
        #if DEBUG
        , MessageBuilder<WorldPointCreatedMessage>.Instance()
        #endif
        )
    { }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure()
    {
        IList<Vertex> vertices = GenerateVertices();

        return new MeshStructure(Dimension.Zero, vertices, null, null, null);
    }

    private static IList<Vertex> GenerateVertices()
    {
        return new Vertex[1] { new Vertex(Vector3D.Zero) };
    }

    #endregion
}