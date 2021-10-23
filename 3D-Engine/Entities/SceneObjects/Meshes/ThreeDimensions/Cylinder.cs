/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 *
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Cylinder"/> mesh.
    /// </summary>
    public sealed class Cylinder : Mesh
    {
        #region Fields and Properties

        private float height, radius;
        private int resolution;

        /// <summary>
        /// The height of the <see cref="Cylinder"/>.
        /// </summary>
        public float Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The radius of the top and bottom <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The number of vertices that are on the perimeter of each of the <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                Vertices = new Vertex[2 * resolution + 2];
                Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertices[1] = new Vertex(new Vector4D(0, 1, 0, 1));

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    Vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
                    Vertices[i + resolution + 2] = new Vertex(new Vector4D(Cos(angle * i), 1, Sin(angle * i), 1));
                }

                Edges = new Edge[2 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Edges[i] = new Edge(Vertices[i + 2], Vertices[i + 3]);
                    Edges[i + resolution] = new Edge(Vertices[i + resolution + 2], Vertices[i + resolution + 3]);
                }
                Edges[resolution - 1] = new Edge(Vertices[resolution + 1], Vertices[2]);
                Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution + 1], Vertices[resolution + 2]);

                Triangles = new SolidTriangle[4 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Triangles[i] = new SolidTriangle(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                    Triangles[i + resolution] = new SolidTriangle(Vertices[i + 2], Vertices[i + resolution + 3], Vertices[i + resolution + 2]);
                    Triangles[i + 2 * resolution] = new SolidTriangle(Vertices[i + 2], Vertices[i + 3], Vertices[i + resolution + 3]);
                    Triangles[i + 3 * resolution] = new SolidTriangle(Vertices[i + resolution + 2], Vertices[i + resolution + 3], Vertices[1]);
                }
                Triangles[resolution - 1] = new SolidTriangle(Vertices[resolution + 1], Vertices[0], Vertices[2]);
                Triangles[2 * resolution - 1] = new SolidTriangle(Vertices[resolution + 1], Vertices[resolution + 2], Vertices[2 * resolution + 1]);
                Triangles[3 * resolution - 1] = new SolidTriangle(Vertices[resolution + 1], Vertices[2], Vertices[resolution + 2]);
                Triangles[4 * resolution - 1] = new SolidTriangle(Vertices[2 * resolution + 1], Vertices[resolution + 2], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cylinder"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cylinder"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cylinder"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cylinder"/>.</param>
        /// <param name="height">The height of the <see cref="Cylinder"/>.</param>
        /// <param name="radius">The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
        public Cylinder(Vector3D worldOrigin,
                        Orientation worldOrientation,
                        float height,
                        float radius,
                        int resolution) : base(worldOrigin, worldOrientation, 3)
        {
            Height = height;
            Radius = radius;
            Resolution = resolution;
        }

        #endregion
    }
}