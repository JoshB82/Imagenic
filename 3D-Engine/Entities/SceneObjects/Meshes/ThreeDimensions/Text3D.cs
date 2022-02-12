/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a three-dimensional text mesh.
 */

using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;

public sealed class Text3D : Mesh
{
    #region Fields and Properties

    public IEnumerable<string> Fonts { get; set; }
    public float Size { get; set; }
    public char Style { get; set; }
    public float Depth { get; set; }

    #endregion

    #region Constructors

    public Text3D(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    IEnumerable<string> fonts,
                    float size,
                    char style,
                    float depth) : base(worldOrigin, worldOrientation, GenerateStructure())
    {
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure()
    {
        IList<Vertex> vertices = GenerateVertices();
        IList<Edge> edges = GenerateEdges();
        IList<Face> faces = GenerateFaces();

        return new MeshStructure(Dimension.Three, vertices, edges, faces);
    }

    private static IList<Vertex> GenerateVertices()
    {
        return null; // TODO: Finish
    }

    private static IList<Edge> GenerateEdges()
    {
        return null; // TODO: Finish
    }

    private static IList<Face> GenerateFaces()
    {
        return null; // TODO: Finish
    }

    #endregion
}