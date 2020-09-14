using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _3D_Engine
{
    public abstract partial class Light : Scene_Object
    {
        #region Fields and Properties

        // Appearance
        public Color Colour { get; set; } = Color.White;
        public Mesh Icon { get; protected set; }
        public float Strength { get; set; }

        /// <summary>
        /// Determines if the <see cref="Light"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool Draw_Icon { get; set; } = false;

        public View_Outline View_Style = View_Outline.Entire;

        /// <summary>
        /// Determines if the outline of the <see cref="Light">Light's</see> projection is drawn.
        /// </summary>
        
        /// <summary>
        /// Determines if the outline of the <see cref="Light">Light's</see> projection is drawn, up to the near plane.
        /// </summary>
        

        // Matrices
        internal Matrix4x4 World_to_Light_View { get; private set; }

        internal Matrix4x4 Light_View_to_Light_Screen;
        internal Matrix4x4 Light_Screen_to_Light_Window { get; private set; }

        internal void Calculate_World_To_Light_View() => World_to_Light_View = Model_to_World.Inverse();

        // Clipping planes
        internal Clipping_Plane[] Light_View_Clipping_Planes;

        // Shadow map volume
        internal float[][] Shadow_Map;
        public abstract int Shadow_Map_Width { get; set; }
        public abstract int Shadow_Map_Height { get; set; }
        public abstract float Shadow_Map_Z_Near { get; set; }
        public abstract float Shadow_Map_Z_Far { get; set; }

        protected void Set_Shadow_Map()
        {
            // Set shadow map
            Shadow_Map = new float[Shadow_Map_Width][];
            for (int i = 0; i < Shadow_Map_Width; i++) Shadow_Map[i] = new float[Shadow_Map_Height];
            
            // Set light-screen-to-light-window matrix
            Light_Screen_to_Light_Window = Transform.Scale(0.5f * (Shadow_Map_Width - 1), 0.5f * (Shadow_Map_Height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0));
        }

        // Export
        /// <summary>
        /// Exports the shadow map to the current working directory of the application, in an "Export" folder.
        /// </summary>
        /// <remarks>The folder is created if it does not exist.</remarks>
        public void Export_Shadow_Map() => Export_Shadow_Map($"{Directory.GetCurrentDirectory()}\\Export\\{GetType().Name}_{ID}_Export_Map.bmp");

        /// <summary>
        /// Exports the shadow map to the specified file path.
        /// </summary>
        /// <remarks>The specified folder is created if it does not exist.</remarks>
        /// <param name="file_path">Destination of the exported image.</param>
        public void Export_Shadow_Map(string file_path)
        {
            Trace.WriteLine($"Generating shadow map for {GetType().Name}...");

            string file_directory = Path.GetDirectoryName(file_path);
            if (!Directory.Exists(file_directory)) Directory.CreateDirectory(file_directory);

            using (Bitmap shadow_map_bitmap = new Bitmap(Shadow_Map_Width, Shadow_Map_Height))
            {
                for (int x = 0; x < Shadow_Map_Width; x++)
                {
                    for (int y = 0; y < Shadow_Map_Height; y++)
                    {
                        int value = (255 * ((Shadow_Map[x][y] + 1) / 2)).Round_to_Int();

                        Color greyscale_colour = Color.FromArgb(255, value, value, value);
                        shadow_map_bitmap.SetPixel(x, y, greyscale_colour);
                    }
                }

                shadow_map_bitmap.Save(file_path, ImageFormat.Bmp);
            }

            Trace.WriteLine($"Successfully saved shadow map for {GetType().Name}");
        }

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        #endregion
    }
}