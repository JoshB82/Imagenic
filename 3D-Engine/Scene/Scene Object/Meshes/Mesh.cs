using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Mesh : Scene_Object
    {
        #region Fields and Properties

        // Structure
        /// <summary>
        /// The <see cref="Texture"/>s that define what to draw on the surface of the <see cref="Mesh"/>.
        /// </summary>
        public Texture[] Textures { get; internal set; }
        internal Vertex[] Vertices { get; set; }
        /// <summary>
        /// The positions of the vertices that make up the <see cref="Mesh"/> in world space.
        /// </summary>
        public Vector3D[] World_Vertices { get; protected set; }
        /// <summary>
        /// The <see cref="Spot"/>s in the <see cref="Mesh"/>.
        /// </summary>
        public Spot[] Spots { get; protected set; }
        /// <summary>
        /// The <see cref="Edge"/>s in the <see cref="Mesh"/>.
        /// </summary>
        public Edge[] Edges { get; protected set; }
        /// <summary>
        /// The <see cref="Face"/>s in the <see cref="Mesh"/>.
        /// </summary>
        public Face[] Faces { get; internal set; }

        // Appearance
        /// <summary>
        /// Determines if the <see cref="Mesh"/>'s <see cref="Spot"/>s are drawn.
        /// </summary>
        public bool Draw_Spots { get; set; } = true;
        /// <summary>
        /// Determines if the <see cref="Mesh"/>'s <see cref="Edge"/>s are drawn.
        /// </summary>
        public bool Draw_Edges { get; set; } = true;
        /// <summary>
        /// Determines if the <see cref="Mesh"/>'s <see cref="Face"/>s are drawn.
        /// </summary>
        public bool Draw_Faces { get; set; } = true;

        // Colours
        private Color spot_colour = Color.Green;
        private Color edge_colour;
        private Color face_colour;
        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Spot"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color Spot_Colour
        {
            get => spot_colour;
            set
            {
                spot_colour = value;
                // Not entirely sure why can't use foreach loop :/
                for (int i = 0; i < Spots.Length; i++) Spots[i].Colour = value;
            }
        }
        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Edge"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color Edge_Colour
        {
            get => edge_colour;
            set
            {
                edge_colour = value;
                for (int i = 0; i < Edges.Length; i++) Edges[i].Colour = value;
            }
        }
        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Face"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color Face_Colour
        {
            get => face_colour;
            set
            {
                face_colour = value;
                for (int i = 0; i < Faces.Length; i++) Faces[i].Colour = value;
            }
        }

        // Miscellaneous
        /// <summary>
        /// Determines if an outline is drawn with the <see cref="Mesh"/>.
        /// </summary>
        public bool Draw_Outline { get; set; } = false;
        /// <summary>
        /// Determines if the <see cref="Mesh"/> is visible or not.
        /// </summary>

        // Object transformations
        internal Matrix4x4 Model_to_World { get; private set; } = Matrix4x4.Identity_Matrix();
        internal Vector3D Scaling { get; set; } = Vector3D.One;

        #endregion

        internal void Calculate_Model_to_World_Matrix()
        {
            // Scale, then rotate, then translate
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(Model_Direction_Forward, World_Direction_Forward);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_forward_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 scale = Transform.Scale(Scaling.X, Scaling.Y, Scaling.Z);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            Model_to_World = translation * direction_up_rotation * direction_forward_rotation * scale;
        }

        // Could world and model points be put into a single struct? With overloading possibly so the computer knows how to handle them?

        #region Constructors

        internal Mesh(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows) { }

        #endregion
    }
}