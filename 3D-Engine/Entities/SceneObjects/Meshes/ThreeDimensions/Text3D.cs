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

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
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
                      float depth) : base(worldOrigin, worldOrientation, 3)
        {
        }

        #endregion

        #region Methods

        protected override IList<Vertex> GenerateVertices(MeshData<Vertex> vertexData)
        {

        }

        protected override IList<Edge> GenerateEdges(MeshData<Edge> edgeData)
        {

        }

        protected override IList<Face> GenerateFaces(MeshData<Face> faceData)
        {

        }

        #endregion
    }
}