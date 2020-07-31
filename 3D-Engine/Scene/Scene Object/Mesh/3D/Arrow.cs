using System;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Arrow : Mesh
    {
        #region Fields and Properties

        private Vector3D start_position, end_position, unit_vector;
        public Vector3D Start_Position
        {
            get => start_position;
            set
            {
                start_position = value;
                World_Origin = start_position;
            }
        }
        public Vector3D End_Position
        {
            get => end_position;
            set
            {
                end_position = value;
                Vector3D line_vector = end_position - start_position;
                unit_vector = line_vector.Normalise();
                length = (line_vector).Magnitude();
                body_length = length - tip_length;
            }   
        }
        public Vector3D Unit_Vector
        {
            get => unit_vector;
            set
            {
                unit_vector = value.Normalise();
                end_position = unit_vector * (body_length + tip_length) + start_position;
            }
        }

        private double body_length, tip_length, length;
        public double Body_Length {
            get => body_length;
            set
            {
                body_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public double Tip_Length
        {
            get => tip_length;
            set
            {
                tip_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public double Length
        {
            get => length;
            set
            {
                length = value;
                end_position = unit_vector * length + start_position;
                body_length = length - tip_length;
            }
        }

        public double Body_Radius { get; set; }
        public double Tip_Radius { get; set; }

        public int Body_Resolution { get; set; }
        public int Tip_Resolution { get; set; }

        #endregion

        #region Constructors

        public Arrow(Vector3D start_position, Vector3D end_position, double body_radius, int body_resolution, double tip_length, double tip_radius, int tip_resolution) : base(start_position, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Start_Position = start_position;
            Body_Length = (end_position - start_position).Magnitude() - tip_length;
            Tip_Length = tip_length;
            End_Position = end_position;
            Body_Radius = body_radius;
            Body_Resolution = body_resolution;
            Tip_Radius = tip_radius;
            Tip_Resolution = tip_resolution;

            /*
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

            */
        }

        public Arrow(Vector3D start_position, Vector3D unit_vector, double body_length, double body_radius, int body_resolution, double tip_length, double tip_radius, int tip_resolution) : this(start_position, start_position+unit_vector*(body_length+tip_length),body_radius,)
        {
            Start_Position = start_position;
        }

        #endregion
    }
}