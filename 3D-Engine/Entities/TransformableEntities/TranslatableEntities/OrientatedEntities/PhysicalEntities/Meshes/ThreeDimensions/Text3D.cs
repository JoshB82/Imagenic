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

using Imagenic.Core.Enums;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

public sealed class Text3D : Mesh
{
    #region Fields and Properties

    public IEnumerable<string> Fonts { get; set; }
    public float Size { get; set; }
    public char Style { get; set; }
    public float Depth { get; set; }

    #endregion

    #region Constructors

    #if DEBUG

    public Text3D(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  IEnumerable<string> fonts,
                  float size,
                  char style,
                  float depth) : base(worldOrigin, worldOrientation, GenerateStructure(), MessageBuilder<Text3DCreatedMessage>.Instance())
    {
        NonDebugConstructorBody();
    }

    #else

    public Text3D(Vector3D worldOrigin,
                  Orientation worldOrientation,
                  IEnumerable<string> fonts,
                  float size,
                  char style,
                  float depth) : base(worldOrigin, worldOrientation, GenerateStructure())
    {
    }

    #endif

    private void NonDebugConstructorBody()
    {

    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure()
    {
        IList<Vertex> vertices = GenerateVertices();
        IList<Edge> edges = GenerateEdges();
        IList<Triangle> triangles = GenerateTriangles();
        IList<Face> faces = GenerateFaces();

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces);
    }

    private static IList<Vertex> GenerateVertices()
    {
        return null; // TODO: Finish
    }

    private static IList<Edge> GenerateEdges()
    {
        return null; // TODO: Finish
    }

    private static IList<Triangle> GenerateTriangles()
    {
        return null; // TODO: Finish
    }

    private static IList<Face> GenerateFaces()
    {
        return null; // TODO: Finish
    }

#endregion
}