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
using _3D_Engine.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.SceneObjects.RenderingObjects;
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
        private bool drawEdges = true;
        /// <summary>
        /// Determines if the <see cref="Mesh"> Mesh's</see> <see cref="Edge">Edges</see> are drawn.
        /// </summary>
        public bool DrawEdges
        {
            get => drawEdges;
            set
            {
                if (value == drawEdges) return;
                drawEdges = value;
                OnUpdate();
            }
        }

        private bool drawFaces = true;
        /// <summary>
        /// Determines if the<see cref="Mesh"> Mesh's</see> <see cref="Face">Faces</see> are drawn.
        /// </summary>
        public bool DrawFaces
        {
            get => drawFaces;
            set
            {
                if (value == drawFaces) return;
                drawFaces = value;
                OnUpdate();
            }
        }

        /*
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
                //foreach (Face face in Faces) face.Colour = faceColour; TODO: fix
            }
        }
        */

        // Headed Rendering Object
        internal RenderingObject HeadedRenderingObject { get; set; }
        internal void UpdateHeadedRenderingObject()
        {
            if (HeadedRenderingObject is not null) HeadedRenderingObject.RenderCamera.NewRenderNeeded = true;
        }

        // Textures
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Textures']/*"/>
        public Texture[] Textures { get; internal set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Has_Texture']/*"/>
        public bool HasTexture { get; protected set; } = false;

        // Miscellaneous
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Dimension']/*"/>
        public int Dimension { get; internal set; }
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Casts_Shadows']/*"/>
        public bool CastsShadows { get; set; } = true;
        /// <include file="Help_8.xml" path="doc/members/member[@name='P:_3D_Engine.Mesh.Draw_Outline']/*"/>
        public bool DrawOutline { get; set; } = false;

        // Matrices and Vectors
        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();
            ModelToWorld *= Transform.Scale(Scaling.x, Scaling.y, Scaling.z);
        }

        private Vector3D scaling = Vector3D.One;
        internal Vector3D Scaling
        {
            get => scaling;
            set
            {
                if (value == scaling) return;
                scaling = value;
                CalculateMatrices();
                OnUpdate();
            }
        }

        #endregion

        #region Constructors

        internal Mesh(Vector3D origin, Vector3D directionForward, Vector3D directionUp, bool hasDirectionArrows = true) : base(origin, directionForward, directionUp, hasDirectionArrows)
        {
            Update += (sender, eventArgs) => { if (HeadedRenderingObject is not null) HeadedRenderingObject.RenderCamera.NewRenderNeeded = true; };
        }

        #endregion

        #region Methods

        public void ColourSolidFaces(Color colour)
        {
            foreach (Face face in Faces)
            {
                if (face is SolidFace solidFace)
                {
                    solidFace.Colour = colour;
                }
            }
        }

        #endregion
    }
}