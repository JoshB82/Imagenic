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

using _3D_Engine.Constants;
using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using Imagenic.Core.Renderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="Camera"/>. Any object which inherits from this class provides camera functionality.
    /// </summary>
    public abstract partial class Camera : RenderingObject
    {
        #region Fields and Properties

        // Buffers
        protected Buffer2D<Color> colourBuffer;

        // Lighting
        public Spotlight Headlamp { get; set; }
        internal abstract void ProcessLighting(Group sceneToRender);

        // Matrices
        protected Matrix4x4 ViewToWorld;
        protected Matrix4x4 ScreenToView;
        protected Matrix4x4 WindowToScreen;
        internal Matrix4x4 ScreenToWorld;

        internal override void CalculateModelToWorldMatrix()
        {
            base.CalculateModelToWorldMatrix();
            ViewToWorld = ModelToWorld;
        }

        //ScreenToWorld = ModelToWorld * ScreenToView;

        // View Volume
        public override float ViewWidth
        {
            get => base.ViewWidth;
            set
            {
                base.ViewWidth = value;

                // Update view-to-screen matrix
                ScreenToView.m00 = ViewWidth / (2 * ZNear);

                NewRenderNeeded = true;
            }
        }
        public override float ViewHeight
        {
            get => base.ViewHeight;
            set
            {
                base.ViewHeight = value;

                // Update view-to-screen matrix
                ScreenToView.m11 = ViewHeight / (2 * ZNear);

                NewRenderNeeded = true;
            }
        }
        public override float ZNear
        {
            get => base.ZNear;
            set
            {
                base.ZNear = value;

                // Update view-to-screen matrix
                ScreenToView.m00 = ViewWidth / (2 * ZNear);
                ScreenToView.m11 = ViewHeight / (2 * ZNear);
                ScreenToView.m32 = (ZNear - ZFar) / (2 * ZNear * ZFar);
                ScreenToView.m33 = (ZNear + ZFar) / (2 * ZNear * ZFar);

                NewRenderNeeded = true;
            }
        }
        public override float ZFar
        {
            get => base.ZFar;
            set
            {
                base.ZFar = value;

                // Update view-to-screen matrix
                ScreenToView.m32 = (ZNear - ZFar) / (2 * ZNear * ZFar);
                ScreenToView.m33 = (ZNear + ZFar) / (2 * ZNear * ZFar);

                NewRenderNeeded = true;
            }
        }
        public override int RenderWidth
        {
            get => base.RenderWidth;
            set
            {
                base.RenderWidth = value;
                UpdateProperties();
            }
        }
        public override int RenderHeight
        {
            get => base.RenderHeight;
            set
            {
                base.RenderHeight = value;
                UpdateProperties();
            }
        }

        internal override void UpdateProperties()
        {
            base.UpdateProperties();
            colourBuffer = new(RenderWidth, RenderHeight);
            WindowToScreen = ScreenToWindow.Inverse();

            NewRenderNeeded = true;
        }

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
                NewRenderNeeded = true;
            }
        }

        /*public void MakeRenderSizeOfControl(Control control)
        {
            RenderWidth = control.Width;
            RenderHeight = control.Height;
        }*/

        private PixelFormat renderPixelFormat = PixelFormat.Format24bppRgb;
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

        // Update
        public UpdateMethod UpdateMethod { get; set; } = UpdateMethod.OnSceneObjectChange;

        #endregion

        #region Constructors

        internal Camera(Vector3D origin,
                        Vector3D directionForward,
                        Vector3D directionUp,
                        float viewWidth,
                        float viewHeight,
                        float zNear,
                        float zFar,
                        int renderWidth,
                        int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight)
        {
            string[] iconObjData = Properties.Resources.Camera.Split(Environment.NewLine);
            Icon = new Custom(origin, directionForward, directionUp, iconObjData) { Dimension = 3 };
            Icon.ColourAllSolidFaces(Color.DarkCyan);
            Icon.Scale(5);
        }

        #endregion

        #region Methods

        public async Task<Bitmap> Render(SceneObject sceneToRender, int renderWidth, int renderHeight, PixelFormat renderPixelFormat, bool includeChildren = true)
        {
            List<SceneObject> sceneObjectsToRender = new() { sceneToRender };
            if (includeChildren)
            {
                sceneObjectsToRender.AddRange(sceneToRender.GetAllChildren());
            }

            return await Render(sceneObjectsToRender, renderWidth, renderHeight, renderPixelFormat);
        }

        public async Task<Bitmap> Render(IEnumerable<SceneObject> sceneToRender,
                                         int renderWidth,
                                         int renderHeight,
                                         PixelFormat renderPixelFormat)
        {

        }

        //
        public async Task<Bitmap> Render(IEnumerable<SceneObject> sceneObjects)
        {

        }

        public async Task<Bitmap> Render(params SceneObject[] sceneObjects)
        {

        }

        public Bitmap Render() => Render(SceneToRender, RenderWidth, RenderHeight, RenderPixelFormat);
        public Bitmap Render(Group sceneToRender) => Render(sceneToRender, RenderWidth, RenderHeight, RenderPixelFormat);
        public Bitmap Render(int renderWidth, int renderHeight) => Render(SceneToRender, renderWidth, renderHeight, RenderPixelFormat);
        public Bitmap Render(Group sceneToRender, int renderWidth, int renderHeight, PixelFormat renderPixelFormat)
        {
            // Parameter checks
            if (sceneToRender is null)
            {
                throw GenerateException<ParameterCannotBeNullException>(nameof(sceneToRender));
            }


            if (sceneToRender is null) throw new ArgumentException(Exceptions.SceneToRenderCannotBeNull, nameof(sceneToRender));
            if (renderWidth < 0) throw new ArgumentException(Exceptions.RenderWidthLessThanZero, nameof(renderWidth));
            if (renderHeight < 0) throw new ArgumentException(Exceptions.RenderHeightLessThanZero, nameof(renderHeight));
            if (renderWidth == 0 || renderHeight == 0) return null;
            if (renderPixelFormat is PixelFormat.DontCare or PixelFormat.Extended or PixelFormat.Max or PixelFormat.Undefined)
            {
                throw new ArgumentException(Exceptions.InvalidPixelFormatForRendering, nameof(renderPixelFormat));
            }

            if (UpdateMethod == UpdateMethod.OnSceneObjectChange && !NewRenderNeeded) return LastRender;

            #if DEBUG

                ConsoleOutput.DisplayMessageFromObject(this, $"Rendering...");

            #endif

            /*

            // Calculate matrices and world origins
            foreach (Camera camera in sceneToRender.Cameras)
            {
                if (camera.Visible)
                {
                    if (camera.DrawIcon) camera.Icon.CalculateMatrices();
                    camera.CalculateMatrices();
                }
            }
            foreach (Light light in sceneToRender.Lights)
            {
                if (light.Visible)
                {
                    if (light.DrawIcon) light.Icon.CalculateMatrices();
                    light.CalculateMatrices();
                    light.CalculateWorldOrigin();
                }
            }
            foreach (Mesh mesh in sceneToRender.Meshes)
            {
                if (mesh.Visible)
                {
                    mesh.CalculateMatrices();
                }
            }

            this.CalculateMatrices();
            this.CalculateWorldOrigin();
            */

            // Generate a shadow map for each light (only if needed)
            foreach (Light light in sceneToRender.Lights)
            {
                if (light.ShadowMapNeedsUpdating && light.Visible)
                {
                    light.CalculateDepth(sceneToRender);

                    #if DEBUG

                    light.ExportShadowMap();

                    #endif
                }
            }

            // Generate z buffer
            CalculateDepth(sceneToRender);

            // Process lighting
            ProcessLighting(sceneToRender);

            NewRenderNeeded = false;

            return LastRender = CreateBitmap(renderWidth, renderHeight, colourBuffer, renderPixelFormat);
        }

        // Lighting
        protected void ApplyLighting(
            Vector4D worldSpacePoint,
            ref Color pointColour, int x, int y,
            Group sceneToRender)
        {
            bool lightApplied = false;

            foreach (Light light in sceneToRender.Lights)
            {
                if (light.Visible)
                {
                    // Move the point from world space to light-view space
                    Vector4D lightViewSpacePoint = light.WorldToView * worldSpacePoint;

                    Color newLightColour = light.Colour;
                    if (light is PointLight or Spotlight)
                    {
                        // Darken the light's colour based on how far away the point is from the light
                        Vector3D lightToPoint = (Vector3D)lightViewSpacePoint;
                        float distantIntensity = light.Strength / lightToPoint.SquaredMagnitude();
                        newLightColour = light.Colour.DarkenPercentage(distantIntensity);
                    }

                    // Move the point from light-view space to light-screen space
                    Vector4D lightScreenSpacePoint = light.ViewToScreen * lightViewSpacePoint;
                    if (light is PointLight or Spotlight)
                    {
                        lightScreenSpacePoint /= lightScreenSpacePoint.w;
                    }

                    Vector4D lightWindowSpacePoint = light.ScreenToWindow * lightScreenSpacePoint;

                    // Round the points
                    int lightPointX = lightWindowSpacePoint.x.RoundToInt();
                    int lightPointY = lightWindowSpacePoint.y.RoundToInt();
                    float lightPointZ = lightWindowSpacePoint.z;

                    //Trace.WriteLine("The following light point has been calculated: "+new Vector3D(light_point_x,light_point_y,light_point_z));

                    if (lightPointX >= 0 && lightPointX < light.RenderWidth &&
                        lightPointY >= 0 && lightPointY < light.RenderHeight)
                    {
                        if (lightPointZ.ApproxLessThanEquals(light.zBuffer.Values[lightPointX][lightPointY], 1E-4F))
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

        internal override void ResetBuffers()
        {
            zBuffer.SetAllToValue(outOfBoundsValue);
            colourBuffer.SetAllToValue(RenderBackgroundColour);
        }

        internal override void AddPointToBuffers<T>(T colour, int x, int y, float z)
        {
            #if DEBUG

            if (x >= 0 && y >= 0 && x < RenderWidth && y < RenderHeight)
            {
                if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                    colourBuffer.Values[x][y] = (Color)(object)colour;
                }
            }
            else
            {
                throw new InvalidOperationException($"Attempted to add a point outside buffer range at ({x}, {y}, {z}).");
            }

            #else

            if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
            {
                zBuffer.Values[x][y] = z;
                colourBuffer.Values[x][y] = (Color)(object)colour;
            }

            #endif
        }

        internal void TextureAddPointToBuffers(Bitmap texture, int x, int y, float z, int tx, int ty)
        {
            #if DEBUG

            if (x >= 0 && y >= 0 && x < RenderWidth && y < RenderHeight)
            {
                if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                    colourBuffer.Values[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
                }
            }
            else
            {
                throw new InvalidOperationException($"Attempted to add a point outside buffer range at ({x}, {y}, {z}).");
            }

            #else

            if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
            {
                zBuffer.Values[x][y] = z;
                colourBuffer.Values[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
            }

            #endif
        }

        #endregion
    }
}