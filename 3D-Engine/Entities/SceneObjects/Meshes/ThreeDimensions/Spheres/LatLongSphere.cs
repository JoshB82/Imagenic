using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Spheres
{
    public sealed class LatLongSphere : Sphere
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public LatLongSphere(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float radius, ) : base(origin, directionForward, directionUp, radius)
        {
            

            Edges = new Edge[];

            Faces = new Face[];
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