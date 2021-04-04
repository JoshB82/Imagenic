/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a rendering object.
 */

using _3D_Engine.Constants;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Transformations;
using System.Collections.Generic;

namespace _3D_Engine.SceneObjects.RenderingObjects
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="RenderingObject"/>. Any object which inherits from this class provides rendering functionality.
    /// </summary>
    public abstract class RenderingObject : SceneObject
    {
        #region Fields and Properties

        // Appearance
        // COME BACK TO
        public Mesh Icon { get; set; }

        private bool drawIcon = false;
        /// <summary>
        /// Determines if the <see cref="RenderingObject"/> is drawn in the <see cref="SceneToRender"/>.
        /// </summary>
        public bool DrawIcon
        {
            get => drawIcon;
            set
            {
                drawIcon = value;
                UpdateRenderCamera();
            }
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
        internal Matrix4x4 WorldToView;
        internal Matrix4x4 ViewToScreen;
        internal Matrix4x4 ScreenToWindow;
        protected static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0)); //?

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();
            WorldToView = ModelToWorld.Inverse();
        }

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
                viewWidth = value;
                UpdateRenderCamera();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        // Update view-to-screen matrix
                        ViewToScreen.m00 = 2 / viewWidth;

                        // Update left and right clipping planes
                        ViewClippingPlanes[0].Point.x = -viewWidth / 2;
                        ViewClippingPlanes[3].Point.x = viewWidth / 2;

                        break;
                    case PerspectiveCamera or Spotlight:
                        // Update view-to-screen matrix
                        ViewToScreen.m00 = 2 * zNear / viewWidth;

                        // Update left and right clipping planes
                        float semiWidth = viewWidth / 2, semiHeight = viewHeight / 2;
                        ViewClippingPlanes[0].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, -semiHeight, zNear), new Vector3D(-semiWidth, semiHeight, zNear));
                        ViewClippingPlanes[3].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, semiHeight, zNear), new Vector3D(semiWidth, -semiHeight, zNear));

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
                viewHeight = value;
                UpdateRenderCamera();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        // Update view-to-screen matrix
                        ViewToScreen.m11 = 2 / viewHeight;

                        // Update top and bottom clipping planes
                        ViewClippingPlanes[1].Point.y = -viewHeight / 2;
                        ViewClippingPlanes[4].Point.y = viewHeight / 2;

                        break;
                    case PerspectiveCamera or Spotlight:
                        // Update view-to-screen matrix
                        ViewToScreen.m11 = 2 * zNear / viewHeight;

                        // Update top and bottom clipping planes
                        float semiWidth = viewWidth / 2, semiHeight = viewHeight / 2;
                        ViewClippingPlanes[4].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, semiHeight, zNear), new Vector3D(semiWidth, semiHeight, zNear));
                        ViewClippingPlanes[1].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, -semiHeight, zNear), new Vector3D(-semiWidth, -semiHeight, zNear));

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
                zNear = value;
                UpdateRenderCamera();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        // Update view-to-screen matrix
                        ViewToScreen.m22 = 2 / (zFar - zNear);
                        ViewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                        // Update near clipping plane
                        ViewClippingPlanes[2].Point.z = zNear;

                        break;
                    case PerspectiveCamera or Spotlight:
                        // Update view-to-screen matrix
                        ViewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
                        ViewToScreen.m23 = -(2 * zFar * zNear) / (zFar - zNear);

                        // Update near clipping plane
                        ViewClippingPlanes[2].Point.z = zNear;

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
                zFar = value;
                UpdateRenderCamera();

                switch (this)
                {
                    case OrthogonalCamera or DistantLight:
                        // Update view-to-screen matrix
                        ViewToScreen.m22 = 2 / (zFar - zNear);
                        ViewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                        // Update far clipping plane
                        ViewClippingPlanes[5].Point.z = zFar;

                        break;
                    case PerspectiveCamera or Spotlight:
                        // Update view-to-screen matrix
                        ViewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
                        ViewToScreen.m23 = -(2 * zFar * zNear) / (zFar - zNear);

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
                renderWidth = value;
                UpdateRenderCamera();
            }
        }
        public virtual int RenderHeight
        {
            get => renderHeight;
            set
            {
                renderHeight = value;
                UpdateRenderCamera();
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
                volumeStyle = value;
                UpdateRenderCamera();

                VolumeEdges.Clear();

                float semiWidth = ViewWidth / 2, semiHeight = ViewHeight / 2;

                Vertex zeroPoint = new(new Vector4D(0, 0, 0, 1));
                Vertex nearTopLeftPoint = new(new Vector4D(-semiWidth, semiHeight, ZNear, 1));
                Vertex nearTopRightPoint = new(new Vector4D(semiWidth, semiHeight, ZNear, 1));
                Vertex nearBottomLeftPoint = new(new Vector4D(-semiWidth, -semiHeight, ZNear, 1));
                Vertex nearBottomRightPoint = new(new Vector4D(semiWidth, -semiHeight, ZNear, 1));

                if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
                {
                    VolumeEdges.AddRange(new Edge[]
                    {
                        new(zeroPoint, nearTopLeftPoint), // Near top left
                        new(zeroPoint, nearTopRightPoint), // Near top right
                        new(zeroPoint, nearBottomLeftPoint), // Near bottom left
                        new(zeroPoint, nearBottomRightPoint), // Near bottom right
                        new(nearTopLeftPoint, nearTopRightPoint), // Near top
                        new(nearBottomLeftPoint, nearBottomRightPoint), // Near bottom
                        new(nearTopLeftPoint, nearBottomLeftPoint), // Near left
                        new(nearTopRightPoint, nearBottomRightPoint) // Near right
                    });
                }

                if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
                {
                    float ratio = (this is OrthogonalCamera or DistantLight) ? 1 : ZFar / ZNear;
                    float semiWidthRatio = semiWidth * ratio, semiHeightRatio = semiHeight * ratio;

                    Vertex farTopLeftPoint = new(new Vector4D(-semiWidthRatio, semiHeightRatio, ZFar, 1));
                    Vertex farTopRightPoint = new(new Vector4D(semiWidthRatio, semiHeightRatio, ZFar, 1));
                    Vertex farBottomLeftPoint = new(new Vector4D(-semiWidthRatio, -semiHeightRatio, ZFar, 1));
                    Vertex farBottomRightPoint = new(new Vector4D(semiWidthRatio, -semiHeightRatio, ZFar, 1));

                    VolumeEdges.AddRange(new Edge[]
                    {
                        new(nearTopLeftPoint, farTopLeftPoint), // Far top left
                        new(nearTopRightPoint, farTopRightPoint), // Far top right
                        new(nearBottomLeftPoint, farBottomLeftPoint), // Far bottom left
                        new(nearBottomRightPoint, farBottomRightPoint), // Far bottom right
                        new(farTopLeftPoint, farTopRightPoint), // Far top
                        new(farBottomLeftPoint, farBottomRightPoint), // Far bottom
                        new(farTopLeftPoint, farBottomLeftPoint), // Far left
                        new(farTopRightPoint, farBottomRightPoint) // Far right
                    });
                }
            }
        }

        #endregion

        #region Constructors

        internal RenderingObject(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp)
        {
            switch (this)
            {
                case OrthogonalCamera or DistantLight:
                    ViewToScreen = Matrix4x4.Identity;

                    ViewClippingPlanes = new ClippingPlane[]
                    {
                        new(Vector3D.Zero, Vector3D.UnitX), // Left
                        new(Vector3D.Zero, Vector3D.UnitY), // Bottom
                        new(Vector3D.Zero, Vector3D.UnitZ), // Near
                        new(Vector3D.Zero, Vector3D.UnitNegativeX), // Right
                        new(Vector3D.Zero, Vector3D.UnitNegativeY), // Top
                        new(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
                    };

                    break;
                case PerspectiveCamera or Spotlight:
                    ViewToScreen = Matrix4x4.Zero;
                    ViewToScreen.m32 = 1;

                    ViewClippingPlanes = new ClippingPlane[]
                    {
                        new(Vector3D.Zero, Vector3D.Zero), // Left
                        new(Vector3D.Zero, Vector3D.Zero), // Bottom
                        new(Vector3D.Zero, Vector3D.UnitZ), // Near
                        new(Vector3D.Zero, Vector3D.Zero), // Right
                        new(Vector3D.Zero, Vector3D.Zero), // Top
                        new(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
                    };

                    break;
                default:
                    throw Exceptions.RenderingObjectTypeNotSupported;
            }

            ZNear = zNear;
            ZFar = zFar;
            this.viewHeight = viewHeight;
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
            this.renderHeight = renderHeight;
            RenderWidth = renderWidth;
            RenderHeight = renderHeight;
        }

        #endregion
    }
}