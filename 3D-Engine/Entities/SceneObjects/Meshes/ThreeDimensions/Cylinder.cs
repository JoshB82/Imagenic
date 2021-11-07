/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a cylinder.
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
    /// A cylinder mesh.
    /// </summary>
    /// <remarks>
    /// Composition:<br/>
    ///
    /// </remarks>
    public sealed class Cylinder : Mesh
    {
        #region Fields and Properties

        public override MeshContent Content { get; set; } = new MeshContent();

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
                if (value == resolution) return;
                resolution = value;
                RequestNewRenders();

                Vertices = new Vertex[2 * resolution + 2];
                Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertices[1] = new Vertex(new Vector4D(0, 1, 0, 1));

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    Vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
                    Vertices[i + resolution + 2] = new Vertex(new Vector4D(Cos(angle * i), 1, Sin(angle * i), 1));
                }

                Edges = new Edge[3 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Edges[i] = new SolidEdge(Vertices[i + 2], Vertices[i + 3]);
                    Edges[i + resolution] = new SolidEdge(Vertices[i + resolution + 2], Vertices[i + resolution + 3]);
                    Edges[i + 2 * resolution] = new SolidEdge(Vertices[i + 2], Vertices[i + resolution + 2]);
                }
                Edges[resolution - 1] = new SolidEdge(Vertices[resolution + 1], Vertices[2]);
                Edges[2 * resolution - 1] = new SolidEdge(Vertices[2 * resolution + 1], Vertices[resolution + 2]);
                Edges[3 * resolution - 1] = new SolidEdge(Vertices[resolution + 1], Vertices[2 * resolution + 1]);

                Faces = new Face[resolution + 2];

                Triangle[] baseTriangles = new Triangle[resolution];
                Triangle[] topTriangles = new Triangle[resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    baseTriangles[i] = new SolidTriangle(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                    topTriangles[i] = new SolidTriangle(Vertices[i + resolution + 2], Vertices[i + resolution + 3], Vertices[1]);
                    Faces[i + 2] = new Face(
                        new SolidTriangle(Vertices[i + 2], Vertices[i + resolution + 3], Vertices[i + resolution + 2]),
                        new SolidTriangle(Vertices[i + 2], Vertices[i + 3], Vertices[i + resolution + 3])
                    );
                }
                baseTriangles[resolution - 1] = new SolidTriangle(Vertices[resolution + 1], Vertices[0], Vertices[2]);
                topTriangles[resolution - 1] = new SolidTriangle(Vertices[2 * resolution + 1], Vertices[resolution + 2], Vertices[1]);

                Faces[0] = new Face(baseTriangles);
                Faces[1] = new Face(topTriangles);
                Faces[resolution] = new Face(
                    new SolidTriangle(Vertices[resolution + 1], Vertices[resolution + 2], Vertices[2 * resolution + 1]),
                    new SolidTriangle(Vertices[resolution + 1], Vertices[2], Vertices[resolution + 2])
                );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cylinder"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cylinder"/>.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Cylinder"/> in world space.</param>
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