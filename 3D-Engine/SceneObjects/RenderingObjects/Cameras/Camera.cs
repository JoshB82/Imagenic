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

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Transformations;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Camera"/>.
    /// </summary>
    public abstract partial class Camera : RenderingObject
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 ScreenToWorld { get; set; }

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            ScreenToWorld = ModelToWorld * ViewToScreen.Inverse();
        }

        internal static readonly ClippingPlane[] CameraScreenClippingPlanes = new ClippingPlane[]
        {
            new(-Vector3D.One, Vector3D.UnitX), // Left
            new(-Vector3D.One, Vector3D.UnitY), // Bottom
            new(-Vector3D.One, Vector3D.UnitZ), // Near
            new(Vector3D.One, Vector3D.UnitNegativeX), // Right
            new(Vector3D.One, Vector3D.UnitNegativeY), // Top
            new(Vector3D.One, Vector3D.UnitNegativeZ) // Far
        };    

        internal abstract void ProcessLighting();

        // Buffers
        protected Buffer2D<Color> colourBuffer;
        protected Buffer2D<float> zBuffer;

        // Matrices
        
        protected Matrix4x4 cameraScreenToWindowInverse;

        // Render
        internal Bitmap LastRender { get; set; }
        internal bool NewRenderNeeded { get; set; } = true;

        private Color renderBackgroundColour = Color.White;
        public Color RenderBackgroundColour
        {
            get => renderBackgroundColour;
            set
            {
                renderBackgroundColour = value;
                if (RenderCamera is not null) RenderCamera.NewRenderNeeded = true;
            }
        }

        protected int renderWidth, renderHeight;
        public int RenderWidth
        {
            get => renderWidth;
            set
            {
                renderWidth = value;
                UpdateProperties();
            }
        }
        public int RenderHeight
        {
            get => renderHeight;
            set
            {
                renderHeight = value;
                UpdateProperties();
            }
        }
        private void UpdateProperties()
        {
            colourBuffer = new(renderWidth, renderHeight);
            zBuffer = new(renderWidth, renderHeight);

            ScreenToWindow = Transform.Scale(0.5f * (renderWidth - 1), 0.5f * (renderHeight - 1), 1) * windowTranslate;
            cameraScreenToWindowInverse = ScreenToWindow.Inverse();

            NewRenderNeeded = true;
        }
        
        public void MakeRenderSizeOfControl(Control control)
        {
            RenderWidth = control.Width;
            RenderHeight = control.Height;
        }

        private PixelFormat renderPixelFormat = PixelFormat.Format24bppRgb; //????(and everywhere)
        public PixelFormat RenderPixelFormat
        {
            get => renderPixelFormat;
            set
            {
                if (value is PixelFormat.DontCare or PixelFormat.Extended or PixelFormat.Max or PixelFormat.Undefined)
                {
                    throw new Exception("Invalid PixelFormat for rendering"); //?
                }
                renderPixelFormat = value;
                NewRenderNeeded = true;
            }
        }

        // Scene
        private Group sceneToRender;
        public Group SceneToRender
        {
            get => sceneToRender;
            set
            {
                sceneToRender = value;
                sceneToRender.RenderCamera = this;
                NewRenderNeeded = true;
            }
        }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : base(origin, directionForward, directionUp)
        {
            string[] iconObjData = Properties.Resources.Camera.Split("\n");
            Icon = new Custom(origin, directionForward, directionUp, iconObjData)
            {
                Dimension = 3,
                FaceColour = Color.DarkCyan
            };
            Icon.Scale(5);
        }

        #endregion

        #region Methods

        public Bitmap Render(int width, int height)
        {
            RenderWidth = width;
            RenderHeight = height;

            return Render();
        }

        public Bitmap Render(Group sceneToRender)
        {
            SceneToRender = sceneToRender;

            return Render();
        }

        public Bitmap Render()
        {
            if (!NewRenderNeeded) return LastRender;
            
            if (SceneToRender is null) throw new NullReferenceException("No scene has been set to render.");
            
            // Calculate matrices and world origins
            foreach (Camera camera in SceneToRender.Cameras)
            {
                if (camera.Visible)
                {
                    if (camera.DrawIcon) camera.Icon.CalculateMatrices();
                    camera.CalculateMatrices();
                }
            }
            foreach (Light light in SceneToRender.Lights)
            {
                if (light.Visible)
                {
                    if (light.DrawIcon) light.Icon.CalculateMatrices();
                    light.CalculateMatrices();
                    light.CalculateWorldOrigin();
                }
            }
            foreach (Mesh mesh in SceneToRender.Meshes)
            {
                if (mesh.Visible)
                {
                    mesh.CalculateMatrices();
                }
            }
            this.CalculateWorldOrigin();

            // Generate a shadow map for each light (only if needed)
            foreach (Light light in SceneToRender.Lights)
            {
                if (light.ShadowMapNeedsUpdating && light.Visible)
                {
                    light.GenerateShadowMap(SceneToRender);

                    #if DEBUG

                    light.ExportShadowMap();
                        
                    #endif
                }
            }
            
            // Generate z buffer
            GenerateZBuffer(SceneToRender);

            // Process lighting
            ProcessLighting();

            NewRenderNeeded = false;
            return LastRender = CreateBitmap(RenderWidth, RenderHeight, colourBuffer, RenderPixelFormat);
        }

        // Lighting
        protected void ApplyLighting(
            Vector4D worldSpacePoint,
            ref Color pointColour, int x, int y)
        {
            bool lightApplied = false;

            foreach (Light light in SceneToRender.Lights)
            {
                if (light.Visible)
                {
                    // Move the point from world space to light-view space
                    Vector4D lightViewSpacePoint = light.WorldToView * worldSpacePoint;

                    Color newLightColour = light.Colour;
                    if (light is PointLight or Spotlight)
                    {
                        // Darken the Light's colour based on how far away the point is from the light
                        Vector3D light_to_point = (Vector3D)lightViewSpacePoint;
                        float distantIntensity = light.Strength / light_to_point.Squared_Magnitude();
                        newLightColour = light.Colour.Darken_Percentage(distantIntensity);
                    }

                    // Move the point from light-view space to light-screen space
                    Vector4D lightScreenSpacePoint = light.ViewToScreen * lightViewSpacePoint;
                    if (light is PointLight or Spotlight)
                    {
                        lightScreenSpacePoint /= lightScreenSpacePoint.w;
                    }

                    Vector4D lightWindowSpacePoint = light.ScreenToWindow * lightScreenSpacePoint;

                    // Round the points
                    int light_point_x = lightWindowSpacePoint.x.RoundToInt();
                    int light_point_y = lightWindowSpacePoint.y.RoundToInt();
                    float light_point_z = lightWindowSpacePoint.z;

                    //Trace.WriteLine("The following light point has been calculated: "+new Vector3D(light_point_x,light_point_y,light_point_z));

                    if (light_point_x >= 0 && light_point_x < light.ShadowMapWidth &&
                        light_point_y >= 0 && light_point_y < light.ShadowMapHeight)
                    {
                        if (light_point_z.Approx_Less_Than_Equals(light.ShadowMap.Values[light_point_x][light_point_y], 1E-4F))
                        {
                            // Point is not in shadow and light does contribute to the point's overall colour
                            pointColour = pointColour.Mix(newLightColour);
                            lightApplied = true;

                            /*if (light_point_z < -1) light_point_z = -1;
                            if (light_point_z > 1) light_point_z = 1;

                            int value = (255 * ((light_point_z + 1) / 2)).Round_to_Int();
                            Color greyscale_colour = Color.FromArgb(255, value, value, value);
                            bitmap.SetPixel(light_point_x, light_point_y, greyscale_colour);*/

                            // Trace.WriteLine("Lighting was added at "+new Vector3D(light_point_x,light_point_y,light_point_z)+" and the shadow map point z was: "+light.Shadow_Map[light_point_x][light_point_y]);
                        }
                    }
                }
            }

            // Update the colour buffer (use black if there are no lights affecting the point)
            colourBuffer.Values[x][y] = lightApplied ? pointColour : Color.Black;
        }

        #endregion
    }
}