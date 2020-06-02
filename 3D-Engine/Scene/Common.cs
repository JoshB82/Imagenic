using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        /// <summary>
        /// Creates an origin point at (0, 0, 0) and adds it to the scene.
        /// </summary>
        public void Create_Origin()
        {
            World_Point origin_mesh = new World_Point(Vector3D.Zero);
            Shape origin = new Shape(origin_mesh);
            Add(origin);
        }

        /// <summary>
        /// Creates axes starting from (0, 0, 0) and adds them to the scene.
        /// </summary>
        public void Create_Axes()
        {
            Line x_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(250, 0, 0)) { Edge_Colour = Color.Red };
            Line y_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 250, 0)) { Edge_Colour = Color.Green };
            Line z_axis_mesh = new Line(new Vector3D(0, 0, 0), new Vector3D(0, 0, 250)) { Edge_Colour = Color.Blue };

            Shape x_axis = new Shape(x_axis_mesh);
            Shape y_axis = new Shape(y_axis_mesh);
            Shape z_axis = new Shape(z_axis_mesh);

            Add(x_axis);
            Add(y_axis);
            Add(z_axis);
        }
    }
}