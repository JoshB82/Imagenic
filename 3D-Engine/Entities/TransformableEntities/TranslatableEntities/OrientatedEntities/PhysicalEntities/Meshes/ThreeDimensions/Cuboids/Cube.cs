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
                Orientation worldOrientation,
                float sideLength) : base(worldOrigin, worldOrientation, GenerateStructure())
    {
        MessageBuilder.AddParameter(sideLength);
        SideLength = sideLength;
    }

    public Cube(Vector3D worldOrigin,
                [DisallowNull] Orientation worldOrientation,
                float sideLength,
                EdgeStyle edgeStyle,
                FaceStyle exteriorFaceStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(edgeStyle, new FaceStyle[] { exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle, exteriorFaceStyle })
            #if DEBUG
            , MessageBuilder<CubeCreatedMessage>.Instance()
            #endif
            )
    {

    }

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

    public Cube(Vector3D worldOrigin, Orientation worldOrientation, Square square) : base(worldOrigin, worldOrientation, 3)
    {
        SideLength = square.SideLength;
        cube.Content.Textures = Structure.Textures;
        cube.Content.Faces[0] = Structure.Faces[0];
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(EdgeStyle edgeStyle, FaceStyle[] exteriorStyles)
    {
        EventList<Vertex> vertices = GenerateVertices();
        EventList<Edge> edges = GenerateEdges(edgeStyle);
        EventList<Triangle> triangles = GenerateTriangles(exteriorStyles);
        EventList<Face> faces = GenerateFaces();

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces, textures);
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
        return new EventList<Triangle>(MeshData.GenerateCuboidTriangles(SolidStyle.Black, exteriorStyles);
    }

    private static IList<Face> GenerateFaces()
    {
        if (Structure.Textures is null)
        {
            return HardcodedMeshData.CuboidSolidFaces;
        }
        else
        {
            return HardcodedMeshData.GenerateCuboidTextureFaces(Structure.Textures.ToArray());
        }
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