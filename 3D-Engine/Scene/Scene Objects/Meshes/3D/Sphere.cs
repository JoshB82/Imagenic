using System;
using System.Globalization;

namespace _3D_Engine
{
    public sealed class Sphere : Mesh
    {
        #region Fields and Properties

        public double Radius { get; set; }
        public int Res_Lat { get; set; }
        public int Res_Long { get; set; }

        #endregion

        #region Constructors /////////////////////////////////////////

        public Sphere(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double radius, int res_lat, int res_long) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;

            Radius = radius;

            double angle_lat = 2 * Math.PI / res_lat, angle_long = Math.PI / (2 * res_long); // divide by 2?

            double x, y, z;

            Vertices = new Vertex[res_lat * res_long];

            for (int i = 0; i < res_lat; i++)
            {
                for (int j = 0; j < res_long; j++)
                {
                    x = radius * Math.Cos(angle_long * j) * Math.Sin(angle_lat * i);
                    y = radius * Math.Sin(angle_long * j);
                    z = radius * Math.Cos(angle_long * j) * Math.Cos(angle_lat * i);
                    Vertices[i * res_lat + j] = new Vertex(new Vector4D(x, y, z));
                }
            }

            Draw_Edges = false;

            //Faces = new Face[];


        }

        #endregion
    }
}