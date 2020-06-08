using System;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Arrow : Mesh
    {
        #region Fields and Properties

        public double Tip_Length { get; set; }
        public double Tip_Radius { get; set; }

        #endregion

        #region Constructors

        public Arrow(Vector3D start_position, Vector3D end_position, double tip_length, double tip_radius, int tip_resolution)
        {
            Tip_Length = tip_length;
            Tip_Radius = tip_radius;

            World_Origin = start_position;
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Vector3D cone_line_intersect = (end_position - start_position) * (1 - tip_length / (end_position - start_position).Magnitude());
            Cone arrow_cone = new Cone(cone_line_intersect,,, tip_length, tip_radius, tip_resolution);
            Line arrow_line = new Line(start_position, cone_line_intersect);

            Vertices = new Vector4D[tip_resolution + 3];
            for (int i = 0; i < tip_resolution + 2) Vertices[i] = arrow_cone.Vertices[i];
            Vertices[tip_resolution + 2] = new Vector4D(start_position);

            //Spots = new Spot[2]
            //{
            //    arrow_cone.Spots
            //}; ;

            Edges = new Edge[2 * tip_resolution + 1];
            for (int i = 0; i < 2 * tip_resolution; i++) Edges[i] = arrow_cone.Edges[i];
            Edges[2 * tip_resolution] = new Edge(new Vector4D(start_position), new Vector4D(cone_line_intersect));

            Faces = new Face[2 * resolution];
            for (int i = 0; i < resolution; i++) Faces[i] = cone_base.Faces[i];

            Debug.WriteLine($"Arrow created at {start_position}");
        }

        #endregion
    }
}