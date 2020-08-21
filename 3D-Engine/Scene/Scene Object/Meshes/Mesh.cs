using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Mesh : Scene_Object
    {
        #region Fields and Properties

        // Structure
        /// <summary>
        /// The <see cref="Vertex">Vertices</see> in the <see cref="Mesh"/>.
        /// </summary>
        public Vertex[] Vertices { get; protected set; }
        /// <summary>
        /// The <see cref="Edge"/>s in the <see cref="Mesh"/>.
        /// </summary>
        public Edge[] Edges { get; protected set; }
        /// <summary>
        /// The <see cref="Face"/>s in the <see cref="Mesh"/>.
        /// </summary>
        public Face[] Faces { get; internal set; }
        /// <summary>
        /// The <see cref="Texture">Textures</see> that define what to draw on the surface of the <see cref="Mesh"/>.
        /// </summary>
        public Texture[] Textures { get; internal set; }

        // Appearance
        /// <summary>
        /// Determines if the <see cref="Mesh">Mesh's</see> <see cref="Edge">Edges</see> are drawn.
        /// </summary>
        public bool Draw_Edges { get; set; } = true;
        /// <summary>
        /// Determines if the <see cref="Mesh">Mesh's</see> <see cref="Face">Faces</see> are drawn.
        /// </summary>
        public bool Draw_Faces { get; set; } = true;

        // Colours
        private Color edge_colour, face_colour;
        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Edge"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color Edge_Colour
        {
            get => edge_colour;
            set
            {
                edge_colour = value;
                foreach (Edge edge in Edges) edge.Colour = edge_colour;
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
                foreach (Face face in Faces) face.Colour = face_colour;
            }
        }

        // Miscellaneous
        /// <summary>
        /// Determines if an outline is drawn with the <see cref="Mesh"/>.
        /// </summary>
        public bool Draw_Outline { get; set; } = false;

        // Matrices and Vectors
        internal override void Calculate_Model_to_World_Matrix()
        {
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(Model_Direction_Forward, World_Direction_Forward);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_forward_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 scale = Transform.Scale(Scaling.X, Scaling.Y, Scaling.Z);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            // String the transformations together in the following order:
            // 1) Scale
            // 2) Rotation around direction forward vector
            // 3) Rotation around direction up vector
            // 4) Translation to final position in world space
            Model_to_World = translation * direction_up_rotation * direction_forward_rotation * scale;
        }

        internal Vector3D Scaling { get; set; } = Vector3D.One;

        #endregion

        #region Constructors

        internal Mesh(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows) { }

        #endregion
    }
}