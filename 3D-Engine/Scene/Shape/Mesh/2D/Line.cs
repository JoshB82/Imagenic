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
                Vector3D line_vector = end_position - start_position;
                Scaling = new Vector3D(line_vector.X, line_vector.Y, line_vector.Z);
            }
        }
        public Vector3D End_Position
        {
            get => end_position;
            set
            {
                Vector3D line_vector = end_position - start_position;
                Scaling = new Vector3D(line_vector.X, line_vector.Y, line_vector.Z);
            }
        }

        #endregion

        #region Constructors

        public Line(Vector3D start_position, Vector3D end_position) : base(start_position, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Start_Position = start_position;
            End_Position = end_position;

            Vertices = new Vector4D[2]
            {
                new Vector4D(0, 0, 0), // 0
                new Vector4D(1, 1, 1) // 1
            };

            Spots = new Spot[]
            {
                new Spot(Vertices[0]), // 0
                new Spot(Vertices[1]) // 1
            };

            Edges = new Edge[1]
            {
                new Edge(Vertices[0], Vertices[1]) // 0
            };

            Draw_Faces = false;
        }

        #endregion
    }
}