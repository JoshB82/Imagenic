using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class Ellipse : Mesh
    {
        #region Fields and Properties

        private float major_axis, minor_axis;

        public float Major_Axis
        {
            get => major_axis;
            set
            {
                major_axis = value;
                Scaling = new Vector3D(major_axis, 1, minor_axis);
            }
        }

        public float Minor_Axis
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

        public Ellipse(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float majorAxis, float minorAxis, int resolution) : base(origin, directionForward, directionUp)
        {
            Dimension = 2;

            Major_Axis = majorAxis;
            Minor_Axis = minorAxis;
            Resolution = resolution;

            Vertices = new Vertex[resolution + 1];
            Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
        }

        #endregion
    }
}
