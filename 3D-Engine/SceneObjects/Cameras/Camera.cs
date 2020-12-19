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

                Vertex zero_point = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertex near_top_left_point = new Vertex(new Vector4D(-semiWidth, semiHeight, ZNear, 1));
                Vertex near_top_right_point = new Vertex(new Vector4D(semiWidth, semiHeight, ZNear, 1));
                Vertex near_bottom_left_point = new Vertex(new Vector4D(-semiWidth, -semiHeight, ZNear, 1));
                Vertex near_bottom_right_point = new Vertex(new Vector4D(semiWidth, -semiHeight, ZNear, 1));

                if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
                {
                    VolumeEdges.AddRange(new[]
                    {
                        new Edge(zero_point, near_top_left_point), // Near top left
                        new Edge(zero_point, near_top_right_point), // Near top right
                        new Edge(zero_point, near_bottom_left_point), // Near bottom left
                        new Edge(zero_point, near_bottom_right_point), // Near bottom right
                        new Edge(near_top_left_point, near_top_right_point), // Near top
                        new Edge(near_bottom_left_point, near_bottom_right_point), // Near bottom
                        new Edge(near_top_left_point, near_bottom_left_point), // Near left
                        new Edge(near_top_right_point, near_bottom_right_point) // Near right
                    });
                }

                if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
                {
                    float ratio = (this is OrthogonalCamera) ? 1 : ZFar / ZNear;
                    float semi_width_ratio = semiWidth * ratio, semi_height_ratio = semiHeight * ratio;

                    Vertex far_top_left_point = new Vertex(new Vector4D(-semi_width_ratio, semi_height_ratio, ZFar, 1));
                    Vertex far_top_right_point = new Vertex(new Vector4D(semi_width_ratio, semi_height_ratio, ZFar, 1));
                    Vertex far_bottom_left_point = new Vertex(new Vector4D(-semi_width_ratio, -semi_height_ratio, ZFar, 1));
                    Vertex far_bottom_right_point = new Vertex(new Vector4D(semi_width_ratio, -semi_height_ratio, ZFar, 1));

                    VolumeEdges.AddRange(new[]
                    {
                        new Edge(near_top_left_point, far_top_left_point), // Far top left
                        new Edge(near_top_right_point, far_top_right_point), // Far top right
                        new Edge(near_bottom_left_point, far_bottom_left_point), // Far bottom left
                        new Edge(near_bottom_right_point, far_bottom_right_point), // Far bottom right
                        new Edge(far_top_left_point, far_top_right_point), // Far top
                        new Edge(far_bottom_left_point, far_bottom_right_point), // Far bottom
                        new Edge(far_top_left_point, far_bottom_left_point), // Far left
                        new Edge(far_top_right_point, far_bottom_right_point) // Far right
                    });
                }
            }
        }

        internal List<Edge> VolumeEdges = new List<Edge>();

        // Matrices
        internal Matrix4x4 WorldToCameraView, CameraViewToCameraScreen, CameraScreenToWorld;

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();
            WorldToCameraView = ModelToWorld.Inverse();
            CameraScreenToWorld = ModelToWorld * CameraViewToCameraScreen.Inverse();
        }

        // Clipping planes
        internal Clipping_Plane[] Camera_View_Clipping_Planes;
        internal static readonly Clipping_Plane[] Camera_Screen_Clipping_Planes = new[]
        {
            new Clipping_Plane(-Vector3D.One, Vector3D.UnitX), // Left
            new Clipping_Plane(-Vector3D.One, Vector3D.UnitY), // Bottom
            new Clipping_Plane(-Vector3D.One, Vector3D.UnitZ), // Near
            new Clipping_Plane(Vector3D.One, Vector3D.UnitNegativeX), // Right
            new Clipping_Plane(Vector3D.One, Vector3D.UnitNegativeY), // Top
            new Clipping_Plane(Vector3D.One, Vector3D.UnitNegativeZ) // Far
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