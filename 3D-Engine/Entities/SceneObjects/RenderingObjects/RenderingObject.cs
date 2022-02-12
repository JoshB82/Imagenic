/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a rendering object.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Renderers;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="RenderingObject"/>. Any object which inherits from this class provides rendering functionality.
    /// </summary>
    /// <remarks>This class inherits from the <see cref="SceneObject"/> class.</remarks>
    public abstract partial class RenderingObject : SceneObject
    {
        #region Fields and Properties

        // Appearance
        private Mesh icon;
        public Mesh Icon
        {
            get => icon;
            set
            {
                icon = value;

            }
        }

        // Buffers
        internal Buffer2D<float> zBuffer;
        internal virtual void UpdateProperties()
        {
            zBuffer = new(renderWidth, renderHeight);
            ScreenToWindow = Transform.Scale(0.5f * (renderWidth - 1), 0.5f * (renderHeight - 1), 1) * windowTranslate;
        }

        // Clipping Planes
        internal ClippingPlane[] ViewClippingPlanes { get; set; }
        internal static readonly ClippingPlane[] ScreenClippingPlanes = new ClippingPlane[]
        {
            new(-Vector3D.One, Vector3D.UnitX), // Left
            new(-Vector3D.One, Vector3D.UnitY), // Bottom
            new(-Vector3D.One, Vector3D.UnitZ), // Near
            new(Vector3D.One, Vector3D.UnitNegativeX), // Right
            new(Vector3D.One, Vector3D.UnitNegativeY), // Top
            new(Vector3D.One, Vector3D.UnitNegativeZ) // Far
        };

        // Matrices
        public Matrix4x4 WorldToView { get; private set; }
        internal override void CalculateModelToWorldMatrix()
        {
            base.CalculateModelToWorldMatrix();
            WorldToView = ModelToWorld.Inverse();
        }

        private Matrix4x4 viewToScreen;
        public Matrix4x4 ViewToScreen
        {
            get => viewToScreen;
            private set
            {
                viewToScreen = value;
            }
        }

        public Matrix4x4 ScreenToWindow { get; private set; }
        protected static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0)); //?

        // View Volume
        private float viewWidth, viewHeight, zNear, zFar;
        private int renderWidth, renderHeight;

        /// <summary>
        /// The width of the <see cref="RenderingObject">RenderingObject's</see> view/near plane.
        /// </summary>
        public virtual float ViewWidth
        {
            get => viewWidth;
            set
            {
                if (value == viewWidth) return;
                viewWidth = value;
                RequestNewRenders();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        UpdateMatrixOrthogonal1();
                        UpdateClippingPlaneOrthogonal1();
                        break;
                    case PerspectiveCamera or Spotlight:
                        UpdateMatrixPerspective1();
                        UpdateClippingPlanePerspective1();
                        break;
                    default:
                        throw Exceptions.RenderingObjectTypeNotSupported;
                }
            }
        }
        /// <summary>
        /// The height of the <see cref="RenderingObject">RenderingObject's</see> view/near plane.
        /// </summary>
        public virtual float ViewHeight
        {
            get => viewHeight;
            set
            {
                if (value == viewHeight) return;
                viewHeight = value;
                RequestNewRenders();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        UpdateMatrixOrthogonal2();
                        UpdateClippingPlaneOrthogonal2();
                        break;
                    case PerspectiveCamera or Spotlight:
                        UpdateMatrixPerspective2();
                        UpdateClippingPlanePerspective2();
                        break;
                    default:
                        throw Exceptions.RenderingObjectTypeNotSupported;
                }
            }
        }
        /// <summary>
        /// The depth of the <see cref="RenderingObject">RenderingObject's</see> view to the near plane.
        /// </summary>
        public virtual float ZNear
        {
            get => zNear;
            set
            {
                if (value == zNear) return;
                zNear = value;
                RequestNewRenders();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        UpdateMatrixOrthogonal3();
                        UpdateClippingPlaneOrthogonal3();
                        break;
                    case PerspectiveCamera or Spotlight:
                        UpdateMatrixPerspective3();
                        UpdateClippingPlanePerspective3();
                        break;
                    default:
                        throw Exceptions.RenderingObjectTypeNotSupported;
                }
            }
        }
        /// <summary>
        /// The depth of the <see cref="RenderingObject">RenderingObject's</see> view to the far plane.
        /// </summary>
        public virtual float ZFar
        {
            get => zFar;
            set
            {
                if (value == zFar) return;
                zFar = value;
                RequestNewRenders();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        UpdateMatrixOrthogonal3();

                        // Update far clipping plane
                        ViewClippingPlanes[5].Point.z = zFar;

                        break;
                    case PerspectiveCamera or Spotlight:
                        // Update view-to-screen matrix
                        viewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
                        viewToScreen.m23 = -(2 * zFar * zNear) / (zFar - zNear);

                        // Update far clipping plane
                        ViewClippingPlanes[5].Point.z = zFar;

                        break;
                    default:
                        throw Exceptions.RenderingObjectTypeNotSupported;
                }
            }
        }
        public virtual int RenderWidth
        {
            get => renderWidth;
            set
            {
                if (value == renderWidth) return;
                renderWidth = value;
                UpdateProperties();
                RequestNewRenders();
            }
        }
        public virtual int RenderHeight
        {
            get => renderHeight;
            set
            {
                if (value == renderHeight) return;
                renderHeight = value;
                UpdateProperties();
                RequestNewRenders();
            }
        }

        internal List<Edge> VolumeEdges = new();

        private VolumeOutline volumeStyle = VolumeOutline.None;

        /// <summary>
        /// Determines how the <see cref="RenderingObject">RenderingObject's</see> view volume outline is drawn.
        /// </summary>
        public VolumeOutline VolumeStyle
        {
            get => volumeStyle;
            set
            {
                if (value == volumeStyle) return;
                volumeStyle = value;
                RequestNewRenders();

                VolumeEdges.Clear();

                float semiViewWidth = viewWidth / 2, semiViewHeight = viewHeight / 2;

                Vertex zeroPoint = new(new Vector4D(0, 0, 0, 1));
                Vertex nearTopLeftPoint = new(new Vector4D(-semiViewWidth, semiViewHeight, ZNear, 1));
                Vertex nearTopRightPoint = new(new Vector4D(semiViewWidth, semiViewHeight, ZNear, 1));
                Vertex nearBottomLeftPoint = new(new Vector4D(-semiViewWidth, -semiViewHeight, ZNear, 1));
                Vertex nearBottomRightPoint = new(new Vector4D(semiViewWidth, -semiViewHeight, ZNear, 1));

                if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
                {
                    VolumeEdges.AddRange(new Edge[]
                    {
                        new DashedEdge(zeroPoint, nearTopLeftPoint), // Near top left
                        new DashedEdge(zeroPoint, nearTopRightPoint), // Near top right
                        new DashedEdge(zeroPoint, nearBottomLeftPoint), // Near bottom left
                        new DashedEdge(zeroPoint, nearBottomRightPoint), // Near bottom right
                        new DashedEdge(nearTopLeftPoint, nearTopRightPoint), // Near top
                        new DashedEdge(nearBottomLeftPoint, nearBottomRightPoint), // Near bottom
                        new DashedEdge(nearTopLeftPoint, nearBottomLeftPoint), // Near left
                        new DashedEdge(nearTopRightPoint, nearBottomRightPoint) // Near right
                    });
                }

                if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
                {
                    float ratio = (this is OrthogonalCamera or DistantLight) ? 1 : ZFar / ZNear;
                    float semiViewWidthRatio = semiViewWidth * ratio, semiViewHeightRatio = semiViewHeight * ratio;

                    Vertex farTopLeftPoint = new(new Vector4D(-semiViewWidthRatio, semiViewHeightRatio, ZFar, 1));
                    Vertex farTopRightPoint = new(new Vector4D(semiViewWidthRatio, semiViewHeightRatio, ZFar, 1));
                    Vertex farBottomLeftPoint = new(new Vector4D(-semiViewWidthRatio, -semiViewHeightRatio, ZFar, 1));
                    Vertex farBottomRightPoint = new(new Vector4D(semiViewWidthRatio, -semiViewHeightRatio, ZFar, 1));

                    VolumeEdges.AddRange(new Edge[]
                    {
                        new DashedEdge(nearTopLeftPoint, farTopLeftPoint), // Far top left
                        new DashedEdge(nearTopRightPoint, farTopRightPoint), // Far top right
                        new DashedEdge(nearBottomLeftPoint, farBottomLeftPoint), // Far bottom left
                        new DashedEdge(nearBottomRightPoint, farBottomRightPoint), // Far bottom right
                        new DashedEdge(farTopLeftPoint, farTopRightPoint), // Far top
                        new DashedEdge(farBottomLeftPoint, farBottomRightPoint), // Far bottom
                        new DashedEdge(farTopLeftPoint, farBottomLeftPoint), // Far left
                        new DashedEdge(farTopRightPoint, farBottomRightPoint) // Far right
                    });
                }
            }
        }

        #endregion

        #region Constructors

        internal RenderingObject(Vector3D worldOrigin,
                                 Orientation worldOrientation,
                                 float viewWidth,
                                 float viewHeight,
                                 float zNear,
                                 float zFar,
                                 int renderWidth,
                                 int renderHeight) : base(worldOrigin, worldOrientation)
        {
            // Construct view-space clipping planes and matrix
            float semiViewWidth = viewWidth / 2, semiViewHeight = viewHeight / 2;
            Vector3D nearBottomLeftPoint = new(-semiViewWidth, -semiViewHeight, zNear);

            switch (this)
            {
                case OrthogonalCamera or DistantLight:
                    viewToScreen = Matrix4x4.Identity;
                    viewToScreen.m00 = 2 / viewWidth;
                    viewToScreen.m11 = 2 / viewHeight;
                    viewToScreen.m22 = 2 / (zFar - zNear);
                    viewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                    Vector3D farTopRightPoint = new(semiViewWidth, semiViewHeight, zFar);

                    ViewClippingPlanes = new ClippingPlane[]
                    {
                        new(nearBottomLeftPoint, Vector3D.UnitX), // Left
                        new(nearBottomLeftPoint, Vector3D.UnitY), // Bottom
                        new(nearBottomLeftPoint, Vector3D.UnitZ), // Near
                        new(farTopRightPoint, Vector3D.UnitNegativeX), // Right
                        new(farTopRightPoint, Vector3D.UnitNegativeY), // Top
                        new(farTopRightPoint, Vector3D.UnitNegativeZ) // Far
                    };

                    break;
                case PerspectiveCamera or Spotlight:
                    viewToScreen = Matrix4x4.Zero;
                    viewToScreen.m00 = 2 * zNear / viewWidth;
                    viewToScreen.m11 = 2 * zNear / viewHeight;
                    viewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
                    viewToScreen.m23 = -2 * zNear * zFar / (zFar - zNear);
                    viewToScreen.m32 = 1;

                    farTopRightPoint = new(viewWidth * zFar / (2 * zNear), viewHeight * zFar / (2 * zNear), zFar);
                    Vector3D nearTopLeftPoint = new(-semiViewWidth, semiViewHeight, zNear);
                    Vector3D nearTopRightPoint = new(semiViewWidth, semiViewHeight, zNear);
                    Vector3D farBottomLeftPoint = new(-semiViewWidth * zFar / zNear, -semiViewHeight * zFar / zNear, zFar);
                    Vector3D farBottomRightPoint = new(semiViewWidth * zFar / zNear, -semiViewHeight * zFar / zNear, zFar);

                    ViewClippingPlanes = new ClippingPlane[]
                    {
                        new(nearBottomLeftPoint, Vector3D.NormalFromPlane(farBottomLeftPoint, nearTopLeftPoint, nearBottomLeftPoint)), // Left
                        new(nearBottomLeftPoint, Vector3D.NormalFromPlane(nearBottomLeftPoint, farBottomRightPoint, farBottomLeftPoint)), // Bottom
                        new(nearBottomLeftPoint, Vector3D.UnitZ), // Near
                        new(farTopRightPoint, Vector3D.NormalFromPlane(nearTopRightPoint, farTopRightPoint, farBottomRightPoint)), // Right
                        new(farTopRightPoint, Vector3D.NormalFromPlane(nearTopLeftPoint, farTopRightPoint, nearTopRightPoint)), // Top
                        new(farTopRightPoint, Vector3D.UnitNegativeZ) // Far
                    };

                    break;
                default:
                    throw Exceptions.RenderingObjectTypeNotSupported;
            }

            this.viewWidth = viewWidth;
            this.viewHeight = viewHeight;
            this.zNear = zNear;
            this.zFar = zFar;
            this.renderWidth = renderWidth;
            this.renderHeight = renderHeight;
            UpdateProperties();
        }

        #endregion

        #region Methods

        // Update matrix
        private void UpdateMatrixOrthogonal1()
        {
            // Update view-to-screen matrix
            viewToScreen.m00 = 2 / viewWidth;
        }

        private void UpdateMatrixPerspective1()
        {
            // Update view-to-screen matrix
            viewToScreen.m00 = 2 * zNear / viewWidth;
        }

        private void UpdateMatrixOrthogonal2()
        {
            // Update view-to-screen matrix
            viewToScreen.m11 = 2 / viewHeight;
        }

        private void UpdateMatrixPerspective2()
        {
            // Update view-to-screen matrix
            viewToScreen.m11 = 2 * zNear / viewHeight;
        }

        private void UpdateMatrixOrthogonal3()
        {
            // Update view-to-screen matrix
            viewToScreen.m22 = 2 / (zFar - zNear);
            viewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);
        }

        private void UpdateMatrixPerspective3()
        {
            // Update view-to-screen matrix
            viewToScreen.m00 = 2 * zNear / viewWidth;
            viewToScreen.m11 = 2 * zNear / viewHeight;
            viewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
            viewToScreen.m23 = -(2 * zFar * zNear) / (zFar - zNear);
        }

        // Update clipping planes
        private void UpdateClippingPlaneOrthogonal1()
        {
            // Update left and right clipping planes
            ViewClippingPlanes[0].Point.x = -viewWidth / 2;
            ViewClippingPlanes[3].Point.x = viewWidth / 2;
        }

        private void UpdateClippingPlanePerspective1()
        {
            // Update left and right clipping planes
            float semiWidth = viewWidth / 2, semiHeight = viewHeight / 2;
            ViewClippingPlanes[0].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, -semiHeight, zNear), new Vector3D(-semiWidth, semiHeight, zNear));
            ViewClippingPlanes[3].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, semiHeight, zNear), new Vector3D(semiWidth, -semiHeight, zNear));
        }

        private void UpdateClippingPlaneOrthogonal2()
        {
            // Update top and bottom clipping planes
            ViewClippingPlanes[1].Point.y = -viewHeight / 2;
            ViewClippingPlanes[4].Point.y = viewHeight / 2;
        }

        private void UpdateClippingPlanePerspective2()
        {
            // Update top and bottom clipping planes
            float semiWidth = viewWidth / 2, semiHeight = viewHeight / 2;
            ViewClippingPlanes[4].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, semiHeight, zNear), new Vector3D(semiWidth, semiHeight, zNear));
            ViewClippingPlanes[1].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, -semiHeight, zNear), new Vector3D(-semiWidth, -semiHeight, zNear));
        }

        private void UpdateClippingPlaneOrthogonal3()
        {
            // Update near clipping plane
            ViewClippingPlanes[2].Point.z = zNear;
        }

        private void UpdateClippingPlanePerspective3()
        {
            // Update near clipping plane
            ViewClippingPlanes[2].Point.z = zNear;
        }

        #endregion
    }
}