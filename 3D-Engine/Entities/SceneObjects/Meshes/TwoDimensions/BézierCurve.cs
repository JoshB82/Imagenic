/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Bézier curve mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class BézierCurve : Mesh
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public BézierCurve(Vector3D worldOrigin,
                           Orientation worldOrientation) : base(worldOrigin, worldOrientation, 2)
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
            return null;
        }

        #endregion
    }
}