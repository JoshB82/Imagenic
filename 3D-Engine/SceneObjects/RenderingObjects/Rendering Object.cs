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
        internal ClippingPlane[] ScreenClippingPlanes { get; set; }

        // Matrices
        internal Matrix4x4 WorldToView { get; set; }
        internal Matrix4x4 ViewToScreen;
        internal Matrix4x4 ScreenToWindow;

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            WorldToView = ModelToWorld.Inverse();
        }

        // View Volume
        private float width, height, znear, zfar;
        private int windowWidth, windowHeight;

        /// <summary>
        /// The width of the <see cref="RenderingObject">RenderingObject's</see> view/near plane.
        /// </summary>
        public virtual float ViewWidth
        {
            get => width;
            set
            {
                width = value;
                UpdateRenderCamera();
            }
        }
        /// <summary>
        /// The height of the <see cref="RenderingObject">RenderingObject's</see> view/near plane.
        /// </summary>
        public virtual float ViewHeight
        {
            get => height;
            set
            {
                height = value;
                UpdateRenderCamera();
            }
        }
        /// <summary>
        /// The depth of the <see cref="RenderingObject">RenderingObject's</see> view to the near plane.
        /// </summary>
        public virtual float ZNear
        {
            get => znear;
            set
            {
                znear = value;
                UpdateRenderCamera();
            }
        }
        /// <summary>
        /// The depth of the <see cref="RenderingObject">RenderingObject's</see> view to the far plane.
        /// </summary>
        public virtual float ZFar
        {
            get => zfar;
            set
            {
                zfar = value;
                UpdateRenderCamera();
            }
        }

        public virtual int WindowWidth
        {
            get => windowWidth;
            set
            {
                windowWidth = value;
                UpdateRenderCamera();
            }
        }
        public virtual int WindowHeight
        {
            get => windowHeight;
            set
            {
                windowHeight = value;
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

        protected static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0)); //?

        #endregion

        #region Constructors

        internal RenderingObject(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : base(origin, directionForward, directionUp) { }

        #endregion
    }
}