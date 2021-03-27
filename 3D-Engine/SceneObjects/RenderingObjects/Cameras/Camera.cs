﻿/*
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

        // Buffers
        protected Buffer2D<Color> colourBuffer;
        protected Buffer2D<float> zBuffer;

        // Matrices
        protected Matrix4x4 ViewToWorld;
        protected Matrix4x4 ScreenToView;
        protected Matrix4x4 WindowToScreen;

        internal Matrix4x4 ScreenToWorld;

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            ViewToWorld = ModelToWorld;
        }

        //ScreenToWorld = ModelToWorld * ScreenToView;
        internal abstract void ProcessLighting(Group sceneToRender);

        // View Volume
        public override float ViewWidth
        {
            get => base.ViewWidth;
            set
            {
                base.ViewWidth = value;
                ScreenToView = ViewToScreen.Inverse();
                NewRenderNeeded = true;
            }
        }
        public override float ViewHeight
        {
            get => base.ViewHeight;
            set
            {
                base.ViewHeight = value;
                ScreenToView = ViewToScreen.Inverse();
                NewRenderNeeded = true;
            }
        }
        public override float ZNear
        {
            get => base.ZNear;
            set
            {
                base.ZNear = value;
                ScreenToView = ViewToScreen.Inverse();
                NewRenderNeeded = true;
            }
        }
        public override float ZFar
        {
            get => base.ZFar;
            set
            {
                base.ZFar = value;
                ScreenToView = ViewToScreen.Inverse();
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

        private void UpdateProperties()
        {
            colourBuffer = new(RenderWidth, RenderHeight);
            zBuffer = new(RenderWidth, RenderHeight);

            ScreenToWindow = Transform.Scale(0.5f * (RenderWidth - 1), 0.5f * (RenderHeight - 1), 1) * windowTranslate;
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

        internal Camera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight)
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

        public Bitmap Render() => Render(SceneToRender, RenderWidth, RenderHeight, RenderPixelFormat);
        public Bitmap Render(Group sceneToRender) => Render(sceneToRender, RenderWidth, RenderHeight, RenderPixelFormat);
        public Bitmap Render(int renderWidth, int renderHeight) => Render(SceneToRender, renderWidth, renderHeight, RenderPixelFormat);
        public Bitmap Render(Group sceneToRender, int renderWidth, int renderHeight, PixelFormat renderPixelFormat)
        {
            // Parameter checks
            if (sceneToRender is null) throw new ArgumentException(Exceptions.SceneToRenderCannotBeNull, nameof(sceneToRender));
            if (renderWidth < 0) throw new ArgumentException(Exceptions.RenderWidthLessThanZero, nameof(renderWidth));
            if (renderHeight < 0) throw new ArgumentException(Exceptions.RenderHeightLessThanZero, nameof(renderHeight));
            if (renderWidth == 0 || renderHeight == 0) return null;
            if (renderPixelFormat is PixelFormat.DontCare or PixelFormat.Extended or PixelFormat.Max or PixelFormat.Undefined)
            {
                throw new ArgumentException(Exceptions.InvalidPixelFormatForRendering, nameof(renderPixelFormat));
            }

            if (!NewRenderNeeded) return LastRender;

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

            // Generate a shadow map for each light (only if needed)
            foreach (Light light in sceneToRender.Lights)
            {
                if (light.ShadowMapNeedsUpdating && light.Visible)
                {
                    light.GenerateShadowMap(sceneToRender);

                    #if DEBUG

                    light.ExportShadowMap();

                    #endif
                }
            }

            // Generate z buffer
            GenerateZBuffer(sceneToRender);

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
                    int lightPointX = lightWindowSpacePoint.x.RoundToInt();
                    int lightPointY = lightWindowSpacePoint.y.RoundToInt();
                    float lightPointZ = lightWindowSpacePoint.z;

                    //Trace.WriteLine("The following light point has been calculated: "+new Vector3D(light_point_x,light_point_y,light_point_z));

                    if (lightPointX >= 0 && lightPointX < light.RenderWidth &&
                        lightPointY >= 0 && lightPointY < light.RenderHeight)
                    {
                        if (lightPointZ.ApproxLessThanEquals(light.ShadowMap.Values[lightPointX][lightPointY], 1E-4F))
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