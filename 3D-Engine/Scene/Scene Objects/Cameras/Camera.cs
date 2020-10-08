/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a camera.
 */

using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Camera"/>.
    /// </summary>
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and Properties

        // Appearance
        public Mesh Icon { get; protected set; }

        /// <summary>
        /// Determines if the <see cref="Camera"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool Draw_Icon { get; set; } = false;

        // View Volume
        private Volume_Outline volume_style = Volume_Outline.None;

        public Volume_Outline Volume_Style
        {
            get => volume_style;
            set
            {
                volume_style = value;

                Volume_Edges.Clear();

                float semi_width = Width / 2, semi_height = Height / 2;

                Vertex zero_point = new Vertex(Vector4D.Zero);
                Vertex near_top_left_point = new Vertex(new Vector4D(-semi_width, semi_height, Z_Near, 1));
                Vertex near_top_right_point = new Vertex(new Vector4D(semi_width, semi_height, Z_Near, 1));
                Vertex near_bottom_left_point = new Vertex(new Vector4D(-semi_width, -semi_height, Z_Near, 1));
                Vertex near_bottom_right_point = new Vertex(new Vector4D(semi_width, -semi_height, Z_Near, 1));

                if ((volume_style & Volume_Outline.Near) == Volume_Outline.Near)
                {
                    Volume_Edges.AddRange(new[]
                    {
                        new Edge(zero_point, near_top_left_point), // Near top left
                        new Edge(zero_point, near_top_right_point), // Near top right
                        new Edge(zero_point, near_bottom_left_point), // Near bottom left
                        new Edge(zero_point, near_bottom_right_point), // Near bottom right
                        new Edge(near_top_left_point, near_top_right_point), // Near top
                        new Edge(near_bottom_left_point, near_bottom_right_point), // Near bottom
                        new Edge(near_top_left_point, near_bottom_left_point), // Near left
                        new Edge(near_top_right_point, near_bottom_right_point) // Near right
                    });
                }

                if ((volume_style & Volume_Outline.Far) == Volume_Outline.Far)
                {
                    float ratio = (this is Orthogonal_Camera) ? 1 : Z_Far / Z_Near;
                    float semi_width_ratio = semi_width * ratio, semi_height_ratio = semi_height * ratio;

                    Vertex far_top_left_point = new Vertex(new Vector4D(-semi_width_ratio, semi_height_ratio, Z_Far, 1));
                    Vertex far_top_right_point = new Vertex(new Vector4D(semi_width_ratio, semi_height_ratio, Z_Far, 1));
                    Vertex far_bottom_left_point =
                        new Vertex(new Vector4D(-semi_width_ratio, -semi_height_ratio, Z_Far, 1));
                    Vertex far_bottom_right_point =
                        new Vertex(new Vector4D(semi_width_ratio, -semi_height_ratio, Z_Far, 1));

                    Volume_Edges.AddRange(new[]
                    {
                        new Edge(near_top_left_point, far_top_left_point), // Far top left
                        new Edge(near_top_right_point, far_top_right_point), // Far top right
                        new Edge(near_bottom_left_point, far_bottom_left_point), // Far bottom left
                        new Edge(near_bottom_right_point, far_bottom_right_point), // Far bottom right
                        new Edge(far_top_left_point, far_top_right_point), // Far top
                        new Edge(far_bottom_left_point, far_bottom_right_point), // Far bottom
                        new Edge(far_top_left_point, far_bottom_left_point), // Far left
                        new Edge(far_top_right_point, far_bottom_right_point) // Far right
                    });
                }
            }
        }

        internal List<Edge> Volume_Edges = new List<Edge>();

        // Matrices
        internal Matrix4x4 World_to_Camera_View, Camera_View_to_Camera_Screen, Camera_Screen_to_World;

        internal override void Calculate_Matrices()
        {
            base.Calculate_Matrices();
            World_to_Camera_View = Model_to_World.Inverse();
            Camera_Screen_to_World = Model_to_World * Camera_View_to_Camera_Screen.Inverse();
        }

        // Clipping planes
        internal Clipping_Plane[] Camera_View_Clipping_Planes;
        internal static Clipping_Plane[] Camera_Screen_Clipping_Planes { get; } = new[]
        {
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_X), // Left
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_Y), // Bottom
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_Z), // Near
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_X), // Right
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_Y), // Top
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_Z) // Far
        };

        // View volume
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract float Z_Near { get; set; }
        public abstract float Z_Far { get; set; }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up)
        {
            string[] icon_obj_data = Properties.Resources.Camera.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, icon_obj_data)
            {
                Dimension = 3,
                Face_Colour = Color.DarkCyan
            };
            Icon.Scale(5);
        }

        #endregion
    }
}