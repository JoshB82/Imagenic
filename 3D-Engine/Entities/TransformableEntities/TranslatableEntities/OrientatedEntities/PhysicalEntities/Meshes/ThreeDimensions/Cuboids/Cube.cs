/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a Cube Mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Net;

namespace Imagenic.Core.Entities;

/// <summary>
/// A mesh of a cube.
/// </summary>
/// <remarks>
/// Composition<br/>
/// It has six square <see cref="Face">faces</see>, each consisting of two <see cref="Triangle">triangles</see>, 12 <see cref="Edge">edges</see> and eight <see cref="Vertex">vertices</see>.
/// </remarks>
[Serializable]
public class Cube : Mesh
{
    #region Fields and Properties

    // Structure
    private float sideLength;

    /// <summary>
    /// The length of each side.
    /// </summary>
    public float SideLength
    {
        get => sideLength;
        set
        {
            sideLength = value;
            Scaling = new Vector3D(sideLength, sideLength, sideLength);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<CubeCreatedMessage>? MessageBuilder => (IMessageBuilder<CubeCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Cube"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cube"/> in world space.</param>
    /// <param name="sideLength">The length of each side.</param>
    public Cube(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                float sideLength) : this(worldOrigin, worldOrientation, sideLength, SolidEdgeStyle.Black, SolidStyle.Red)
    { }

    /// <summary>
    /// Creates a <see cref="Cube"/> mesh with specified edge and face styles.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cube"/>.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cube"/>.</param>
    /// <param name="sideLength">The length of each side.</param>
    /// <param name="edgeStyle">The appearance of each edge.</param>
    /// <param name="exteriorFaceStyle">The appearance of each exterior face.</param>
    public Cube(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                float sideLength,
                [DisallowNull] EdgeStyle edgeStyle,
                [DisallowNull] FaceStyle exteriorFaceStyle)
        : this(worldOrigin, worldOrientation, sideLength, edgeStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle)
    { }

    /// <summary>
    /// Creates a <see cref="Cube"/> mesh with specified edge and face styles.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cube"/>.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cube"/>.</param>
    /// <param name="sideLength">The length of each side.</param>
    /// <param name="edgeStyle">The appearance of each edge.</param>
    /// <param name="exteriorFrontFaceStyle">The appearance of the front face.</param>
    /// <param name="exteriorRightFaceStyle">The appearance of the right face.</param>
    /// <param name="exteriorBackFaceStyle">The appearance of the back face.</param>
    /// <param name="exteriorLeftFaceStyle">The appearance of the left face.</param>
    /// <param name="exteriorTopFaceStyle">The appearance of the top face.</param>
    /// <param name="exteriorBottomFaceStyle">The appearance of the bottom face.</param>
    public Cube(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                float sideLength,
                [DisallowNull] EdgeStyle edgeStyle,
                [DisallowNull] FaceStyle exteriorFrontFaceStyle,
                [DisallowNull] FaceStyle exteriorRightFaceStyle,
                [DisallowNull] FaceStyle exteriorBackFaceStyle,
                [DisallowNull] FaceStyle exteriorLeftFaceStyle,
                [DisallowNull] FaceStyle exteriorTopFaceStyle,
                [DisallowNull] FaceStyle exteriorBottomFaceStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(edgeStyle, new FaceStyle[] { exteriorFrontFaceStyle, exteriorRightFaceStyle, exteriorBackFaceStyle, exteriorLeftFaceStyle, exteriorTopFaceStyle, exteriorBottomFaceStyle })
            #if DEBUG
            , MessageBuilder<CubeCreatedMessage>.Instance()
            #endif
            )
    {
        MessageBuilder.AddParameter(sideLength);
        SideLength = sideLength;
    }

    public Cube(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                [DisallowNull] Square square) : this(worldOrigin, worldOrientation, square.SideLength, square.Structure.Edges[0].Style, square.Structure.Faces[0].FrontStyle)
    { }

    /*
    /// <summary>
    /// Creates a textured <see cref="Cube"/> mesh, specifying a single <see cref="Texture"/> for all sides.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cube"/> in world space.</param>
    /// <param name="sideLength">The length of each side.</param>
    /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cube"/>.</param>
    public Cube(Vector3D worldOrigin,
                Orientation worldOrientation,
                float sideLength,
                Texture texture) : base(worldOrigin, worldOrientation, GenerateStructure(), new Texture[] { texture })
    {
        SideLength = sideLength;
    }

    /// <summary>
    /// Creates a textured <see cref="Cube"/> mesh, specifying a <see cref="Texture"/> for each side.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cube"/> in world space.</param>
    /// <param name="sideLength">The length of each side of the <see cref="Cube"/>.</param>
    /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cube"/>.</param>
    /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cube"/>.</param>
    /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cube"/>.</param>
    /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cube"/>.</param>
    /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cube"/>.</param>
    /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cube"/>.</param>
    public Cube(Vector3D worldOrigin,
                Orientation worldOrientation,
                float sideLength,
                Texture front,
                Texture right,
                Texture back,
                Texture left,
                Texture top,
                Texture bottom) : base(worldOrigin, worldOrientation, 3, new Texture[] { back, right, front, left, top, bottom })
    {
        SideLength = sideLength;
    }

    
    */
    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(EdgeStyle edgeStyle, FaceStyle[] exteriorFaceStyles)
    {
        EventList<Vertex> vertices = GenerateVertices();
        EventList<Edge> edges = GenerateEdges(edgeStyle);
        EventList<Triangle> triangles = GenerateTriangles(exteriorFaceStyles);
        EventList<Face> faces = GenerateFaces(triangles);

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

    private static EventList<Triangle> GenerateTriangles(FaceStyle[] exteriorStyles)
    {
        return new EventList<Triangle>(MeshData.GenerateCuboidTriangles(SolidStyle.Black, exteriorStyles));
    }

    private static EventList<Face> GenerateFaces(IList<Triangle> triangles)
    {
        /*
        if (Structure.Textures is null)
        {
            return HardcodedMeshData.CuboidSolidFaces;
        }
        else
        {
            return HardcodedMeshData.GenerateCuboidTextureFaces(Structure.Textures.ToArray());
        }*/

        return new EventList<Face>(MeshData.GenerateCuboidFaces(triangles));
    }

    public override Cube ShallowCopy() => (Cube)MemberwiseClone();
    public override Cube DeepCopy()
    {
        var cube = (Cube)base.DeepCopy();
        cube.sideLength = sideLength;
        return cube;
    }

    #endregion

    #region Casting

    /// <summary>
    /// Casts a <see cref="Cube"/> into a <see cref="Cuboid"/>.
    /// </summary>
    /// <param name="cube"><see cref="Cube"/> to cast.</param>
    public static implicit operator Cuboid(Cube cube)
    {
        Cuboid cuboid = new Cuboid(cube.WorldOrigin,
                                    cube.WorldOrientation,
                                    cube.sideLength,
                                    cube.sideLength,
                                    cube.sideLength)
        {
            Textures = cube.Textures
        };
        cuboid.Structure.Faces = cube.Structure.Faces;
        return cuboid;
    }

    #endregion
}