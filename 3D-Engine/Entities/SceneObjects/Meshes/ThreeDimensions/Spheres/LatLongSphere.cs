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

        public LatLongSphere(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float radius, int latResolution, int longResolution) : base(origin, directionForward, directionUp, radius)
        {
            Vertices = new Vertex[latResolution * longResolution];

            float latAngleStep = 2 * PI / latResolution;
            float longAngleStep = PI / longResolution;

            for (int i = 0; i < longResolution; i++)
            {
                float longAngle = longAngleStep * i - PI / 2;

                for (int j = 0; j < latResolution; j++)
                {
                    float latAngle = latAngleStep * j;

                    float x = Cos(latAngle) * Cos(longAngle);
                    float y = Sin(latAngle) * Cos(longAngle);
                    float z = Sin(longAngle);

                    Vertices[i * longResolution + j] = new Vertex(new Vector4D(x, y, z, 1));
                }
            }

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