using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Spheres
{
    public sealed class LatLongSphere : Sphere
    {
        #region Fields and Properties

        public int LatResolution { get; set; }
        public int LongResolution { get; set; }

        #endregion

        #region Constructors

        public LatLongSphere(Vector3D worldOrigin, Orientation worldOrientation, float radius, int latResolution, int longResolution) : base(worldOrigin, worldOrientation, GenerateStructure(latResolution, longResolution), radius)
        {
            LatResolution = latResolution;
            LongResolution = longResolution;
        }

        #endregion

        #region Methods

        private static MeshStructure GenerateStructure(int latResolution, int longResolution)
        {
            IList<Vertex> vertices = GenerateVertices(latResolution, longResolution);
            IList<Edge> edges = GenerateEdges();
            IList<Face> faces = GenerateFaces();

            return new MeshStructure(Dimension.Three, vertices, edges, faces);
        }

        private static IList<Vertex> GenerateVertices(int latResolution, int longResolution)
        {    
            IList<Vertex> vertices = new Vertex[latResolution * longResolution];

            float latAngleStep = Tau / latResolution;
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

                    
                
                    
                
                    
                default:
                    throw new MessageBuilder<ParameterNotSupportedMessage>()
                        .AddParameters(nameof(construction))
                        .BuildIntoException<ArgumentException>();

            

            return null; // TODO: Finish
        }

        private static IList<Edge> GenerateEdges()
        {
            IList<Edge> edges = new Edge[];
        }

        private static IList<Face> GenerateFaces()
        {
            IList<Face> faces = new Face[];
        }

        #endregion

        #region Casting

        public static implicit operator Icosphere(LatLongSphere sphere)
        {

        }

        public static implicit operator CubeSphere(LatLongSphere sphere)
        {

        }

        #endregion
    }
}