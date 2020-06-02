using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Mesh
    {
        #region Fields and Properties

        // Origins
        internal Vector4D Origin { get; set; } = Vector4D.Zero;
        public Vector3D World_Origin { get; set; }

        // Structure
        internal Vector4D[] Vertices { get; set; }
        public Vector3D[] World_Vertices { get; protected set; }

        public Vector3D[] Texture_Vertices { get; protected set; }
        public Texture[] Textures { get; protected set; }

        public Spot[] Spots { get; protected set; }
        public Edge[] Edges { get; protected set; }
        public Face[] Faces { get; protected set; }

        // Directions
        public Vector3D Model_Direction { get; } = Vector3D.Unit_X;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_Z;

        public Vector3D World_Direction { get; private set; }
        public Vector3D World_Direction_Up { get; private set; }
        public Vector3D World_Direction_Right { get; private set; }

        // Appearance
        /// <summary>
        /// Determines if the mesh's spots are drawn.
        /// </summary>
        public bool Draw_Spots { get; set; } = true;
        /// <summary>
        /// Determines if the mesh's edges are drawn.
        /// </summary>
        public bool Draw_Edges { get; set; } = true;
        /// <summary>
        /// Determines if the mesh's faces are drawn.
        /// </summary>
        public bool Draw_Faces { get; set; } = true;

        // Colours
        private Color spot_colour, edge_colour, face_colour;
        /// <summary>
        /// The colour of each spot in the mesh.
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
        /// The colour of each edge in the mesh.
        /// </summary>
        public Color Edge_Colour
        {
            get => edge_colour;
            set
            {
                edge_colour = value;
                // Not entirely sure why can't use foreach loop :/
                for (int i = 0; i < Edges.Length; i++) Edges[i].Colour = value;
            }
        }
        /// <summary>
        /// The colour of each face in the mesh.
        /// </summary>
        public Color Face_Colour
        {
            get => face_colour;
            set
            {
                face_colour = value;
                // Not entirely sure why can't use foreach loop :/
                for (int i = 0; i < Faces.Length; i++) Faces[i].Colour = value;
            }
        }

        // Miscellaneous
        /// <summary>
        /// Determines if an outline is drawn with the mesh.
        /// </summary>
        public bool Draw_Outline { get; set; } = false;
        /// <summary>
        /// Determines if the mesh is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        // Object transformations
        public Matrix4x4 Model_to_World { get; private set; }
        public Vector3D Scaling { get; protected set; } = Vector3D.One;

        #endregion

        public void Calculate_Model_to_World_Matrix()
        {
            // Scale, then rotate, then translate
            Matrix4x4 direction_rotation = Transform.Rotate_Between_Vectors(Model_Direction, World_Direction);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 scale = Transform.Scale(Scaling.X, Scaling.Y, Scaling.Z);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            Model_to_World = translation * direction_up_rotation * direction_rotation * scale;
        }

        // Could world and model points be put into a single struct? With overloading possibly so the computer knows how to handle them?
    }
}