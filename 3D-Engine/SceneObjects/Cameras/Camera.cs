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
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Camera"/>.
    /// </summary>
    public abstract partial class Camera : SceneObject
    {
        #region Fields and Properties

        // Appearance
        public readonly Mesh Icon;

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
    }
}