using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Enums;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public class Sphere : Mesh
    {
        #region Fields and Properties

        public float Radius { get; set; }

        public SphereConstruction Construction { get; }

        public int Res_Lat { get; set; }
        public int Res_Long { get; set; }

        #endregion

        #region Constructors

        public Sphere(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float radius, int latResolution, int longResolution) : base(origin, directionForward, directionUp)
        {
            Dimension = 3;
            Radius = radius;
            Construction = SphereConstruction.SectorsAndStacks;

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
    }
}