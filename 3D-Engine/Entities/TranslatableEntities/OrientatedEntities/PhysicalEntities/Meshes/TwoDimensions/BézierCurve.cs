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

using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions
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