using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        /// <summary>
        /// Creates a <see cref="World_Point"/> at (0, 0, 0) and adds it to the <see cref="Scene"/>.
        /// </summary>
        public void Create_Origin()
        {
            World_Point origin = new World_Point(Vector3D.Zero);
            Add(origin);
        }

        /// <summary>
        /// Creates axes (<see cref="Arrow"/>) starting from (0, 0, 0) and adds them to the <see cref="Scene"/>.
        /// </summary>
        public void Create_Axes()
        {
            int resolution = 50, body_radius = 5, tip_radius = 10, tip_length = 20;
            Arrow x_axis = new Arrow(Vector3D.Zero, new Vector3D(150, 0, 0), Vector3D.Unit_Y, body_radius, tip_length, tip_radius, resolution) { Face_Colour = Color.Red };
            Arrow y_axis = new Arrow(Vector3D.Zero, new Vector3D(0, 150, 0), Vector3D.Unit_Negative_Z, body_radius, tip_length, tip_radius, resolution) { Face_Colour = Color.Green };
            Arrow z_axis = new Arrow(Vector3D.Zero, new Vector3D(0, 0, 150), -Vector3D.Unit_Y, body_radius, tip_length, tip_radius, resolution) { Face_Colour = Color.Blue };

            Add(x_axis); Add(y_axis); Add(z_axis);
        }
    }
}