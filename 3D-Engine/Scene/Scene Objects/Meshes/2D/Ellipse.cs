using System;

namespace _3D_Engine
{
    public class Ellipse : Mesh
    {
        #region Fields and Properties

        private double major_axis, minor_axis;

        public double Major_Axis
        {
            get => major_axis;
            set
            {
                major_axis = value;
                Scaling = new Vector3D(major_axis, 1, minor_axis);
            }
        }

        public double Minor_Axis
        {
            get => Minor_Axis;
            set
            {
                minor_axis = value;
                Scaling = new Vector3D(major_axis, 1, minor_axis);
            }
        }

        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Ellipse(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double major_axis, double minor_axis, int resolution) : base(origin, direction_forward, direction_up)
        {
            Dimension = 2;

            Major_Axis = major_axis;
            Minor_Axis = minor_axis;
            Resolution = resolution;

            Vertices = new Vertex[resolution + 1];
            Vertices[0] = new Vertex(Vector4D.Zero);

            double angle = 2 * Math.PI / resolution;
            for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Math.Cos(angle * i), 0, Math.Sin(angle * i)));
        }

        #endregion
    }
}
