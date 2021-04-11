/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a circle mesh.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.SceneObjects.Meshes.Components.Faces;
using System;
using static System.MathF;

namespace _3D_Engine.SceneObjects.Meshes.TwoDimensions
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Circle']/*"/>
    public sealed class Circle : Mesh
    {
        #region Fields and Properties

        private float radius;
        private int resolution;

        /// <summary>
        /// The radius of the <see cref="Circle"/>.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, 1, radius);
            }
        }

        /// <summary>
        /// The number of <see cref="Vertex">vertices</see> that are on the perimeter of the <see cref="Circle"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                if (value < 3) throw new ArgumentException("Resolution cannot be less than 3.");
                resolution = value;

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();
            }
        }

        #endregion

        #region Constructors

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Circle.#ctor(_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Single,System.Int32)']/*"/>
        public Circle(Vector3D origin, Vector3D direction_forward, Vector3D normal, float radius, int resolution) : base(origin, direction_forward, normal)
        {
            Dimension = 2;

            Radius = radius;
            Resolution = resolution;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Circle.#ctor(_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Single,System.Int32,_3D_Engine.Texture)']/*"/>
        public Circle(Vector3D origin, Vector3D directionForward, Vector3D normal, float radius, int resolution, Texture texture) : base(origin, directionForward, normal)
        {
            Dimension = 2;

            Radius = radius;
            Textures = new Texture[1] { texture };
            Resolution = resolution;
        }

        #endregion

        #region Methods

        private void GenerateVertices()
        {
            // Vertices are defined in anti-clockwise order.
            Vertices = new Vertex[resolution + 1]; // ?
            Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));

            if (Textures is not null)
            {
                Textures[0].Vertices = new Vector3D[resolution + 1];
                Textures[0].Vertices[0] = new Vector3D(0.5f, 0.5f, 1);

                for (int i = 0; i < resolution; i++) Textures[0].Vertices[i + 1] = new Vector3D(Cos(angle * i) * 0.5f, Sin(angle * i) * 0.5f, 1);
            }
        }
        private void GenerateEdges()
        {
            Edges = new Edge[resolution];
            for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 1], Vertices[i + 2]);
            Edges[resolution - 1] = new Edge(Vertices[resolution], Vertices[1]);
        }
        private void GenerateFaces()
        {
            if (Textures is null)
            {
                Faces = new SolidFace[resolution];
                for (int i = 0; i < resolution - 1; i++) Faces[i] = new SolidFace(Vertices[i + 1], Vertices[0], Vertices[i + 2]);
                Faces[resolution - 1] = new SolidFace(Vertices[resolution], Vertices[0], Vertices[1]);
            }
            else
            {
                Faces = new TextureFace[resolution];
                for (int i = 0; i < resolution - 1; i++) Faces[i] = new TextureFace(Vertices[i + 1], Vertices[0], Vertices[i + 2], Textures[0].Vertices[i + 1], Textures[0].Vertices[0], Textures[0].Vertices[i + 2], Textures[0]);
                Faces[resolution - 1] = new TextureFace(Vertices[resolution], Vertices[0], Vertices[1], Textures[0].Vertices[resolution], Textures[0].Vertices[0], Textures[0].Vertices[1], Textures[0]);
            }
        }

        #endregion
    }
}