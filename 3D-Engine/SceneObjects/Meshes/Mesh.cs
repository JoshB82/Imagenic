/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a mesh.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.Transformations;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Meshes
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Mesh']/*"/>
    public abstract partial class Mesh : SceneObject
    {
        #region Fields and Properties

        // Structure
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Vertices']/*"/>
        public Vertex[] Vertices { get; protected set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Edges']/*"/>
        public Edge[] Edges { get; protected set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Faces']/*"/>
        public Face[] Faces { get; internal set; }

        // Appearance
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Edges']/*"/>
        public bool Draw_Edges { get; set; } = true;
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Faces']/*"/>
        public bool Draw_Faces { get; set; } = true;

        // Colours
        private Color edgeColour, faceColour;

        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Edge"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color EdgeColour
        {
            get => edgeColour;
            set
            {
                edgeColour = value;
                foreach (Edge edge in Edges) edge.Colour = edgeColour;
            }
        }
        /// <summary>
        /// The <see cref="Color"/> of each <see cref="Face"/> in the <see cref="Mesh"/>.
        /// </summary>
        public Color FaceColour
        {
            get => faceColour;
            set
            {
                faceColour = value;
                foreach (Face face in Faces) face.Colour = faceColour;
            }
        }

        // Textures
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Textures']/*"/>
        public Texture[] Textures { get; internal set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Has_Texture']/*"/>
        public bool Has_Texture { get; protected set; } = false;

        // Miscellaneous
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Dimension']/*"/>
        public int Dimension { get; internal set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Casts_Shadows']/*"/>
        public bool Casts_Shadows { get; set; } = true;
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Outline']/*"/>
        public bool Draw_Outline { get; set; } = false;

        // Matrices and Vectors
        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            ModelToWorld *= Transform.Scale(Scaling.x, Scaling.y, Scaling.z);
        }

        internal Vector3D Scaling = Vector3D.One;

        #endregion

        #region Constructors

        internal Mesh(Vector3D origin, Vector3D directionForward, Vector3D directionUp, bool hasDirectionArrows = true) : base(origin, directionForward, directionUp, hasDirectionArrows) { }

        #endregion
    }
}