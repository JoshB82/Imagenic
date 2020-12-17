using _3D_Engine.Maths.Vectors;

using static System.MathF;

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

        private float body_length, tip_length, length;
        public float Body_Length {
            get => body_length;
            set
            {
                body_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public float Tip_Length
        {
            get => tip_length;
            set
            {
                tip_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public float Length
        {
            get => length;
            set
            {
                length = value;
                end_position = unit_vector * length + start_position;
                body_length = length - tip_length;
            }
        }

        public float Body_Radius { get; set; }
        public float Tip_Radius { get; set; }

        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Arrow(Vector3D start_position, Vector3D end_position, Vector3D direction_up, float body_radius, float tip_length, float tip_radius, int resolution, bool has_direction_arrows = true) : base(start_position, end_position - start_position, direction_up, has_direction_arrows)
        {
            Dimension = 3;

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
            Vertices[1] = new Vertex(Vector3D.Unit_Z * body_length);
            Vertices[2] = new Vertex(Vector3D.Unit_Z * (body_length + tip_length));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                float sin = Sin(angle * i), cos = Cos(angle * i);
                Vertices[i + 3] = new Vertex(new Vector4D(cos * body_radius, sin * body_radius, 0, 1));
                Vertices[i + resolution + 3] = new Vertex(new Vector4D(cos * body_radius, sin * body_radius, body_length, 1));
                Vertices[i + 2 * resolution + 3] = new Vertex(new Vector4D(cos * tip_radius, sin * tip_radius, body_length, 1));
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

            Draw_Edges = false;

            Faces = new Face[6 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new Face(Vertices[i + 3], Vertices[0], Vertices[i + 4]);
                Faces[i + resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Faces[i + 2 * resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 4], Vertices[i + 4]);
                Faces[i + 3 * resolution] = new Face(Vertices[i + resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[i + 2 * resolution + 3]);
                Faces[i + 4 * resolution] = new Face(Vertices[i + resolution + 3], Vertices[i + resolution + 4], Vertices[i + 2 * resolution + 4]);
                Faces[i + 5 * resolution] = new Face(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[2]);
            }
            Faces[resolution - 1] = new Face(Vertices[resolution + 2], Vertices[0], Vertices[3]);
            Faces[2 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Faces[3 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[resolution + 3], Vertices[3]);
            Faces[4 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[2 * resolution + 3], Vertices[3 * resolution + 2]);
            Faces[5 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[resolution + 3], Vertices[2 * resolution + 3]);
            Faces[6 * resolution - 1] = new Face(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3], Vertices[2]);
        }

        public Arrow(Vector3D start_position, Vector3D unit_vector, Vector3D direction_up, float body_length, float body_radius, float tip_length, float tip_radius, int resolution, bool has_direction_arrows = true) : this(start_position, unit_vector * (body_length + tip_length) + start_position, direction_up, body_radius, tip_length, tip_radius, resolution, has_direction_arrows) { }

        #endregion
    }
}