/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Handles creation of a scene and contains rendering methods.
 */

using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Scene"/>.
    /// </summary>
    public sealed partial class Scene
    {
        #region Fields and Properties

        private Rectangle screen_rectangle;

        // Buffers
        private float[][] z_buffer;
        private Color[][] colour_buffer;
        private const byte out_of_bounds_value = 2;

        // Components
        /// <include file="Help_7.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Background_Colour']/*"/>
        public Color Background_Colour { get; set; } = Color.White;
        /// <include file="Help_7.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Canvas_Box']/*"/>
        public PictureBox Canvas_Box { get; set; }
        /// <summary>
        /// The <see cref="Camera"/> containing the view that will be rendered on the screen.
        /// </summary>
        public Camera Render_Camera { get; set; }

        private Bitmap new_frame;
        
        // Miscellaneous
        private static readonly object locker = new object();

        // Contents
        /// <include file="Help_7.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Cameras']/*"/>
        public readonly List<Camera> Cameras = new List<Camera>();
        /// <include file="Help_7.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Lights']/*"/>
        public readonly List<Light> Lights = new List<Light>();
        /// <include file="Help_7.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Meshes']/*"/>
        public readonly List<Mesh> Meshes = new List<Mesh>();

        // Dimensions
        private Matrix4x4 screen_to_window, screen_to_window_inverse;
        private static readonly Matrix4x4 window_translate = Transform.Translate(new Vector3D(1, 1, 0));

        private int width, height;

        /// <include file="Help_7.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Width']/*"/>
        public int Width
        {
            get => width;
            set
            {
                lock (locker)
                {
                    width = value;
                    Set_Dimensions();
                }
            }
        }
        /// <include file="Help_7.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Height']/*"/>
        public int Height
        {
            get => height;
            set
            {
                lock (locker)
                {
                    height = value;
                    Set_Dimensions();
                }
            }
        }

        private void Set_Dimensions()
        {
            // Set buffers
            z_buffer = new float[width][];
            colour_buffer = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                z_buffer[i] = new float[height];
                colour_buffer[i] = new Color[height];
            }

            // Set screen-to-window matrix
            screen_to_window = Transform.Scale(0.5f * (width - 1), 0.5f * (height - 1), 1) * window_translate;
            screen_to_window_inverse = screen_to_window.Inverse();
            screen_rectangle = new Rectangle(0, 0, width, height); //?-1?
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Scene"/>.
        /// </summary>
        /// <param name="canvas_box">The <see cref="PictureBox"/> where the <see cref="Scene"/> will be rendered.</param>
        /// <param name="width">The width of the <see cref="Scene"/>.</param>
        /// <param name="height">The height of the <see cref="Scene"/>.</param>
        public Scene(PictureBox canvas_box, int width, int height)
        {
            Canvas_Box = canvas_box;
            Width = width;
            Height = height;

            Reset_Light_Buffers();

            Trace.WriteLine("Scene created");
        }

        #endregion

        #region Add to scene methods

        /// <summary>
        /// Adds a <see cref="Scene_Object"/> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene_object"><see cref="Scene_Object"/> to add.</param>
        public void Add(Scene_Object scene_object)
        {
            lock (locker)
            {
                switch (scene_object)
                {
                    case Camera camera:
                        Cameras.Add(camera);
                        break;
                    case Light light:
                        Lights.Add(light);
                        break;
                    case Mesh mesh:
                        Meshes.Add(mesh);
                        break;
                }
            }
        }

        public void Add(Scene_Object[] scene_objects)
        {
            foreach (Scene_Object scene_object in scene_objects) Add(scene_object);
        }

        #endregion

        #region Remove from scene methods

        public void Remove(int ID)
        {
            lock (locker) Meshes.RemoveAll(x => x.ID == ID);
        }

        public void Remove(int start_ID, int finish_ID)
        {
            lock (locker)
            {
                for (int ID = start_ID; ID <= finish_ID; ID++)
                {
                    Meshes.RemoveAll(x => x.ID == ID);
                }
            }
        }

        #endregion

        /// <summary>
        /// Renders the <see cref="Scene"/>. The Render Camera must be set before this method is called.
        /// </summary>
        public void Render()
        {
            if (Render_Camera is null)
            {
                Trace.WriteLine("Error drawing frame: No render camera has been set yet!");
                return;
            }

            lock (locker)
            {
                // Create temporary canvas for this frame
                //if (new_frame is not null) new_frame.Dispose();
                new_frame = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                
                // Reset scene buffers
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        z_buffer[i][j] = out_of_bounds_value;
                        colour_buffer[i][j] = Background_Colour;
                    }
                }

                Reset_Light_Buffers();

                // Calculate necessary matrices for all scene objects
                Generate_Matrices();

                // Calculate depth information for each light
                foreach (Light light in Lights)
                {
                    if (light.Visible) Generate_Shadow_Map(light);
                }

                // Calculate depth information for each mesh
                foreach (Light light in Lights)
                {
                    if (light.Draw_Icon)
                    {
                        foreach (Face face in light.Icon.Faces)
                        {
                            Generate_Z_Buffer(face, 3, light.Icon.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                        }
                    }
                }
                foreach (Mesh mesh in Meshes)
                {
                    if (mesh.Visible && mesh.Draw_Faces)
                    {
                        foreach (Face face in mesh.Faces)
                        {
                            if (face.Visible)
                            {
                                Generate_Z_Buffer(face, mesh.Dimension, mesh.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                        }

                        if (mesh.Has_Direction_Arrows && mesh.Display_Direction_Arrows)
                        {
                            Arrow direction_forward = mesh.Direction_Arrows.Scene_Objects[0] as Arrow;
                            Arrow direction_up = mesh.Direction_Arrows.Scene_Objects[1] as Arrow;
                            Arrow direction_right = mesh.Direction_Arrows.Scene_Objects[2] as Arrow;

                            foreach (Face face in direction_forward.Faces)
                            {
                                Generate_Z_Buffer(face, 3, direction_forward.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                            foreach (Face face in direction_up.Faces)
                            {
                                Generate_Z_Buffer(face, 3, direction_up.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                            foreach (Face face in direction_right.Faces)
                            {
                                Generate_Z_Buffer(face, 3, direction_right.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                        }
                    }
                }
                
                // Apply lighting
                if (Render_Camera is Orthogonal_Camera)
                {
                    Matrix4x4 window_to_world = Render_Camera.Model_to_World * Render_Camera.Camera_View_to_Camera_Screen.Inverse() * screen_to_window_inverse;

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (z_buffer[x][y] != out_of_bounds_value)
                            {
                                // Move the point from window space to world space and apply lighting
                                Apply_Lighting(window_to_world * new Vector4D(x, y, z_buffer[x][y]), ref colour_buffer[x][y], x, y, null);
                            }
                        }
                    }
                }
                else
                {
                    /*Trace.WriteLine("start");
                    string file_path = "C:\\Users\\jbrya\\Desktop\\image3.bmp";
                    string file_directory = System.IO.Path.GetDirectoryName(file_path);
                    if (!System.IO.Directory.Exists(file_directory)) System.IO.Directory.CreateDirectory(file_directory);*/
                    
                    Bitmap shadow_map_bitmap = new Bitmap(Lights[0].Shadow_Map_Width, Lights[0].Shadow_Map_Height);

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            // check all floats and ints
                            if (z_buffer[x][y] != out_of_bounds_value)
                            {
                                SMC_Camera_Perspective(ref colour_buffer[x][y],
                                    screen_to_window_inverse,
                                    Render_Camera.Camera_Screen_to_World,
                                    x, y, z_buffer[x][y], shadow_map_bitmap);
                            }
                        }
                    }

                    //shadow_map_bitmap.Save(file_path, System.Drawing.Imaging.ImageFormat.Bmp);
                    //shadow_map_bitmap.Dispose();

                    Trace.WriteLine("finish");
                }
                
                // Draw edges
                foreach (Light light in Lights)
                {
                    if (light.Draw_Icon)
                    {
                        foreach (Edge edge in light.Icon.Edges)
                        {
                            Draw_Edge(edge, light.Icon.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                        }
                    }
                }
                foreach (Mesh mesh in Meshes)
                {
                    if (mesh.Draw_Edges)
                    {
                        foreach (Edge edge in mesh.Edges)
                        {
                            if (edge.Visible)
                            {
                                Draw_Edge(edge, mesh.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                        }

                        if (mesh.Has_Direction_Arrows && mesh.Display_Direction_Arrows)
                        {
                            Arrow direction_forward = (Arrow)mesh.Direction_Arrows.Scene_Objects[0];
                            Arrow direction_up = (Arrow)mesh.Direction_Arrows.Scene_Objects[1];
                            Arrow direction_right = (Arrow)mesh.Direction_Arrows.Scene_Objects[2];

                            foreach (Edge edge in direction_forward.Edges)
                            {
                                Draw_Edge(edge, direction_forward.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                            foreach (Edge edge in direction_up.Edges)
                            {
                                Draw_Edge(edge, direction_up.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                            foreach (Edge edge in direction_right.Edges)
                            {
                                Draw_Edge(edge, direction_right.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                            }
                        }
                    }
                }

                // Draw camera volumes
                foreach (Camera camera in Cameras)
                {
                    foreach (Edge edge in camera.Volume_Edges)
                    {
                        Draw_Edge(edge, camera.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                    }
                }
                
                // Draw light volumes
                foreach (Light light in Lights)
                {
                    foreach (Edge edge in light.Volume_Edges)
                    {
                        Draw_Edge(edge, light.Model_to_World, Render_Camera.World_to_Camera_View, Render_Camera.Camera_View_to_Camera_Screen);
                    }
                }

                // Draw all points
                Draw_Colour_Buffer(new_frame, colour_buffer);
                Canvas_Box.Image = new_frame;
            }
        }
        private unsafe void Draw_Colour_Buffer(Bitmap canvas, Color[][] new_colour_buffer) // source of this method?!
        {
            BitmapData data = canvas.LockBits(screen_rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; y++)
            {
                byte* row_start = (byte*)data.Scan0 + y * data.Stride;
                for (int x = 0; x < width; x++)
                {
                    row_start[x * 3] = new_colour_buffer[x][y * -1 + height - 1].B; // Blue
                    row_start[x * 3 + 1] = new_colour_buffer[x][y * -1 + height - 1].G; // Green
                    row_start[x * 3 + 2] = new_colour_buffer[x][y * -1 + height - 1].R; // Red
                }
            }

            canvas.UnlockBits(data);
        }

        // Reset light buffers
        private void Reset_Light_Buffers()
        {
            foreach (Light light in Lights)
            {
                for (int i = 0; i < light.Shadow_Map_Width; i++)
                {
                    for (int j = 0; j < light.Shadow_Map_Height; j++)
                    {
                        light.Shadow_Map[i][j] = out_of_bounds_value;
                    }
                }
            }
        }

        // Generate matrices
        public void Generate_Matrices()
        {
            foreach (Camera camera in Cameras) camera.Calculate_Matrices();
            foreach (Light light in Lights)
            {
                if (light.Draw_Icon) light.Icon.Calculate_Matrices();
                if (light.Visible) light.Calculate_Matrices();
            }
            foreach (Mesh mesh in Meshes)
            {
                if (mesh.Visible) mesh.Calculate_Matrices();
            }

            Render_Camera.Calculate_World_Origin();
            foreach (Light light in Lights) light.Calculate_World_Origin();
        }
    }
}