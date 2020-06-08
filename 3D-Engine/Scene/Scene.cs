using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace _3D_Engine
{
    public abstract class Scene_Object { } // Used to group scene objects together.

    public sealed partial class Scene
    {
        private static readonly object locker = new object();
        private Clipping_Plane[] screen_clipping_planes;

        public readonly List<Camera> Camera_List = new List<Camera>();
        public readonly List<Light> Light_List = new List<Light>();
        public readonly List<Shape> Shape_List = new List<Shape>();

        public Camera Render_Camera { get; set; }
        public PictureBox Canvas_Box { get; set; }
        public Bitmap Canvas { get; private set; }
        public Color Background_colour { get; set; }

        // Buffers
        private double[][] z_buffer;
        private Color[][] colour_buffer;

        public bool Change_scene { get; set; } = true;

        #region Dimensions

        private Matrix4x4 screen_to_window;
        private int width, height;
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
        /// Create a new scene.
        /// </summary>
        /// <param name="canvas_box">Picture box where the scene will be rendered.</param>
        /// <param name="width">Width of scene.</param>
        /// <param name="height">Height of scene.</param>
        public Scene(PictureBox canvas_box, int width, int height)
        {
            Canvas_Box = canvas_box;
            Width = width;
            Height = height;

            Background_colour = Color.White;
            Canvas = new Bitmap(width, height);

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

            Debug.WriteLine("Scene created.");
        }

        #region Add to scene methods

        /// <summary>
        /// Add an object to the scene.
        /// </summary>
        /// <param name="object">Object to add.</param>
        public void Add(Scene_Object @object)
        {
            switch (@object.GetType().Name)
            {
                case "Orthogonal_Camera":
                case "Perspective_Camera":
                    Camera_List.Add((Camera)@object);
                    break;
                case "Light":
                    Light_List.Add((Light)@object);
                    break;
                case "Shape":
                    Shape_List.Add((Shape)@object);
                    break;
            }
        }

        // Probably not working
        /// <summary>
        /// Add objects to the scene.
        /// </summary>
        /// <param name="objects">Array of objects to add.</param>
        public void Add(Scene_Object[] objects)
        {
            switch (objects.GetType().Name)
            {
                case "Orthogonal_Camera[]":
                case "Perspective_Camera[]":
                    foreach (object camera in objects) Camera_List.Add((Camera)camera);
                    break;
                case "Light[]":
                    foreach (object light in objects) Light_List.Add((Light)light);
                    break;
                case "Shape[]":
                    foreach (object shape in objects) Shape_List.Add((Shape)shape);
                    break;
            }
        }

        #endregion

        public void Remove(int ID)
        {
            lock (locker) Shape_List.RemoveAll(x => x.ID == ID);
        }

        public void Render()
        {
            if (Render_Camera == null) throw new Exception("No camera has been set yet!");

            // Only render if a change in scene has taken place
            // if (!Change_scene) return;

            lock (locker)
            {
                // Create temporary canvas for this frame
                Bitmap temp_canvas = new Bitmap(Width, Height);

                // Reset buffers
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) z_buffer[i][j] = 2; // 2 is always greater than anything to be rendered??///
                for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) colour_buffer[i][j] = Background_colour;

                // Calculate camera properties
                Render_Camera.Calculate_Model_to_World_Matrix();
                Render_Camera.World_Origin = new Vector3D(Render_Camera.Model_to_World * Render_Camera.Origin);
                Render_Camera.Calculate_World_to_View_Matrix();
                string camera_type = Render_Camera.GetType().Name;
                Matrix4x4 world_to_view = Render_Camera.World_to_View;
                Matrix4x4 view_to_screen = Render_Camera.View_to_Screen;

                // Draw graphics
                using (Graphics g = Graphics.FromImage(temp_canvas))
                {
                    // Draw shapes
                    foreach (Shape shape in Shape_List)
                    {
                        Mesh shape_mesh = shape.Render_Mesh;

                        // Calculate shape matrix
                        shape_mesh.Calculate_Model_to_World_Matrix();
                        Matrix4x4 model_to_world = shape_mesh.Model_to_World;

                        shape_mesh.Origin = screen_to_window * view_to_screen * world_to_view * model_to_world * shape_mesh.Origin;
                        
                        string shape_type = shape_mesh.GetType().Name;

                        // Draw faces
                        if (shape_mesh.Draw_Faces && shape_mesh.Visible)
                        {
                            foreach (Face face in shape.Render_Mesh.Faces)
                            {
                                if (face.Visible) Draw_Face(face, camera_type, shape_type, model_to_world, world_to_view, view_to_screen);
                            }
                        }

                        // Draw edges
                        if (shape_mesh.Draw_Edges && shape_mesh.Visible)
                        {
                            foreach (Edge edge in shape.Render_Mesh.Edges)
                            {
                                if (edge.Visible) Draw_Edge(edge, camera_type, model_to_world, world_to_view, view_to_screen);
                            }
                        }

                        // Draw spots
                        if (shape_mesh.Draw_Spots && shape_mesh.Visible)
                        {
                            foreach (Spot spot in shape.Render_Mesh.Spots) if (spot.Visible) Draw_Spot(spot, Render_Camera);
                        }
                    }

                    // Draw camera views
                    foreach (Camera camera_to_draw in Camera_List)
                    {
                        // Calculate camera matrix
                        camera_to_draw.Calculate_Model_to_World_Matrix();
                        Matrix4x4 model_to_world = camera_to_draw.Model_to_World;
                        
                        Draw_Camera(camera_to_draw, camera_type, model_to_world, world_to_view, view_to_screen);
                    }

                    // Draw each pixel from the colour buffer
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            using (SolidBrush face_brush = new SolidBrush(colour_buffer[x][y])) g.FillRectangle(face_brush, x, y * -1 + Height - 1, 1, 1);
                        }
                    }
                }

                Canvas = temp_canvas;
                Canvas_Box.Invalidate();
                Change_scene = false;
            }
        }
    }
}