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

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Transformations;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using _3D_Engine.SceneObjects.Lights;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Scene"/>.
    /// </summary>
    public sealed partial class Scene
    {
        #region Fields and Properties

        private Rectangle screenRectangle;

        // Components
        /// <summary>
        /// The background <see cref="Color"/> of the <see cref="Scene"/>.
        /// </summary>
        public Color BackgroundColour { get; set; } = Color.White;

        /// <summary>
        /// The <see cref="Camera"/> containing the view that will be rendered on the screen if no other camera is specifiedddddddddddddddddd.
        /// </summary>
        public Camera DefaultCamera { get; set; }

        internal bool ShadowMapsNeedUpdating { get; set; }
        
        

        // Canvas
        private Bitmap canvas;
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Scene.Canvas_Box']/*"/>
        public PictureBox Canvas_Box { get; set; }

        // Contents (check below comments)
        public readonly List<SceneObject> SceneObjects = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Cameras']/*"/>
        public readonly List<Camera> Cameras = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Lights']/*"/>
        public readonly List<Light> Lights = new();
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Scene.Meshes']/*"/>
        public readonly List<Mesh> Meshes = new();

        // Dimensions
        private Matrix4x4 screenToWindow, screenToWindowInverse;
        
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
            
            screenRectangle = new Rectangle(0, 0, width, height); //?-1?
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

            

            Trace.WriteLine("Scene created");
        }

        #endregion

        #region Methods

        // Add to scene
        /// <summary>
        /// Adds a <see cref="SceneObject"/> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="SceneObject"/> to add.</param>
        public void Add(SceneObject sceneObject)
        {
            lock (locker)
            {
                SceneObjects.Add(sceneObject);
                if (sceneObject is Camera)
                {
                    (sceneObject as Camera).ParentScene = this;
                }
            }
        }

        /// <summary>
        /// Adds multiple <see cref="SceneObject">Scene_Objects</see> to the <see cref="Scene"/>.
        /// </summary>
        /// <param name="sceneObjects"><see cref="IEnumerable"/> containing <see cref="SceneObject">SceneObjects</see> to add.</param>
        public void Add(IEnumerable<SceneObject> sceneObjects)
        {
            lock (locker)
            {
                foreach (SceneObject sceneObject in sceneObjects)
                {
                    SceneObjects.Add(sceneObject);
                    if (sceneObject is Camera)
                    {
                        (sceneObject as Camera).ParentScene = this;
                    }
                }
            }
        }

        // Remove from scene
        /// <summary>
        /// Removes a <see cref="SceneObject"/> from the <see cref="Scene"/> that matches the specified id.
        /// </summary>
        /// <param name="Id">Id of the <see cref="SceneObject"/> to be removed.</param>
        public void Remove(int Id)
        {
            lock (locker) SceneObjects.RemoveAll(x => x.Id == Id);
        }

        public void Remove(int start_ID, int finish_ID)
        {
            lock (locker)
            {
                for (int ID = start_ID; ID <= finish_ID; ID++)
                {
                    SceneObjects.RemoveAll(x => x.Id == ID);
                }
            }
        }
        
        /// <summary>
        /// Renders the <see cref="Scene"/> with the <see cref="DefaultCamera">DefaultCamera</see>.
        /// </summary>
        public void Render() => Render(DefaultCamera);
        /// <summary>
        /// Renders the <see cref="Scene"/> with the specified <see cref="Camera"/>.
        /// </summary>
        /// <param name="renderCamera">The <see cref="Camera"/> to render the <see cref="Scene"/> from.</param>
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

                
                

                

                

                
                
                
                
                // Draw edges
                foreach (SceneObject sceneObject in SceneObjects)
                {
                    switch (sceneObject)
                    {
                        case Camera camera when camera.DrawIcon:
                            Matrix4x4 model_to_camera_view = renderCamera.WorldToCameraView * camera.Icon.ModelToWorld;

                            foreach (Edge edge in camera.Icon.Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.CameraViewToCameraScreen
                                );
                            }
                            break;
                        case Light light when light.DrawIcon:
                            model_to_camera_view = renderCamera.WorldToCameraView * light.Icon.ModelToWorld;

                            foreach (Edge edge in light.Icon.Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.CameraViewToCameraScreen
                                );
                            }
                            break;
                        case Mesh mesh when mesh.Visible && mesh.Draw_Edges:

                            model_to_camera_view = renderCamera.WorldToCameraView * mesh.ModelToWorld;

                            foreach (Edge edge in mesh.Edges)
                            {
                                if (edge.Visible)
                                {
                                    Draw_Edge
                                    (
                                        edge,
                                        ref model_to_camera_view,
                                        ref renderCamera.CameraViewToCameraScreen
                                    );
                                }
                            }
                            break;
                    }

                    if (sceneObject.HasDirectionArrows && sceneObject.DisplayDirectionArrows)
                    {
                        Arrow direction_forward = sceneObject.DirectionArrows.SceneObjects[0] as Arrow;
                        Arrow direction_up = sceneObject.DirectionArrows.SceneObjects[1] as Arrow;
                        Arrow direction_right = sceneObject.DirectionArrows.SceneObjects[2] as Arrow;

                        Matrix4x4 direction_forward_model_to_camera_view = renderCamera.WorldToCameraView * direction_forward.ModelToWorld;
                        Matrix4x4 direction_up_model_to_camera_view = renderCamera.WorldToCameraView * direction_up.ModelToWorld;
                        Matrix4x4 direction_right_model_to_camera_view = renderCamera.WorldToCameraView * direction_right.ModelToWorld;

                        foreach (Edge edge in direction_forward.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_forward_model_to_camera_view,
                                ref renderCamera.CameraViewToCameraScreen
                            );
                        }
                        foreach (Edge edge in direction_up.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_up_model_to_camera_view,
                                ref renderCamera.CameraViewToCameraScreen
                            );
                        }
                        foreach (Edge edge in direction_right.Edges)
                        {
                            Draw_Edge
                            (
                                edge,
                                ref direction_right_model_to_camera_view,
                                ref renderCamera.CameraViewToCameraScreen
                            );
                        }
                    }
                }

                // Draw view volumes
                foreach (SceneObject scene_object in SceneObjects)
                {
                    switch (scene_object)
                    {
                        case Camera camera when camera.VolumeStyle != VolumeOutline.None:
                            Matrix4x4 model_to_camera_view = renderCamera.WorldToCameraView * camera.ModelToWorld;

                            foreach (Edge edge in camera.VolumeEdges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.CameraViewToCameraScreen
                                );
                            }
                            break;
                        case Light light when light.Volume_Style != VolumeOutline.None:
                            model_to_camera_view = renderCamera.WorldToCameraView * light.ModelToWorld;

                            foreach (Edge edge in light.Volume_Edges)
                            {
                                Draw_Edge
                                (
                                    edge,
                                    ref model_to_camera_view,
                                    ref renderCamera.CameraViewToCameraScreen
                                );
                            }
                            break;
                    }
                }

                // Draw colour buffer on canvas
                Draw_Colour_Buffer(canvas, colourBuffer.Values);
                Canvas_Box.Image = canvas;
            }
        }
        private unsafe void Draw_Colour_Buffer(Bitmap canvas, Color[][] new_colour_buffer) // source of this method?!
        {
            
        }

        // Generate matrices
        public void GenerateMatrices()
        {
            
        }

        #endregion
    }
}