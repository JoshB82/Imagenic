using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public abstract class Sphere : Mesh
    {
        #region Fields and Properties

        public float Radius { get; set; }

        //public SphereConstruction Construction { get; }

        //public int Res_Lat { get; set; }
        //public int Res_Long { get; set; }

        public SphereConstruction Construction { get; set; }

        #endregion

        #region Constructors

        public Sphere(Vector3D worldOrigin, Orientation worldOrientation, float radius) : base(worldOrigin, worldOrientation)
        {
            Radius = radius;
        }

        /*
        public Sphere(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float radius, int res_lat, int res_long) : base(origin, directionForward, direction_up)
        {
            Dimension = 3;

            Radius = radius;

            float angle_lat = 2 * PI / res_lat, angle_long = PI / (2 * res_long); // divide by 2?

            float x, y, z;

            Vertices = new Vertex[res_lat * res_long];

            for (int i = 0; i < res_lat; i++)
            {
                for (int j = 0; j < res_long; j++)
                {
                    x = radius * Cos(angle_long * j) * Sin(angle_lat * i);
                    y = radius * Sin(angle_long * j);
                    z = radius * Cos(angle_long * j) * Cos(angle_lat * i);
                    Vertices[i * res_lat + j] = new Vertex(new Vector4D(x, y, z, 1));
                }
            }

            DrawEdges = false;

            //Faces = new Face[];

        }*/

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
}