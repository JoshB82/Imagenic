using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Mesh : Scene_Object
    {
        #region Fields and Properties

        // Structure
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Vertices']/*"/>
        public Vertex[] Vertices { get; protected set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Edges']/*"/>
        public Edge[] Edges { get; protected set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Faces']/*"/>
        public Face[] Faces { get; internal set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Textures']/*"/>
        public Texture[] Textures { get; internal set; }
        public bool Has_Texture { get; protected set; } = false;

        // Appearance
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Edges']/*"/>
        public bool Draw_Edges { get; set; } = true;
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Faces']/*"/>
        public bool Draw_Faces { get; set; } = true;

        // Colours
        private Color edge_colour, face_colour;

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Edge_Colour']/*"/>
        public Color Edge_Colour
        {
            get => edge_colour;
            set
            {
                edge_colour = value;
                foreach (Edge edge in Edges) edge.Colour = edge_colour;
            }
        }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Face_Colour']/*"/>
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
        /// Represents the number of dimensions the <see cref="Mesh"/> occupies.
        /// </summary>
        public int Dimension { get; protected set; }
        public bool Casts_Shadows { get; set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Outline']/*"/>
        public bool Draw_Outline { get; set; } = false;

        // Matrices and Vectors
        internal override void Calculate_Model_to_World()
        {
            base.Calculate_Model_to_World();

            Matrix4x4 scale = Transform.Scale(Scaling.x, Scaling.y, Scaling.z);
            Model_to_World *= scale;
        }

        internal Vector3D Scaling = Vector3D.One;

        #endregion

        #region Constructors

        internal Mesh(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows) { }

        #endregion
    }
}