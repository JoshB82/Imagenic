namespace _3D_Engine
{
    public sealed class Line : Mesh
    {
        #region Fields and Properties

        private Vector3D start_position, end_position;

        public Vector3D Start_Position
        {
            get => start_position;
            set
            {
                start_position = value;
                Vector3D line_vector = end_position - start_position;
                Scaling = new Vector3D(line_vector.x, line_vector.y, line_vector.z);
            }
        }
        public Vector3D End_Position
        {
            get => end_position;
            set
            {
                end_position = value;
                Vector3D line_vector = end_position - start_position;
                Scaling = new Vector3D(line_vector.x, line_vector.y, line_vector.z);
            }
        }

        public float Length { get; set; } //ss
        public Vector3D Unit_Vector { get; set; } //s

        #endregion

        #region Constructors

        public Line(Vector3D start_position, Vector3D end_position) : base(start_position, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Dimension = 2;

            Start_Position = start_position;
            End_Position = end_position;

            Vertices = new Vertex[2]
            {
                new Vertex(new Vector4D(0, 0, 0, 1)), // 0
                new Vertex(new Vector4D(1, 1, 1, 1)) // 1
            };

            Edges = new Edge[1] { new Edge(Vertices[0], Vertices[1]) };

            Draw_Faces = false;
        }

        public Line(Vector3D start_position, Vector3D unit_vector, float length) : this(start_position, start_position + unit_vector * length)
        {
            Length = length;
            Unit_Vector = unit_vector;
        }

        #endregion
    }
}