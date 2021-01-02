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

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Lights;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Transformations;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace _3D_Engine.SceneObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Camera"/>.
    /// </summary>
    public abstract partial class Camera : SceneObject
    {
        #region Fields and Properties

        // Appearance
        public Mesh Icon { get; set; }

        /// <summary>
        /// Determines if the <see cref="Camera"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool DrawIcon { get; set; } = false;

        // View Volume
        private VolumeOutline volumeStyle = VolumeOutline.None;

        /// <summary>
        /// Determines how the <see cref="Camera">Camera's</see> view volume outline is drawn.
        /// </summary>
        public VolumeOutline VolumeStyle
        {
            get => volumeStyle;
            set
            {
                volumeStyle = value;

                VolumeEdges.Clear();

                float semiWidth = Width / 2, semiHeight = Height / 2;

                Vertex zeroPoint = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertex nearTopLeftPoint = new Vertex(new Vector4D(-semiWidth, semiHeight, ZNear, 1));
                Vertex nearTopRightPoint = new Vertex(new Vector4D(semiWidth, semiHeight, ZNear, 1));
                Vertex nearBottomLeftPoint = new Vertex(new Vector4D(-semiWidth, -semiHeight, ZNear, 1));
                Vertex nearBottomRightPoint = new Vertex(new Vector4D(semiWidth, -semiHeight, ZNear, 1));

                if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
                {
                    VolumeEdges.AddRange(new[]
                    {
                        new Edge(zeroPoint, nearTopLeftPoint), // Near top left
                        new Edge(zeroPoint, nearTopRightPoint), // Near top right
                        new Edge(zeroPoint, nearBottomLeftPoint), // Near bottom left
                        new Edge(zeroPoint, nearBottomRightPoint), // Near bottom right
                        new Edge(nearTopLeftPoint, nearTopRightPoint), // Near top
                        new Edge(nearBottomLeftPoint, nearBottomRightPoint), // Near bottom
                        new Edge(nearTopLeftPoint, nearBottomLeftPoint), // Near left
                        new Edge(nearTopRightPoint, nearBottomRightPoint) // Near right
                    });
                }

                if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
                {
                    float ratio = (this is OrthogonalCamera) ? 1 : ZFar / ZNear;
                    float semiWidthRatio = semiWidth * ratio, semiHeightRatio = semiHeight * ratio;

                    Vertex farTopLeftPoint = new Vertex(new Vector4D(-semiWidthRatio, semiHeightRatio, ZFar, 1));
                    Vertex farTopRightPoint = new Vertex(new Vector4D(semiWidthRatio, semiHeightRatio, ZFar, 1));
                    Vertex farBottomLeftPoint = new Vertex(new Vector4D(-semiWidthRatio, -semiHeightRatio, ZFar, 1));
                    Vertex farBottomRightPoint = new Vertex(new Vector4D(semiWidthRatio, -semiHeightRatio, ZFar, 1));

                    VolumeEdges.AddRange(new[]
                    {
                        new Edge(nearTopLeftPoint, farTopLeftPoint), // Far top left
                        new Edge(nearTopRightPoint, farTopRightPoint), // Far top right
                        new Edge(nearBottomLeftPoint, farBottomLeftPoint), // Far bottom left
                        new Edge(nearBottomRightPoint, farBottomRightPoint), // Far bottom right
                        new Edge(farTopLeftPoint, farTopRightPoint), // Far top
                        new Edge(farBottomLeftPoint, farBottomRightPoint), // Far bottom
                        new Edge(farTopLeftPoint, farBottomLeftPoint), // Far left
                        new Edge(farTopRightPoint, farBottomRightPoint) // Far right
                    });
                }
            }
        }

        internal List<Edge> VolumeEdges = new();

        // Matrices
        private static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0));
        internal Matrix4x4 WorldToCameraView, CameraViewToCameraScreen, CameraScreenToWorld;

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            WorldToCameraView = ModelToWorld.Inverse();
            CameraScreenToWorld = ModelToWorld * CameraViewToCameraScreen.Inverse();
        }

        // Clipping planes
        internal ClippingPlane[] CameraViewClippingPlanes;
        internal static readonly ClippingPlane[] CameraScreenClippingPlanes = new ClippingPlane[]
        {
            new(-Vector3D.One, Vector3D.UnitX), // Left
            new(-Vector3D.One, Vector3D.UnitY), // Bottom
            new(-Vector3D.One, Vector3D.UnitZ), // Near
            new(Vector3D.One, Vector3D.UnitNegativeX), // Right
            new(Vector3D.One, Vector3D.UnitNegativeY), // Top
            new(Vector3D.One, Vector3D.UnitNegativeZ) // Far
        };

        // View volume
        /// <summary>
        /// The width of the <see cref="Camera">Camera's</see> view/near plane.
        /// </summary>
        public abstract float Width { get; set; }
        /// <summary>
        /// The height of the <see cref="Camera">Camera's</see> view/near plane.
        /// </summary>
        public abstract float Height { get; set; }
        /// <summary>
        /// The depth of the <see cref="Camera">Camera's</see> view to the near plane.
        /// </summary>
        public abstract float ZNear { get; set; }
        /// <summary>
        /// The depth of the <see cref="Camera">Camera's</see> view to the far plane.
        /// </summary>
        public abstract float ZFar { get; set; }

        // Miscellaneous
        private const byte outOfBoundsValue = 2;

        // Render
        public Color RenderBackgroundColour { get; set; }

        // Buffers
        private Buffer2D<Color> colourBuffer;
        private Buffer2D<float> zBuffer;

        // Matrices
        private Matrix4x4 cameraScreenToWindow, cameraScreenToWindowInverse;

        // Render
        private int renderWidth, renderHeight;
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

            cameraScreenToWindow = Transform.Scale(0.5f * (renderWidth - 1), 0.5f * (renderHeight - 1), 1) * windowTranslate;
            cameraScreenToWindowInverse = cameraScreenToWindow.Inverse();
        }
        
        public void MakeRenderSizeOfControl(Control control)
        {
            RenderWidth = control.Width;
            RenderHeight = control.Height;
        }

        // Scene
        internal Scene ParentScene { get; set; }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, bool hasDirectionArrows = true) : base(origin, directionForward, directionUp, hasDirectionArrows)
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

        public Bitmap Render(int width, int height, PixelFormat pixelFormat = PixelFormat.Format24bppRgb)
        {
            RenderWidth = width;
            RenderHeight = height;

            return Render(pixelFormat);
        }

        public Bitmap Render(PixelFormat pixelFormat = PixelFormat.Format24bppRgb)
        {    
            // Reset scene buffers
            zBuffer.SetAllToValue(outOfBoundsValue);
            colourBuffer.SetAllToValue(RenderBackgroundColour);
            foreach (Light light in ParentScene.Lights)
            {
                light.ShadowMap.SetAllToValue(outOfBoundsValue);
            }

            // Calculate matrices and world origins
            foreach (SceneObject sceneObject in ParentScene.SceneObjects)
            {
                switch (sceneObject)
                {
                    case Camera camera when camera.Visible:
                        if (camera.DrawIcon)
                        {
                            camera.Icon.CalculateMatrices();
                        }
                        camera.CalculateMatrices();
                        break;
                    case Light light when light.Visible:
                        if (light.DrawIcon)
                        {
                            light.Icon.CalculateMatrices();
                        }
                        light.CalculateMatrices();
                        light.CalculateWorldOrigin();
                        break;
                    case Mesh mesh when mesh.Visible:
                        mesh.CalculateMatrices();
                        break;
                }
            }
            this.CalculateWorldOrigin(); //?

            // Generate a shadow map for each light (only if needed)
            if (ParentScene.ShadowMapsNeedUpdating)
            {
                foreach (Light light in ParentScene.Lights)
                {
                    if (light.Visible)
                    {
                        GenerateShadowMap(light);

                        #if DEBUG

                        light.ExportShadowMap();
                        
                        #endif
                    }
                }
            }

            // Generate z buffer
            foreach (SceneObject sceneObject in ParentScene.SceneObjects)
            {
                switch (sceneObject)
                {
                    case Camera camera when camera.DrawIcon:
                        Matrix4x4 modelToCameraView = this.WorldToCameraView * camera.Icon.ModelToWorld;

                        foreach (Face face in camera.Icon.Faces)
                        {
                            AddFaceToZBuffer
                            (
                                face,
                                3,
                                ref modelToCameraView,
                                ref this.CameraViewToCameraScreen,
                                ref cameraScreenToWindow
                            );
                        }
                        break;
                    case Light light when light.DrawIcon:
                        modelToCameraView = this.WorldToCameraView * light.Icon.ModelToWorld;

                        foreach (Face face in light.Icon.Faces)
                        {
                            AddFaceToZBuffer
                            (
                                face,
                                3,
                                ref modelToCameraView,
                                ref this.CameraViewToCameraScreen,
                                ref cameraScreenToWindow
                            );
                        }
                        break;
                    case Mesh mesh when mesh.Visible && mesh.DrawFaces:
                        modelToCameraView = this.WorldToCameraView * mesh.ModelToWorld;

                        foreach (Face face in mesh.Faces)
                        {
                            if (face.Visible)
                            {
                                AddFaceToZBuffer
                                (
                                    face,
                                    mesh.Dimension,
                                    ref modelToCameraView,
                                    ref this.CameraViewToCameraScreen,
                                    ref cameraScreenToWindow
                                );
                            }
                        }
                        break;
                }

                if (sceneObject.HasDirectionArrows && sceneObject.DisplayDirectionArrows)
                {
                    Arrow directionForward = sceneObject.DirectionArrows.SceneObjects[0] as Arrow;
                    Arrow directionUp = sceneObject.DirectionArrows.SceneObjects[1] as Arrow;
                    Arrow directionRight = sceneObject.DirectionArrows.SceneObjects[2] as Arrow;

                    Matrix4x4 directionForwardModelToCameraView = this.WorldToCameraView * directionForward.ModelToWorld;
                    Matrix4x4 directionUpModelToCameraView = this.WorldToCameraView * directionUp.ModelToWorld;
                    Matrix4x4 directionRightModelToCameraView = this.WorldToCameraView * directionRight.ModelToWorld;

                    foreach (Face face in directionForward.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionForwardModelToCameraView,
                            ref this.CameraViewToCameraScreen,
                            ref cameraScreenToWindow
                        );
                    }
                    foreach (Face face in directionUp.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionUpModelToCameraView,
                            ref this.CameraViewToCameraScreen,
                            ref cameraScreenToWindow
                        );
                    }
                    foreach (Face face in directionRight.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionRightModelToCameraView,
                            ref this.CameraViewToCameraScreen,
                            ref cameraScreenToWindow
                        );
                    }
                }
            }

            // Apply lighting
            switch (this)
            {
                case OrthogonalCamera orthogonalCamera:
                    Matrix4x4 windowToWorld = this.ModelToWorld * this.CameraViewToCameraScreen.Inverse() * cameraScreenToWindowInverse;

                    for (int x = 0; x < renderWidth; x++)
                    {
                        for (int y = 0; y < renderHeight; y++)
                        {
                            if (zBuffer.Values[x][y] != outOfBoundsValue)
                            {
                                // Move the point from window space to world space and apply lighting
                                ApplyLighting(windowToWorld * new Vector4D(x, y, zBuffer.Values[x][y], 1), ref colourBuffer.Values[x][y], x, y);
                            }
                        }
                    }
                    break;
                case PerspectiveCamera perspectiveCamera:
                    for (int x = 0; x < renderWidth; x++)
                    {
                        for (int y = 0; y < renderHeight; y++)
                        {
                            // check all floats and ints
                            if (zBuffer.Values[x][y] != outOfBoundsValue)
                            {
                                SMC_Camera_Perspective
                                (
                                    x, y, zBuffer.Values[x][y],
                                    ref colourBuffer.Values[x][y],
                                    ref cameraScreenToWindowInverse,
                                    ref this.CameraScreenToWorld
                                );
                            }
                        }
                    }
                    break;
            }

            return CreateBitmap(RenderWidth, RenderHeight, colourBuffer);
        }

        // how works?
        public static unsafe Bitmap CreateBitmap(int width, int height, Buffer2D<Color> colourBuffer)
        {
            Bitmap newFrame = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData data = newFrame.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb); //????????????

            for (int y = 0; y < height; y++)
            {
                byte* row_start = (byte*)data.Scan0 + y * data.Stride;
                for (int x = 0; x < width; x++)
                {
                    row_start[x * 3] = colourBuffer.Values[x][y * -1 + height - 1].B; // Blue
                    row_start[x * 3 + 1] = colourBuffer.Values[x][y * -1 + height - 1].G; // Green
                    row_start[x * 3 + 2] = colourBuffer.Values[x][y * -1 + height - 1].R; // Red
                }
            }

            newFrame.UnlockBits(data);
            return newFrame;
        }

        #endregion
    }
}