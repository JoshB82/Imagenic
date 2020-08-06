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

            // Is the has_direction_arrows parameter completely necessary?

            Vector3D forward = unit_vector.Cross_Product(Vector3D.Unit_Negative_X);
            Circle arrow_base = new Circle(start_position, forward, unit_vector, body_radius, resolution, false);
            arrow_base.Calculate_Model_to_World_Matrix();
            for (int i = 0; i < resolution; i++) arrow_base.Vertices[i].Point = arrow_base.Model_to_World * arrow_base.Vertices[i].Point;

            Vector3D body_tip_intersection = unit_vector * body_length + start_position;
            Ring arrow_ring = new Ring(body_tip_intersection, forward, unit_vector, body_radius, tip_radius, resolution, false);

            // Vertices must line up so that the arrow isn't twisted.
            Vertices = new Vertex[3 * resolution + 3];
            Vertices[0] = new Vertex(Vector4D.Zero);
            Vertices[1] = new Vertex(new Vector4D(unit_vector * body_length));
            Vertices[2] = new Vertex(new Vector4D(unit_vector * (body_length + tip_length)));
            for (int i = 1; i <= resolution; i++)
            {
                Vertices[i + 2] = arrow_base.Vertices[i];
                Vertices[i + resolution + 2] = arrow_ring.Vertices[i];
                Vertices[i + 2 * resolution + 2] = arrow_ring.Vertices[i + resolution];
            }

            Edges = new Edge[5 * resolution];
            Faces = new Face[6 * resolution];

            for (int i = 0; i < resolution; i++)
            {
                Edges[i] = arrow_base.Edges[i];
                Edges[i + resolution] = arrow_ring.Edges[i];
                Edges[i + 2 * resolution] = arrow_ring.Edges[i + resolution];
                Edges[i + 3 * resolution] = new Edge(arrow_base.Vertices[i + 1], arrow_ring.Vertices[i + 1]);
                Edges[i + 4 * resolution] = new Edge(Vertices[2], arrow_ring.Vertices[i + resolution + 1]);

                Faces[i] = arrow_base.Faces[i];
            }
            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i + resolution] = new Face(arrow_base.Vertices[i + 1], arrow_ring.Vertices[i + 1], arrow_ring.Vertices[i + 2]);
                Faces[i + 2 * resolution] = new Face(arrow_base.Vertices[i + 1], arrow_ring.Vertices[i + 2], arrow_base.Vertices[i + 2]);
            }
            Faces[2 * resolution - 1] = new Face(arrow_base.Vertices[resolution - 1], arrow_ring.Vertices[resolution], arrow_ring.Vertices[1]);
            Faces[3 * resolution - 1] = new Face(arrow_base.Vertices[resolution - 1], arrow_ring.Vertices[1], arrow_base.Vertices[0]);
            for (int i = 0; i < 2 * resolution; i++) Faces[i + 3 * resolution] = arrow_ring.Faces[i];
            for (int i = 0; i < resolution - 1; i++) Faces[i + 5 * resolution] = new Face(arrow_ring.Vertices[i + resolution + 1], arrow_ring.Vertices[i + resolution + 2], Vertices[2]);
            Faces[6 * resolution - 1] = new Face(arrow_ring.Vertices[2 * resolution], arrow_ring.Vertices[resolution + 1], Vertices[2]);
        }

        public Arrow(Vector3D start_position, Vector3D unit_vector, double body_length, double body_radius, double tip_length, double tip_radius, int resolution, bool has_direction_arrows = true) : this(start_position, unit_vector * (body_length + tip_length) + start_position, body_radius, tip_length, tip_radius, resolution, has_direction_arrows) { }

        #endregion
    }
}