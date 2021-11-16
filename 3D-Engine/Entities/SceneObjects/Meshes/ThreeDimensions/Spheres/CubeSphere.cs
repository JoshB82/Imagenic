using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Spheres
{
    public sealed class CubeSphere : Mesh
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public CubeSphere(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation, 3)
        {

        }

        #endregion

        #region Methods

        protected override IList<Vertex> GenerateVertices()
        {

        }

        protected override IList<Edge> GenerateEdges()
        {

        }

        protected override IList<Face> GenerateFaces()
        {

        }

        #endregion
    }
}