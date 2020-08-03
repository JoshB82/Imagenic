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
        /// Creates axes (arrows) starting from (0, 0, 0) and adds them to the <see cref="Scene"/>.
        /// </summary>
        public void Create_Axes()
        {
            int resolution = 30, body_radius = 50, tip_radius = 100, tip_length = 50;
            Arrow x_axis = new Arrow(new Vector3D(0, 0, 0), new Vector3D(250, 0, 0), body_radius, tip_length, tip_radius, resolution) { Edge_Colour = Color.Red };
            Arrow y_axis = new Arrow(new Vector3D(0, 0, 0), new Vector3D(0, 250, 0), body_radius, tip_length, tip_radius, resolution) { Edge_Colour = Color.Green };
            Arrow z_axis = new Arrow(new Vector3D(0, 0, 0), new Vector3D(0, 0, 250), body_radius, tip_length, tip_radius, resolution) { Edge_Colour = Color.Blue };

            Add(x_axis); Add(y_axis); Add(z_axis);
        }
    }
}