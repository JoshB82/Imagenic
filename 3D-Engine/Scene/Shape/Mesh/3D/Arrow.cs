using System;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Arrow : Mesh
    {
        public double Tip_Length { get; set; }
        public double Tip_Radius { get; set; }

        public Arrow(Vector3D start_position, Vector3D end_position, double tip_length, double tip_radius, int tip_resolution)
        {
            /*
            Tip_Length = tip_length;
            Tip_Radius = tip_radius;

            World_Origin = new Vector4D(start_position);
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Vector3D cone_line_intersect = (end_position - start_position) * (1 - tip_length / (end_position - start_position).Magnitude());
            Cone arrow_cone = new Cone(cone_line_intersect,,, tip_length, tip_radius, tip_resolution);
            Line arrow_line = new Line(start_position, cone_line_intersect);

            Model_Vertices = new Vector4D[tip_resolution + 3];
            for (int i = 0; i < tip_resolution + 2) Model_Vertices[i] = arrow_cone.Model_Vertices[i];
            Model_Vertices[tip_resolution + 2] = new Vector4D(start_position);

            Spots = new Spot[2]
            {
                arrow_cone.Spots
            };
            */
        }
    }
}