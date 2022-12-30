/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a plane.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A sealed class representing a two-dimensional plane mesh. It inherits from
/// the abstract <see cref="Mesh"/> class.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description><strong>4</strong> vertices</description></item>
/// <item><description><strong>4</strong> edges</description></item>
/// <item><description><strong>1</strong> face (made of <strong>2</strong> triangles)</description></item>
/// </list>
/// </remarks>
public sealed class Plane : Mesh
{
    #region Fields and Properties

    private float length, width;

    /// <summary>
    /// The length of the <see cref="Plane"/>.
    /// </summary>
    public float Length
    {
        get => length;
        set
        {
            length = value;
            Scaling = new Vector3D(length, 1, width);
        }
    }
    /// <summary>
    /// The width of the <see cref="Plane"/>.
    /// </summary>
    public float Width
    {
        get => width;
        set
        {
            width = value;
            Scaling = new Vector3D(length, 1, width);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Plane"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Plane"/>.</param>
    /// <param name="worldOrientation">The initial orientation of the <see cref="Plane"/>.</param>
    /// <param name="length">The length of the <see cref="Plane"/>.</param>
    /// <param name="width">The width of the <see cref="Plane"/>.</param>
    public Plane(Vector3D worldOrigin,
                 Orientation worldOrientation,
                 float length,
                 float width)
        : this(worldOrigin, worldOrientation, length, width, null)
    { }

    /// <summary>
    /// Creates a textured <see cref="Plane"/> mesh, specifying a single <see cref="Texture"/> for all sides.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Plane"/>.</param>
    /// <param name="worldOrientation"></param>
    /// <param name="length">The length of the <see cref="Plane"/>.</param>
    /// <param name="width">The width of the <see cref="Plane"/>.</param>
    /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Plane"/>.</param>
    public Plane(Vector3D worldOrigin,
                 Orientation worldOrientation,
                 float length,
                 float width,
                 Texture texture)
        : base(worldOrigin, worldOrientation, GenerateStructure(null)
              #if DEBUG
              , MessageBuilder<PlaneCreatedMessage>.Instance()
              #endif
              )
    {
        Length = length;
        Width = width;
        Textures = new Texture[1] { texture };
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(IList<Texture>? textures)
    {
        EventList<Vertex> vertices = GenerateVertices();
        EventList<Edge> edges = GenerateEdges();
        EventList<Triangle> triangles = GenerateTriangles();
        EventList<Face> faces = GenerateFaces(textures);

        return new MeshStructure(Dimension.Two, vertices, edges, triangles, faces, textures);
    }

    private static EventList<Vertex> GenerateVertices()
    {
        return new EventList<Vertex>(HardcodedMeshData.PlaneVertices);
    }

    private static EventList<Edge> GenerateEdges()
    {
        return new EventList<Edge>(HardcodedMeshData.PlaneEdges);
    }

    private static EventList<Triangle> GenerateTriangles()
    {
        return new EventList<Triangle>();
    }

    private static EventList<Face> GenerateFaces(IList<Texture>? textures)
    {
        return new EventList<Face>((textures is not null && textures.Count > 0)
                                   ? HardcodedMeshData.GeneratePlaneTextureFace(textures[0])
                                   : HardcodedMeshData.PlaneSolidFaces);
    }

    #endregion

    #region Casting

    public static explicit operator Square(Plane plane)
    {
        return new Square(plane.WorldOrigin, plane.WorldOrientation, Min(plane.Length, plane.Width), plane.Structure.Textures?[0]);
    }

    #endregion
}