using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public class Sphere : Mesh
    {
        #region Fields and Properties

        public float Radius { get; set; }

        //public SphereConstruction Construction { get; }

        //public int Res_Lat { get; set; }
        //public int Res_Long { get; set; }

        public SphereConstruction Construction { get; set; }

        #endregion

        #region Constructors

        public Sphere(Vector3D worldOrigin, Orientation worldOrientation, float radius, SphereConstruction construction = SphereConstruction.SectorsAndStacks) : base(worldOrigin, worldOrientation, GenerateStructure(construction))
        {
            Radius = radius;
        }

        

        #endregion

        #region Methods

        private static MeshStructure GenerateStructure(SphereConstruction construction)
        {
            IList<Vertex> vertices = GenerateVertices(construction);
            IList<Edge> edges = GenerateEdges();
            IList<Face> faces = GenerateFaces();

            return new MeshStructure(Dimension.Three, vertices, edges, faces);
        }

        private static IList<Vertex> GenerateVertices(SphereConstruction construction, int latResolution, int longResolution)
        {
            switch (construction)
            {
                case SphereConstruction.SectorsAndStacks:
                    IList<Vertex> vertices = new Vertex[latResolution * longResolution];

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

                            vertices[i * longResolution + j] = new Vertex(new Vector4D(x, y, z, 1));
                        }
                    }

                    break;
                case SphereConstruction.Icosahedron:
                    break;
                case SphereConstruction.Cube:
                    break;
                default:
                    throw new MessageBuilder<ParameterNotSupportedMessage>()
                        .AddParameters(nameof(construction))
                        .BuildIntoException<ArgumentException>();
                        
            }

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