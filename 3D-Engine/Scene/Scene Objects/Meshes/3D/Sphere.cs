using System;

namespace _3D_Engine
{
    public sealed class Sphere : Mesh
    {
        #region Fields and Properties

        public float Radius { get; set; }
        public int Res_Lat { get; set; }
        public int Res_Long { get; set; }

        #endregion

        #region Constructors /////////////////////////////////////////

        public Sphere(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float radius, int res_lat, int res_long) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;

            Radius = radius;

            float angle_lat = (float)2 * (float)Math.PI / res_lat, angle_long = (float)Math.PI / (2 * res_long); // divide by 2?

            float x, y, z;

            Vertices = new Vertex[res_lat * res_long];

            for (int i = 0; i < res_lat; i++)
            {
                for (int j = 0; j < res_long; j++)
                {
                    x = radius * (float)Math.Cos(angle_long * j) * (float)Math.Sin(angle_lat * i);
                    y = radius * (float)Math.Sin(angle_long * j);
                    z = radius * (float)Math.Cos(angle_long * j) * (float)Math.Cos(angle_lat * i);
                    Vertices[i * res_lat + j] = new Vertex(new Vector4D(x, y, z));
                }
            }

            Draw_Edges = false;

            //Faces = new Face[];


        }

        #endregion
    }
}