/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a Cuboid Mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

/// <summary>
/// A mesh of a cuboid.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// Six square <see cref="Face">faces</see>, each consisting of two <see cref="Triangle">triangles</see>, 12 <see cref="Edge">edges</see> and eight <see cref="Vertex">vertices</see>.
/// </remarks>
[Serializable]
public sealed class Cuboid : Mesh
{
    #region Fields and Properties

    private float length, width, height;

    /// <summary>
    /// The length of the <see cref="Cuboid"/>.
    /// </summary>
    public float Length
    {
        get => length;
        set
        {
            length = value;
            Scaling = new Vector3D(length, width, height);
        }
    }
    /// <summary>
    /// The width of the <see cref="Cuboid"/>.
    /// </summary>
    public float Width
    {
        get => width;
        set
        {
            width = value;
            Scaling = new Vector3D(length, width, height);
        }
    }
    /// <summary>
    /// The height of the <see cref="Cuboid"/>.
    /// </summary>
    public float Height
    {
        get => height;
        set
        {
            height = value;
            Scaling = new Vector3D(length, width, height);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<CuboidCreatedMessage>? MessageBuilder => (IMessageBuilder<CuboidCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Cuboid"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
    /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
    /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
    public Cuboid(Vector3D worldOrigin,
                  [DisallowNull] Orientation worldOrientation,
                  float length,
                  float width,
                  float height)
        : this(worldOrigin, worldOrientation, length, width, height, SolidEdgeStyle.Black, SolidStyle.Red)
    { }

    public Cuboid(Vector3D worldOrigin,
                  [DisallowNull] Orientation worldOrientation,
                  float length,
                  float width,
                  float height,
                  [DisallowNull] EdgeStyle edgeStyle,
                  [DisallowNull] FaceStyle exteriorFaceStyle)
        : this(worldOrigin, worldOrientation, length, width, height, edgeStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle)
    { }

    public Cuboid(Vector3D worldOrigin,
                  [DisallowNull] Orientation worldOrientation,
                  float length,
                  float width,
                  float height,
                  [DisallowNull] EdgeStyle edgeStyle,
                  [DisallowNull] FaceStyle exteriorFrontFaceStyle,
                  [DisallowNull] FaceStyle exteriorRightFaceStyle,
                  [DisallowNull] FaceStyle exteriorBackFaceStyle,
                  [DisallowNull] FaceStyle exteriorLeftFaceStyle,
                  [DisallowNull] FaceStyle exteriorTopFaceStyle,
                  [DisallowNull] FaceStyle exteriorBottomFaceStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(edgeStyle, new FaceStyle[] { exteriorFrontFaceStyle, exteriorRightFaceStyle, exteriorBackFaceStyle, exteriorLeftFaceStyle, exteriorTopFaceStyle, exteriorBottomFaceStyle })
        #if DEBUG
              , MessageBuilder<CuboidCreatedMessage>.Instance()
        #endif
              )
    {
        Length = length;
        Width = width;
        Height = height;
    }

    /*

    /// <summary>
    /// Creates a textured <see cref="Cuboid"/> mesh, specifying a single <see cref="Texture"/> for all sides.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
    /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
    /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
    /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cuboid"/>.</param>
    public Cuboid(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float length,
                  float width,
                  float height,
                  Texture texture) : base(worldOrigin, worldOrientation, 3, new Texture[] { texture, texture, texture, texture, texture, texture })
    {
        Length = length;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Creates a textured <see cref="Cuboid"/> mesh, specifying a <see cref="Texture"/> for each side.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
    /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
    /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
    /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
    /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cuboid"/>.</param>
    /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cuboid"/>.</param>
    /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cuboid"/>.</param>
    /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cuboid"/>.</param>
    /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cuboid"/>.</param>
    /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cuboid"/>.</param>
    public Cuboid(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float length,
                  float width,
                  float height,
                  Texture front,
                  Texture right,
                  Texture back,
                  Texture left,
                  Texture top,
                  Texture bottom) : base(worldOrigin, worldOrientation, 3, new Texture[] { back, right, front, left, top, bottom })
    {
        Length = length;
        Width = width;
        Height = height;
    }
    */
    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(EdgeStyle edgeStyle, FaceStyle[] exteriorFaceStyles)
    {
        EventList<Vertex> vertices = GenerateVertices();
        EventList<Edge> edges = GenerateEdges(edgeStyle);
        EventList<Triangle> triangles = GenerateTriangles(exteriorFaceStyles);
        EventList<Face> faces = GenerateFaces(exteriorFaceStyles);

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces);
    }

    private static EventList<Vertex> GenerateVertices()
    {
        return new EventList<Vertex>(MeshData.CuboidVertices);
    }

    private static EventList<Edge> GenerateEdges(EdgeStyle edgeStyle)
    {
        return new EventList<Edge>(MeshData.GenerateCuboidEdges(edgeStyle));
    }

    private static EventList<Triangle> GenerateTriangles(FaceStyle[] exteriorFaceStyles)
    {
        return new EventList<Triangle>(MeshData.GenerateCuboidTriangles(new FaceStyle[] { SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black }, exteriorFaceStyles));
    }

    private static EventList<Face> GenerateFaces(FaceStyle[] exteriorFaceStyles)
    {
        return new EventList<Face>(MeshData.GenerateCuboidFaces(new FaceStyle[] { SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black, SolidStyle.Black }, exteriorFaceStyles));
    }

    public override Cuboid ShallowCopy() => (Cuboid)MemberwiseClone();
    public override Cuboid DeepCopy()
    {
        var cuboid = (Cuboid)base.DeepCopy();
        cuboid.length = length;
        cuboid.height = height;
        cuboid.width = width;
        return cuboid;
    }

    #endregion

    #region Casting

    /// <summary>
    /// Casts a <see cref="Cuboid"/> into a <see cref="Cube"/>.
    /// </summary>
    /// <param name="cuboid"><see cref="Cuboid"/> to cast.</param>
    public static explicit operator Cube(Cuboid cuboid)
    {
        Cube cube = new Cube(cuboid.WorldOrigin,
                        cuboid.WorldOrientation,
                        Math.Min(Math.Min(cuboid.Length, cuboid.Width), cuboid.Height));
        cube.Structure.Faces = cuboid.Structure.Faces;
        cube.Structure.Textures = cuboid.Structure.Textures;

        return cube;
    }

    #endregion
}