using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Spheres
{
    public sealed class CubeSphere : Sphere
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public CubeSphere(Vector3D worldOrigin, Orientation worldOrientation, float radius) : base(worldOrigin, worldOrientation, GenerateStructure(), radius)
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

        }

        private static IList<Edge> GenerateEdges()
        {

        }

        private static IList<Face> GenerateFaces()
        {

        }

        #endregion

        #region Casting

        public static implicit operator Icosphere(CubeSphere sphere)
        {

        }

        public static implicit operator LatLongSphere(CubeSphere sphere)
        {

        }

        #endregion
    }
}