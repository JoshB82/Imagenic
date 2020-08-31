using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using static System.IO.Directory; // ?

namespace _3D_Engine
{
    public abstract partial class Light : Scene_Object
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 World_to_Light_View { get; set; } // use fields instead?
        internal Matrix4x4 Light_View_to_Light_Screen { get; set; }
        internal Matrix4x4 Light_Screen_to_Light_Window { get; set; }

        internal void Calculate_World_to_Light_View_Matrix()
        {
            // Calculate required transformations
            Matrix4x4 translation = Transform.Translate(-World_Origin);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_up_rotation * new Vector4D(World_Direction_Forward)), Model_Direction_Forward);

            // String the transformations together in the following order:
            // 1) Translation to final position in view space
            // 2) Rotation around direction up vector
            // 3) Rotation around direction forward vector
            World_to_Light_View = direction_forward_rotation * direction_up_rotation * translation;
        }

        // Clipping planes
        internal Clipping_Plane[] Light_View_Clipping_Planes { get; set; }
        internal abstract void Calculate_Light_View_Clipping_Planes(); // necessary?

        // Appearance
        public Color Colour { get; set; } = Color.White;
        public Mesh Icon { get; protected set; }
        public bool Show_Icon { get; set; } = false;
        public double Strength { get; set; }

        public bool Draw_Camera_Model { get; set; } = false;
        public bool Draw_View { get; set; } = false;

        // Shadow map
        internal double[][] Shadow_Map { get; set; }

        public abstract int Shadow_Map_Width { get; set; }
        public abstract int Shadow_Map_Height { get; set; }
        public abstract double Shadow_Map_Z_Near { get; set; }
        public abstract double Shadow_Map_Z_Far { get; set; }

        protected void Set_Shadow_Map()
        {
            Shadow_Map = new double[Shadow_Map_Width][];
            for (int i = 0; i < Shadow_Map_Width; i++) Shadow_Map[i] = new double[Shadow_Map_Height];
            Light_Screen_to_Light_Window = Transform.Scale(0.5 * (Shadow_Map_Width - 1), 0.5 * (Shadow_Map_Height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0));
        }

        // Export
        public void Export_Shadow_Map()
        {
            // Generate file name and path
            string export_path = $"{GetCurrentDirectory()}\\Export";
            if (!File.Exists(export_path))
            {
                CreateDirectory(export_path);
            }
            string export_name = $"\\{GetType().Name}_{ID}_Export_Map.bmp";

            Export_Shadow_Map(export_path + export_name);
        }

        public void Export_Shadow_Map(string file_path)
        {
            Trace.WriteLine($"Generating shadow map for {GetType().Name}...");

            using (Bitmap shadow_map_bitmap = new Bitmap(Shadow_Map_Width, Shadow_Map_Height))
            {
                for (int x = 0; x < Shadow_Map_Width; x++)
                {
                    for (int y = 0; y < Shadow_Map_Height; y++)
                    {
                        int value = Round_To_Int(255 * ((Shadow_Map[x][y] + 1) / 2));

                        Color greyscale_colour = Color.FromArgb(255, value, value, value);
                        shadow_map_bitmap.SetPixel(x, y, greyscale_colour);
                    }
                }

                shadow_map_bitmap.Save(file_path, ImageFormat.Bmp);
            }

            Trace.WriteLine($"Successfully saved shadow map for {GetType().Name}");
        }

        private static int Round_To_Int(double x) => (int)Math.Round(x, MidpointRounding.AwayFromZero);

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        #endregion
    }
}