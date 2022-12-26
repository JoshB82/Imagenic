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
/// Encapsulates creation of a <see cref="WorldPoint"/> mesh.
/// </summary>
public sealed class WorldPoint : Mesh
{
    #region Fields and Properties

    #if DEBUG

    private protected override IMessageBuilder<WorldPointCreatedMessage>? MessageBuilder => (IMessageBuilder<WorldPointCreatedMessage>?)base.MessageBuilder;

    #endif

    public static readonly WorldPoint ZeroOrigin = new(Vector3D.Zero);

    #endregion

    #region Constructors

    #if DEBUG

    public WorldPoint(Vector3D worldOrigin) : this(worldOrigin, Orientation.ModelOrientation) { }

    public WorldPoint(Vector3D worldOrigin,
                      Orientation worldOrientation) : base(worldOrigin, worldOrientation, GenerateStructure(), MessageBuilder<WorldPointCreatedMessage>.Instance()) { }

    #else

    /// <summary>
    /// Creates a <see cref="WorldPoint"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="WorldPoint"/> in world space.</param>
    public WorldPoint(Vector3D worldOrigin) : this(worldOrigin, Orientation.ModelOrientation) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldOrigin"></param>
    /// <param name="worldOrientation"></param>
    public WorldPoint(Vector3D worldOrigin,
                      Orientation worldOrientation) : base(worldOrigin, worldOrientation, GenerateStructure()) { }

    #endif

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure()
    {
        IList<Vertex> vertices = GenerateVertices();

        return new MeshStructure(Dimension.Zero, vertices, null, null);
    }

    private static IList<Vertex> GenerateVertices()
    {
        return new Vertex[1] { new Vertex(new Vector4D(0, 0, 0, 1)) };
    }

    #endregion
}