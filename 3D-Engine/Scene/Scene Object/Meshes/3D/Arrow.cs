using System;
using System.Text.RegularExpressions;

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

        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Arrow(Vector3D start_position, Vector3D end_position, double body_radius, double tip_length, double tip_radius, int resolution, bool has_direction_arrows = true) : base(start_position, Vector3D.Unit_Z, Vector3D.Unit_Y, has_direction_arrows)
        {
            Start_Position = start_position;
            Body_Length = (end_position - start_position).Magnitude() - tip_length;
            Tip_Length = tip_length;
            End_Position = end_position;
            Body_Radius = body_radius;
            Tip_Radius = tip_radius;
            Resolution = resolution;

            // Vertices are defined in anti-clockwise order.
            Vertices = new Vertex[3 * resolution + 3];
            Vertices[0] = new Vertex(Vector4D.Zero);
            Vertices[1] = new Vertex(new Vector4D(Vector3D.Unit_Z * body_length));
            Vertices[2] = new Vertex(new Vector4D(Vector3D.Unit_Z * (body_length + tip_length)));

            double angle = 2 * Math.PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                double sin = Math.Sin(angle * i), cos = Math.Cos(angle * i);
                Vertices[i + 3] = new Vertex(new Vector4D(cos * body_radius, sin * body_radius, 0));
                Vertices[i + resolution + 3] = new Vertex(new Vector4D(cos * body_radius, sin * body_radius, body_length));
                Vertices[i + 2 * resolution + 3] = new Vertex(new Vector4D(cos * tip_radius, sin * tip_radius, body_length));
            }

            Edges = new Edge[5 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Edges[i] = new Edge(Vertices[i + 3], Vertices[i + 4]);
                Edges[i + resolution] = new Edge(Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Edges[i + 2 * resolution] = new Edge(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4]);
            }
            Edges[resolution - 1] = new Edge(Vertices[resolution + 2], Vertices[3]);
            Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Edges[3 * resolution - 1] = new Edge(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3]);

            for (int i = 0; i < resolution; i++)
            {
                Edges[i + 3 * resolution] = new Edge(Vertices[i + 3], Vertices[i + resolution + 3]);
                Edges[i + 4 * resolution] = new Edge(Vertices[i + 2 * resolution + 3], Vertices[2]);
            }

            Faces = new Face[6 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new Face(Vertices[i + 3], Vertices[0], Vertices[i + 4]);
                Faces[i + resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Faces[i + 2 * resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 4], Vertices[i + 4]);
                Faces[i + 3 * resolution] = new Face(Vertices[i + resolution + 3], Vertices[2 * resolution + 4], Vertices[2 * resolution + 3]);
                Faces[i + 4 * resolution] = new Face(Vertices[resolution + 3], Vertices[resolution + 4], Vertices[2 * resolution + 4]);
                Faces[i + 5 * resolution] = new Face(Vertices[2 * resolution + 3], Vertices[2], Vertices[2 * resolution + 4]);
            }
            Faces[resolution - 1] = new Face(Vertices[resolution], Vertices[0], Vertices[1]);
            Faces[2 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Faces[3 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[resolution + 3], Vertices[3]);
            Faces[4 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[2 * resolution + 3], Vertices[3 * resolution + 2]);
            Faces[5 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[resolution + 3], Vertices[2 * resolution + 3]);
            Faces[6 * resolution - 1] = new Face(Vertices[3 * resolution + 2], Vertices[2], Vertices[2 * resolution + 3]);
        }

        public Arrow(Vector3D start_position, Vector3D unit_vector, double body_length, double body_radius, double tip_length, double tip_radius, int resolution, bool has_direction_arrows = true) : this(start_position, unit_vector * (body_length + tip_length) + start_position, body_radius, tip_length, tip_radius, resolution, has_direction_arrows) { }

        #endregion
    }
}
 