/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a cone mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;

/// <summary>
/// Encapsulates creation of a <see cref="Cone"/> mesh.
/// </summary>
public sealed class Cone : Mesh
{
    #region Fields and Properties

    private float height, radius;
    private int resolution;

    /// <summary>
    /// The height of the <see cref="Cone"/>.
    /// </summary>
    public float Height
    {
        get => height;
        set
        {
            height = value;
            Scaling = new Vector3D(radius, height, radius);
        }
    }

    /// <summary>
    /// The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.
    /// </summary>
    public float Radius
    {
        get => radius;
        set
        {
            radius = value;
            Scaling = new Vector3D(radius, height, radius);
        }
    }

    /// <summary>
    /// The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.
    /// </summary>
    public int Resolution
    {
        get => resolution;
        set
        {
            if (value == resolution) return;
            resolution = value;
            RequestNewRenders();

            GenerateVertices();
            GenerateEdges();
            GenerateFaces();
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Cone"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cone"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cone"/> in world space.</param>
    /// <param name="height">The height of the <see cref="Cone"/>.</param>
    /// <param name="radius">The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
    /// <param name="resolution">The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
    public Cone(Vector3D worldOrigin,
                Orientation worldOrientation,
                float height,
                float radius,
                int resolution) : base(worldOrigin, worldOrientation, 3)
    {
        Height = height;
        Radius = radius;
        Resolution = resolution;
    }

    /// <summary>
    /// Creates a <see cref="Cone"/> mesh from a <see cref="Circle"/>.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cone"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cone"/> in world space.</param>
    /// <param name="baseCircle"></param>
    /// <param name="height">The height of the <see cref="Cone"/>.</param>
    public Cone(Vector3D worldOrigin,
                Orientation worldOrientation,
                Circle baseCircle,
                float height) : this(worldOrigin, worldOrientation, height, baseCircle.Radius, baseCircle.Resolution) { }

    #endregion

    #region Methods

    protected override IList<Vertex> GenerateVertices()
    {
        // They are defined in anti-clockwise order, looking from above and then downwards.
        IList<Vertex> vertices = new Vertex[resolution + 2];
        vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
        vertices[1] = new Vertex(new Vector4D(0, 1, 0, 1));

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
        }

        return vertices;
    }

    protected override IList<Edge> GenerateEdges()
    {
        IList<Vertex> vertices = Content.Vertices;
        IList<Edge> edges = new Edge[resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new SolidEdge(vertices[i + 2], vertices[i + 3]);
        }
        edges[resolution - 1] = new SolidEdge(vertices[resolution + 1], vertices[2]);

        return edges;
    }

    protected override IList<Face> GenerateFaces()
    {
        IList<Vertex> vertices = Content.Vertices;
        IList<Face> faces = new Face[resolution + 1];

        Triangle[] baseTriangles = new Triangle[resolution];
        for (int i = 0; i < resolution - 1; i++)
        {
            baseTriangles[i] = new SolidTriangle(vertices[i + 2], vertices[0], vertices[i + 3]);
        }
        baseTriangles[resolution - 1] = new SolidTriangle(vertices[resolution - 1], vertices[0], vertices[2]);
        faces[0] = new Face(baseTriangles);

        for (int i = 0; i < resolution - 1; i++)
        {
            faces[i + 1] = new Face(new SolidTriangle(vertices[i + 2], vertices[i + 3], vertices[1]));
        }
        faces[2 * resolution - 1] = new Face(new SolidTriangle(vertices[resolution - 1], vertices[2], vertices[1]));

        return faces;
    }

    #endregion
}