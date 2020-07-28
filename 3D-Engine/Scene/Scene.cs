using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private static readonly object locker = new object();
        private Clipping_Plane[] screen_clipping_planes;
        private Rectangle entire_canvas_rectangle;

        // Buffers
        private double[][] z_buffer;
        private Color[][] colour_buffer;

        public Camera Render_Camera { get; set; }
        /// <summary>
        /// <see cref="PictureBox"/> where the <see cref="Scene"/> will be rendered.
        /// </summary>
        public PictureBox Canvas_Box { get; set; }
        public Bitmap Canvas { get; private set; }
        /// <summary>
        /// The background <see cref="Color"/> of the <see cref="Scene"/>.
        /// </summary>
        public Color Background_Colour { get; set; } = Color.White;

        /// <summary>
        /// List of all <see cref="Camera"/>s in the current <see cref="Scene"/>.
        /// </summary>
        public readonly List<Camera> Cameras = new List<Camera>();
        /// <summary>
        /// List of all <see cref="Light"/>s in the current <see cref="Scene"/>.
        /// </summary>
        public readonly List<Light> Lights = new List<Light>();
        /// <summary>
        /// List of all <see cref="Mesh"/>es in the current <see cref="Scene"/>.
        /// </summary>
        public readonly List<Mesh> Meshes = new List<Mesh>();
        
        public bool Change_scene { get; set; } = true;

        #region Dimensions

        private Matrix4x4 screen_to_window;

        private int width, height;
        /// <summary>
        /// The width of the <see cref="Scene"/>.
        /// </summary>
        public int Width
        {
            get => width;
            set
            {
                lock (locker)
                {
                    width = value;
                    screen_to_window = Transform.Scale(0.5 * (width - 1), 0.5 * (height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0));
                    Set_Buffer();
                }
            }
        }
        /// <summary>
        /// The height of the <see cref="Scene"/>.
        /// </summary>
        public int Height
        {
            get => height;
            set
            {
                lock (locker)
                {
                    height = value;
                    screen_to_window = Transform.Scale(0.5 * (width - 1), 0.5 * (height - 1), 1) * Transform.Translate(new Vector3D(1, 1, 0));
                    Set_Buffer();
                }
            }
        }

        private void Set_Buffer()
        {
            z_buffer = new double[width][];
            colour_buffer = new Color[width][];
            for (int i = 0; i < width; i++) z_buffer[i] = new double[height];
            for (int i = 0; i < width; i++) colour_buffer[i] = new Color[height];
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="Scene"/>.
        /// </summary>
        /// <param name="canvas_box"><see cref="PictureBox"/> where the <see cref="Scene"/> will be rendered.</param>
        /// <param name="width">The width of the <see cref="Scene"/>.</param>
        /// <param name="height">The height of the <see cref="Scene"/>.</param>
        public Scene(PictureBox canvas_box, int width, int height)
        {
            Canvas_Box = canvas_box;
            Width = width;
            Height = height;

            Canvas = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            entire_canvas_rectangle = new Rectangle(0, 0, width, height);

            Vector3D near_bottom_left_point = new Vector3D(-1, -1, -1), far_top_right_point = new Vector3D(1, 1, 1);
            screen_clipping_planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_X), // Left
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Y), // Bottom
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near
                new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_X), // Right
                new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Y), // Top
                new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Z) // Far
            };

            Debug.WriteLine("Scene created");
        }

        #region Add to scene methods
        
        /// <summary>
        /// Adds a <see cref="Scene_Object"/> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene_object"><see cref="Scene_Object"/> to add.</param>
        public void Add(Scene_Object scene_object)
        {
            switch(scene_object.GetType().BaseType.Name)
            {
                case "Camera":
                    Cameras.Add((Camera)scene_object);
                    break;
                case "Light":
                    Lights.Add((Light)scene_object);
                    break;
                case "Mesh":
                    Meshes.Add((Mesh)scene_object);
                    break;
            }
        }

        // Probably not working (review add method code)
        public void Add(Scene_Object[] scene_objects)
        {
            switch (scene_objects.GetType().Name)
            {
                case "Orthogonal_Camera[]":
                case "Perspective_Camera[]":
                    foreach (Camera camera in scene_objects) Cameras.Add(camera);
                    break;
                case "Light[]":
                    foreach (Light light in scene_objects) Lights.Add(light);
                    break;
                case "Mesh[]":
                    foreach (Mesh mesh in scene_objects) Meshes.Add(mesh);
                    break;
            }
        }

        #endregion

        public void Remove(int ID)
        {
            lock (locker) Meshes.RemoveAll(x => x.ID == ID);
        }

        /// <summary>
        /// Renders the <see cref="Scene"/>. A Render Camera must be set before this method is called.
        /// </summary>
        public void Render()
        {
            if (Render_Camera == null)
            {
                Debug.WriteLine("Failed to draw frame: No camera has been set yet!");
                return;
            }
            
            // Only render if a change in scene has taken place
            // if (!Change_scene) return;

            lock (locker)
            {
                // Create temporary canvas for this frame
                Bitmap temp_canvas = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                // Reset buffers
                for (int i = 0; i < width; i++) for (int j = 0; j < height; j++)
                {
                    z_buffer[i][j] = 2;
                    colour_buffer[i][j] = Background_Colour;
                }

                // Calculate camera properties
                Render_Camera.Calculate_Model_to_World_Matrix();
                Render_Camera.World_Origin = new Vector3D(Render_Camera.Model_to_World * Render_Camera.Origin);
                Render_Camera.Calculate_World_to_View_Matrix();
                string camera_type = Render_Camera.GetType().Name;
                Matrix4x4 world_to_view = Render_Camera.World_to_View;
                Matrix4x4 view_to_screen = Render_Camera.View_to_Screen;

                // Draw meshes
                foreach (Mesh mesh in Meshes)
                {
                    mesh.Calculate_Model_to_World_Matrix();
                    Matrix4x4 model_to_world = mesh.Model_to_World;

                    mesh.Origin = screen_to_window * view_to_screen * world_to_view * model_to_world * mesh.Origin;
                        
                    string mesh_type = mesh.GetType().Name;

                    if (mesh.Visible)
                    {
                        // Draw faces
                        if (mesh.Draw_Faces)
                        {
                            foreach (Face face in mesh.Faces)
                            {
                                if (face.Visible) Draw_Face(face, camera_type, mesh_type, model_to_world, world_to_view, view_to_screen, Render_Camera);
                            }
                        }

                        // Draw edges
                        if (mesh.Draw_Edges)
                        {
                            foreach (Edge edge in mesh.Edges)
                            {
                                if (edge.Visible) Draw_Edge(edge, camera_type, model_to_world, world_to_view, view_to_screen);
                            }
                        }

                        // Draw spots
                        if (mesh.Draw_Spots)
                        {
                            foreach (Spot spot in mesh.Spots) if (spot.Visible) Draw_Spot(spot, Render_Camera);
                        }
                    }
                }

                // Draw camera views
                foreach (Camera camera_to_draw in Cameras)
                {
                    // Calculate camera matrix
                    camera_to_draw.Calculate_Model_to_World_Matrix();
                    Matrix4x4 model_to_world = camera_to_draw.Model_to_World;
                        
                    Draw_Camera(camera_to_draw, camera_type, model_to_world, world_to_view, view_to_screen);
                }

                Draw_Colour_Buffer(temp_canvas, colour_buffer);

                Canvas = temp_canvas;
                Canvas_Box.Invalidate();
                Change_scene = false;
            }
        }

        private unsafe void Draw_Colour_Buffer(Bitmap canvas, Color[][] colour_buffer)
        {
            BitmapData data = canvas.LockBits(entire_canvas_rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; y++)
            {
                byte* row_start = (byte*)data.Scan0 + y * data.Stride;
                for (int x = 0; x < width; x++)
                {
                    row_start[x * 3] = colour_buffer[x][y * -1 + height - 1].B; // Blue
                    row_start[x * 3 + 1] = colour_buffer[x][y * -1 + height - 1].G; // Green
                    row_start[x * 3 + 2] = colour_buffer[x][y * -1 + height - 1].R; // Red
                }
            }

            canvas.UnlockBits(data);
        }
    }
}