﻿/*
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

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Mesh']/*"/>
    public abstract partial class Mesh : SceneObject
    {
        #region Fields and Properties

        // Structure
        public IList<Vertex> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }
        public IList<Face> Faces { get; set; }

        /// <summary>
        /// The <see cref="Vertex">vertices</see> in the <see cref="Mesh"/>.
        /// </summary>
        //public Vertex[] Vertices { get; protected set; }
        /// <summary>
        /// The <see cref="Edge">edges</see> in the <see cref="Mesh"/>.
        /// </summary>
        //public Edge[] Edges { get; protected set; }
        /// <summary>
        /// The <see cref="Triangle">triangles</see> in the <see cref="Mesh"/>.
        /// </summary>
        //public Triangle[] Triangles { get; internal set; }

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
                RequestNewRenders();
            }
        }

        private bool drawFaces = true;
        /// <summary>
        /// Determines if the<see cref="Mesh"> Mesh's</see> <see cref="Triangle">Faces</see> are drawn.
        /// </summary>
        public bool DrawFaces
        {
            get => drawFaces;
            set
            {
                if (value == drawFaces) return;
                drawFaces = value;
                RequestNewRenders();
            }
        }

        // Headed Rendering Object
        internal List<RenderingObject> HeadedRenderingObjects { get; set; } = new();
        internal override void RequestNewRenders()
        {
            base.RequestNewRenders();

            foreach (RenderingObject renderingObject in HeadedRenderingObjects)
            {
                foreach (Camera camera in renderingObject.RenderCameras)
                {
                    camera.NewRenderNeeded = true;
                }
            }
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
        public float Opacity { get; set; } = 1f;

        // Matrices and Vectors
        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();
            ModelToWorld *= Transform.Scale(Scaling);
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
                RequestNewRenders();
            }
        }

        #endregion

        #region Constructors

        internal Mesh(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      int dimension,
                      bool hasDirectionArrows = true) : base(worldOrigin, worldOrientation, hasDirectionArrows)
        {
            Dimension = dimension;
        }

        #endregion
    }
}