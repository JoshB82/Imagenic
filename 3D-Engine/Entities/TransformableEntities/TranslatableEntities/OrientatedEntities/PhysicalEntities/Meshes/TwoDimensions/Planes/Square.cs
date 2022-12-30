﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a class for square meshes which are composed of two identical triangular faces.
 */

using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A sealed class representing a two-dimensional square mesh. It inherits from
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
public sealed class Square : Mesh
{
    #region Fields and Properties

    private float sideLength;

    /// <summary>
    /// The length of each side of the <see cref="Square"/>.
    /// </summary>
    public float SideLength
    {
        get => sideLength;
        set
        {
            sideLength = value;
            Scaling = new Vector3D(sideLength, 1, sideLength);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Square"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Square"/>.</param>
    /// <param name="worldOrientation"></param>
    /// <param name="sideLength">The length of each side of the <see cref="Square"/>.</param>
    public Square(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float sideLength) : base(worldOrigin, worldOrientation, 2)
    {


        Faces = HardcodedMeshData.PlaneSolidFaces;

        SideLength = sideLength;
    }

    /// <summary>
    /// Creates a textured <see cref="Square"/> mesh, specifying a single <see cref="Texture"/> for all sides.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Square"/>.</param>
    /// <param name="worldOrientation"></param>
    /// <param name="sideLength">The length of each side of the <see cref="Square"/>.</param>
    /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Square"/>.</param>
    public Square(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  float sideLength,
                  Texture texture) : base(worldOrigin, worldOrientation, 2, new Texture[] { texture })
    {


        Faces = HardcodedMeshData.GeneratePlaneTextureFace(texture);

        SideLength = sideLength;
    }

    #endregion

    #region Casting

    /// <summary>
    /// Casts a <see cref="Square"/> into a <see cref="Plane"/>.
    /// </summary>
    /// <param name="square"><see cref="Square"/> to cast.</param>
    public static explicit operator Plane(Square square)
    {
        return new Plane(square.WorldOrigin, square.WorldOrientation, square.sideLength, square.sideLength)
        {
            Textures = square.Textures,
        };
    }

    #endregion

    #region Methods

    protected override IList<Vertex> GenerateVertices(MeshData<Vertex> vertexData)
    {
        return HardcodedMeshData.PlaneVertices;
    }

    protected override IList<Edge> GenerateEdges(MeshData<Edge> edgeData)
    {
        return HardcodedMeshData.PlaneEdges;
    }

    protected override IList<Face> GenerateFaces(MeshData<Face> faceData)
    {

    }



    #endregion
}