namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Ring"/> mesh.
    /// </summary>
    public sealed class Ring : Mesh
    {
        #region Fields and Properties

        private double inner_radius, outer_radius;
        private int resolution;

        private Circle inner_circle, outer_circle;

        public double Inner_Radius
        {
            get => inner_radius;
            set
            {
                inner_radius = value;
                inner_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution, false);
                Set_Circle(inner_circle);

                for (int i = 1; i <= resolution; i++) Vertices[i] = inner_circle.Vertices[i];
            }
        }
        public double Outer_Radius
        {
            get => outer_radius;
            set
            {
                outer_radius = value;
                outer_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution, false);
                Set_Circle(outer_circle);

                for (int i = 1; i <= resolution; i++) Vertices[i + resolution] = outer_circle.Vertices[i];
            }
        }
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                inner_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution, false);
                outer_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution, false);
                Set_Circle(inner_circle);
                Set_Circle(outer_circle);

                Vertices = new Vertex[2 * resolution + 1];
                Vertices[0] = new Vertex(Vector4D.Zero);
                for (int i = 1; i <= resolution; i++)
                {
                    Vertices[i] = inner_circle.Vertices[i];
                    Vertices[i + resolution] = outer_circle.Vertices[i];
                }

                Faces = new Face[2 * resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i] = new Face(inner_circle.Vertices[i], outer_circle.Vertices[i + 1], outer_circle.Vertices[i]);
                    Faces[i + resolution] = new Face(inner_circle.Vertices[i], inner_circle.Vertices[i + 1], outer_circle.Vertices[i + 1]);
                }
                Faces[resolution - 1] = new Face(inner_circle.Vertices[resolution - 1], outer_circle.Vertices[0], outer_circle.Vertices[resolution - 1]);
                Faces[2 * resolution - 1] = new Face(inner_circle.Vertices[resolution - 1], inner_circle.Vertices[0], outer_circle.Vertices[0]);
            }
        }

        private void Set_Circle(Circle circle)
        {
            circle.Calculate_Model_to_World_Matrix();
            for (int i = 0; i < resolution; i++) circle.Vertices[i].Point = circle.Model_to_World * circle.Vertices[i].Point;
        }

        #endregion

        #region Constructors

        public Ring(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double inner_radius, double outer_radius, int resolution, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows)
        {
            Resolution = resolution;
            Inner_Radius = inner_radius;
            Outer_Radius = outer_radius;
        }

        #endregion
    }
}