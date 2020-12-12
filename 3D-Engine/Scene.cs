/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a scene and contains rendering methods.
 */

using _3D_Engine.Rendering;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Scene"/>.
    /// </summary>
    public sealed partial class Scene
    {
        #region Fields and Properties

        private Rectangle screen_rectangle;

        private const byte outOfBoundsValue = 2;

        // Components
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Background_Colour']/*"/>
        public Color Background_Colour { get; set; } = Color.White;
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Canvas_Box']/*"/>
        public PictureBox Canvas_Box { get; set; }

        /// <summary>
        /// The <see cref="Camera"/> containing the view that will be rendered on the screen if no other camera is specifiedddddddddddddddddd.
        /// </summary>
        public Camera DefaultCamera { get; set; }

        private Bitmap canvas;

        // Buffers
        private Buffer2D<Color> colourBuffer;
        private Buffer2D<float> zBuffer;

        // Contents
        public readonly List<Scene_Object> SceneObjects = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Cameras']/*"/>
        public readonly List<Camera> Cameras = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Lights']/*"/>
        public readonly List<Light> Lights = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Meshes']/*"/>
        public readonly List<Mesh> Meshes = new();

        // Dimensions
        private Matrix4x4 screenToWindow, screenToWindowInverse;
        private static readonly Matrix4x4 window_translate = Transform.Translate(new Vector3D(1, 1, 0));
        private int width, height;

        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Width']/*"/>
        public int Width
        {
            get => width;
            set
            {
                lock (locker)
                {
                    width = value;
                    colourBuffer.FirstDimensionSize = width;
                    zBuffer.FirstDimensionSize = width;
                    
                    Update_Dimensions();
                }
            }
        }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Height']/*"/>
        public int Height
        {
            get => height;
            set
            {
                lock (locker)
                {
                    height = value;
                    colourBuffer.SecondDimensionSize = height;
                    zBuffer.SecondDimensionSize = height;

                    Update_Dimensions();
                }
            }
        }

        private void Update_Dimensions()
        {
            // Update screen-to-window matrices
            screenToWindow = Transform.Scale(0.5f * (width - 1), 0.5f * (height - 1), 1) * window_translate;
            screenToWindowInverse = screenToWindow.Inverse();
            screen_rectangle = new Rectangle(0, 0, width, height); //?-1?
        }

        // Miscellaneous
        private static readonly object locker = new();

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

            colourBuffer = new(width, height);
            zBuffer = new(width, height);

            Trace.WriteLine("Scene created");
        }

        #endregion

        #region Methods

        // Add to scene
        /// <summary>
        /// Adds a <see cref="Scene_Object"/> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene_object"><see cref="Scene_Object"/> to add.</param>
        public void Add(Scene_Object scene_object)
        {
            lock (locker) SceneObjects.Add(scene_object);
        }

        /// <summary>
        /// Adds multiple <see cref="Scene_Object">Scene_Objects</see> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene_objects"><see cref="IEnumerable"/> containing <see cref="Scene_Object">Scene_Objects</see> to add.</param>
        public void Add(IEnumerable<Scene_Object> scene_objects)
        {
            lock (locker)
            {
                foreach (Scene_Object scene_object in scene_objects)
                {
                    SceneObjects.Add(scene_object);
                }
            }
        }

        // Remove from scene
        public void Remove(int ID)
        {
            lock (locker) SceneObjects.RemoveAll(x => x.ID == ID);
        }

        public void Remove(int start_ID, int finish_ID)
        {
            lock (locker)
            {
                for (int ID = start_ID; ID <= finish_ID; ID++)
                {
                    SceneObjects.RemoveAll(x => x.ID == ID);
                }
            }
        }

        // vv!!
        /// <summary>
        /// Renders the <see cref="Scene"/>. The Render Camera must be set before this method is called.
        /// </summary>
        /// 


        public void Render() => Render(DefaultCamera);
        public void Render(Camera renderCamera)
        {
            if (renderCamera is null)
            {
                Trace.WriteLine("Error: Cannot render camera set to null.");
                return;
            }

            lock (locker)
            {
                // Create temporary canvas for this frame
                //if (new_frame is not null) new_frame.Dispose();
                canvas = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                // Reset scene buffers
                zBuffer.SetAllToValue(outOfBoundsValue);
                colourBuffer.SetAllToValue(Background_Colour);
                foreach (Light light in Lights) light.Shadow_Map.SetAllToValue(outOfBoundsValue);

                // Calculate necessary matrices for all scene objects
                Generate_Matrices();

                // Calculate depth information for each light
                foreach (Light light in Lights)
                {
                    if (light.Visible) Generate_Shadow_Map(light);
                }

                // Calculate depth information for each mesh
                foreach (Scene_Object sceneObject in SceneObjects)
                {
                    switch (sceneObject)
                    {
                        case Camera camera when camera.Draw_Icon:
                            Matrix4x4 modelToCameraView = renderCamera.World_to_Camera_View * camera.Icon.Model_to_World;

                            foreach (Face face in camera.Icon.Faces)
                            {
                                Generate_Z_Buffer
                                (
                                    face,
                                    3,
                                    ref modelToCameraView,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                        case Light light when light.Draw_Icon:
                            modelToCameraView = renderCamera.World_to_Camera_View * light.Icon.Model_to_World;

                            foreach (Face face in light.Icon.Faces)
                            {
                                Generate_Z_Buffer
                                (
                                    face,
                                    3,
                                    ref modelToCameraView,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                        case Mesh mesh when mesh.Visible && mesh.Draw_Faces:
                            modelToCameraView = renderCamera.World_to_Camera_View * mesh.Model_to_World;

                            foreach (Face face in mesh.Faces)
                            {
                                if (face.Visible)
                                {
                                    Generate_Z_Buffer
                                    (
                                        face,
                                        mesh.Dimension,
                                        ref modelToCameraView,
                                        ref renderCamera.Camera_View_to_Camera_Screen
                                    );
                                }
                            }
                            break;
                    }
                
                    if (sceneObject.Has_Direction_Arrows && sceneObject.Display_Direction_Arrows)
                    {
                        Arrow directionForward = sceneObject.Direction_Arrows.Scene_Objects[0] as Arrow;
                        Arrow directionUp = sceneObject.Direction_Arrows.Scene_Objects[1] as Arrow;
                        Arrow directionRight = sceneObject.Direction_Arrows.Scene_Objects[2] as Arrow;

                        Matrix4x4 direction_forward_model_to_camera_view = renderCamera.World_to_Camera_View * directionForward.Model_to_World;
                        Matrix4x4 direction_up_model_to_camera_view = renderCamera.World_to_Camera_View * directionUp.Model_to_World;
                        Matrix4x4 direction_right_model_to_camera_view = renderCamera.World_to_Camera_View * directionRight.Model_to_World;

                        foreach (Face face in directionForward.Faces)
                        {
                            Generate_Z_Buffer
                            (
                                face,
                                3,
                                ref direction_forward_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                        foreach (Face face in directionUp.Faces)
                        {
                            Generate_Z_Buffer
                            (
                                face,
                                3,
                                ref direction_up_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                        foreach (Face face in directionRight.Faces)
                        {
                            Generate_Z_Buffer
                            (
                                face,
                                3,
                                ref direction_right_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                    }
                }
                
                // Apply lighting
                switch (renderCamera)
                {
                    case Orthogonal_Camera orthogonal_camera:
                        Matrix4x4 window_to_world = renderCamera.Model_to_World * renderCamera.Camera_View_to_Camera_Screen.Inverse() * screenToWindowInverse;

                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                if (zBuffer.Values[x][y] != outOfBoundsValue)
                                {
                                    // Move the point from window space to world space and apply lighting
                                    Apply_Lighting(window_to_world * new Vector4D(x, y, zBuffer.Values[x][y], 1), ref colourBuffer.Values[x][y], x, y, null);
                                }
                            }
                        }
                        break;
                    case Perspective_Camera perspective_camera:
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
                                if (zBuffer.Values[x][y] != outOfBoundsValue)
                                {
                                    SMC_Camera_Perspective
                                    (
                                        x, y, zBuffer.Values[x][y],
                                        ref colourBuffer.Values[x][y],
                                        ref screenToWindowInverse,
                                        ref renderCamera.Camera_Screen_to_World,
                                        shadow_map_bitmap
                                    );
                                }
                            }
                        }

                        //shadow_map_bitmap.Save(file_path, System.Drawing.Imaging.ImageFormat.Bmp);
                        //shadow_map_bitmap.Dispose();

                        //Trace.WriteLine("finish");
                        break;
                }
                
                // Draw edges
                foreach (Scene_Object scene_object in SceneObjects)
                {
                    switch (scene_object)
                    {
                        case Camera camera when camera.Draw_Icon:
                            Matrix4x4 model_to_camera_view = renderCamera.World_to_Camera_View * camera.Icon.Model_to_World;

                            foreach (Edge edge in camera.Icon.Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                        case Light light when light.Draw_Icon:
                            model_to_camera_view = renderCamera.World_to_Camera_View * light.Icon.Model_to_World;

                            foreach (Edge edge in light.Icon.Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                        case Mesh mesh when mesh.Visible && mesh.Draw_Edges:

                            model_to_camera_view = renderCamera.World_to_Camera_View * mesh.Model_to_World;

                            foreach (Edge edge in mesh.Edges)
                            {
                                if (edge.Visible)
                                {
                                    Draw_Edge
                                    (
                                        edge,
                                        ref model_to_camera_view,
                                        ref renderCamera.Camera_View_to_Camera_Screen
                                    );
                                }
                            }
                            break;
                    }

                    if (scene_object.Has_Direction_Arrows && scene_object.Display_Direction_Arrows)
                    {
                        Arrow direction_forward = scene_object.Direction_Arrows.Scene_Objects[0] as Arrow;
                        Arrow direction_up = scene_object.Direction_Arrows.Scene_Objects[1] as Arrow;
                        Arrow direction_right = scene_object.Direction_Arrows.Scene_Objects[2] as Arrow;

                        Matrix4x4 direction_forward_model_to_camera_view = renderCamera.World_to_Camera_View * direction_forward.Model_to_World;
                        Matrix4x4 direction_up_model_to_camera_view = renderCamera.World_to_Camera_View * direction_up.Model_to_World;
                        Matrix4x4 direction_right_model_to_camera_view = renderCamera.World_to_Camera_View * direction_right.Model_to_World;

                        foreach (Edge edge in direction_forward.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_forward_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                        foreach (Edge edge in direction_up.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_up_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                        foreach (Edge edge in direction_right.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_right_model_to_camera_view,
                                ref renderCamera.Camera_View_to_Camera_Screen
                            );
                        }
                    }
                }

                // Draw view volumes
                foreach (Scene_Object scene_object in SceneObjects)
                {
                    switch (scene_object)
                    {
                        case Camera camera when camera.Volume_Style != Volume_Outline.None:
                            Matrix4x4 model_to_camera_view = renderCamera.World_to_Camera_View * camera.Model_to_World;

                            foreach (Edge edge in camera.Volume_Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                        case Light light when light.Volume_Style != Volume_Outline.None:
                            model_to_camera_view = renderCamera.World_to_Camera_View * light.Model_to_World;

                            foreach (Edge edge in light.Volume_Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.Camera_View_to_Camera_Screen
                                );
                            }
                            break;
                    }
                }

                // Draw all points
                Draw_Colour_Buffer(canvas, colourBuffer.Values);
                Canvas_Box.Image = canvas;
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

        // Generate matrices
        public void Generate_Matrices()
        {
            foreach (Scene_Object sceneObject in SceneObjects)
            {
                switch (sceneObject)
                {
                    case Camera camera when camera.Visible:
                        if (camera.Draw_Icon) camera.Icon.Calculate_Matrices();
                        camera.Calculate_Matrices();
                        break;
                    case Light light when light.Visible:
                        if (light.Draw_Icon) light.Icon.Calculate_Matrices();
                        light.Calculate_Matrices();
                        light.Calculate_World_Origin();
                        break;
                    case Mesh mesh when mesh.Visible:
                        mesh.Calculate_Matrices();
                        break;
                }
            }

            Render_Camera.Calculate_World_Origin();
        }

        #endregion
    }
}