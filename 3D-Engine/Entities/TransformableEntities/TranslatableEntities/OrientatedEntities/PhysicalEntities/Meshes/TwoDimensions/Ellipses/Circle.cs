/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a circle mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A sealed class representing a two-dimensional circle mesh. It inherits from<br/>
/// the abstract <see cref="Mesh"/> class.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description>A number of vertices equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.</description></item>
/// <item><description>A number of edges equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.</description></item>
/// <item><description><strong>1</strong> face (made of a number of triangles equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.)</description></item>
/// </list>
/// </remarks>
public sealed class Circle : Mesh
{
    #region Fields and Properties

    private float radius;
    private int resolution;

    /// <summary>
    /// The radius of the <see cref="Circle"/>.
    /// </summary>
    public float Radius
    {
        get => radius;
        set
        {
            radius = value;
            Scaling = new Vector3D(radius, 1, radius);
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The number of <see cref="Vertex">vertices</see> that are on the perimeter of the <see cref="Circle"/>.
    /// </summary>
    public int Resolution
    {
        get => resolution;
        set
        {
            if (value < 3) throw new ArgumentException("Resolution cannot be less than 3.");
            resolution = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(Structure.Textures, resolution);
            Structure.Edges = GenerateEdges(Structure.Vertices, resolution);
            Structure.Triangles = GenerateTriangles(Structure.Textures, Structure.Vertices, resolution);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Circle"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Circle"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Circle"/> in world space.</param>
    /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
    /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
    public Circle(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float radius,
                  int resolution) : base(worldOrigin, worldOrientation, GenerateStructure(null, resolution)
                      #if DEBUG
                      , MessageBuilder<CircleCreatedMessage>.Instance()
                      #endif
                      )
    {
        Radius = radius;
        Resolution = resolution;
    }

    /// <summary>
    /// Creates a textured <see cref="Circle"/> mesh, specifying a single <see cref= "Texture" /> for all sides.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Circle"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Circle"/> in world space.</param>
    /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
    /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
    /// <param name="texture">The <see cref="Texture"/> that defines what to draw on the surface of the <see cref="Circle"/>.</param>
    public Circle(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float radius,
                  int resolution,
                  Texture texture) : base(worldOrigin, worldOrientation, GenerateStructure(new Texture[] { texture }, resolution)
                    #if DEBUG
                      , MessageBuilder<CircleCreatedMessage>.Instance()
                    #endif
                      )
    {
        Radius = radius;
        Textures = new Texture[1] { texture };
        Resolution = resolution;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(IList<Texture>? textures, int resolution)
    {
        IList<Vertex> vertices = GenerateVertices(textures, resolution);
        IList<Edge> edges = GenerateEdges(vertices, resolution);
        IList<Triangle> triangles = GenerateTriangles(textures, vertices, resolution);
        IList<Face> faces = GenerateFaces(triangles);

        return new MeshStructure(Dimension.Two, vertices, edges, triangles, faces);
    }

    private static EventList<Vertex> GenerateVertices(IList<Texture>? textures, int resolution)
    {
        // Vertices are defined in anti-clockwise order.
        IList<Vertex> vertices = new Vertex[resolution + 1];
        vertices[0] = new Vertex(Vector3D.Zero);

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            vertices[i + 1] = new Vertex(new Vector3D(Cos(angle * i), 0, Sin(angle * i)));
        }

        if (textures is not null)
        {
            textures[0].Vertices = new Vector3D[resolution + 1];
            textures[0].Vertices[0] = new Vector3D(0.5f, 0.5f, 1);

            for (int i = 0; i < resolution; i++)
            {
                textures[0].Vertices[i + 1] = new Vector3D(Cos(angle * i) * 0.5f, Sin(angle * i) * 0.5f, 1);
            }
        }

        return new EventList<Vertex>(vertices);
    }

    private static EventList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
    {
        IList<Edge> edges = new Edge[resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new SolidEdge(vertices[i + 1], vertices[i + 2]);
        }
        edges[resolution - 1] = new SolidEdge(vertices[resolution], vertices[1]);

        return new EventList<Edge>(edges);
    }

    private static EventList<Triangle> GenerateTriangles(IList<Texture>? textures, IList<Vertex> vertices, int resolution)
    {
        IList<Triangle> triangles = new Triangle[resolution];
        
        if (textures is null)
        {
            for (int i = 0; i < resolution - 1; i++)
            {
                triangles.Add(new SolidTriangle(vertices[i + 1], vertices[0], vertices[i + 2]));
            }
            triangles.Add(new SolidTriangle(vertices[resolution], vertices[0], vertices[1]));
        }
        else
        {
            for (int i = 0; i < resolution - 1; i++)
            {
                triangles.Add(new TextureTriangle(vertices[i + 1], vertices[0], vertices[i + 2], textures[0].Vertices[i + 1], textures[0].Vertices[0], textures[0].Vertices[i + 2], textures[0]));
            }
            triangles.Add(new TextureTriangle(vertices[resolution], vertices[0], vertices[1], textures[0].Vertices[resolution], textures[0].Vertices[0], textures[0].Vertices[1], textures[0]));
        }

        return new EventList<Triangle>(triangles);
    }

    private static EventList<Face> GenerateFaces(IList<Triangle> triangles)
    {
        IList<Face> faces = new Face[1];

        faces[0] = new SolidFace(triangles);

        return new EventList<Face>(faces);
    }

    #endregion

    #region Casting

    //cast to sphere?

    #endregion
}