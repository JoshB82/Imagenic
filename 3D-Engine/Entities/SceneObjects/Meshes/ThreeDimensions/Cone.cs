/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a cone.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Cone"/> mesh.
    /// </summary>
    public sealed class Cone : Mesh
    {
        #region Fields and Properties

        private float height, radius;
        private int resolution;

        /// <summary>
        /// The height of the <see cref="Cone"/>.
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
        /// The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.
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
        /// The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                if (value == resolution) return;
                resolution = value;
                RequestNewRenders();

                // Vertices
                // They are defined in anti-clockwise order, looking from above and then downwards.
                Vertices = new Vertex[resolution + 2];
                Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertices[1] = new Vertex(new Vector4D(0, 1, 0, 1));

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));

                // Edges
                Edges = new Edge[resolution];

                for (int i = 0; i < resolution - 1; i++) Edges[i] = new SolidEdge(Vertices[i + 2], Vertices[i + 3]);
                Edges[resolution - 1] = new SolidEdge(Vertices[resolution + 1], Vertices[2]);

                // Faces
                Faces = new Face[resolution + 1];

                Triangle[] baseTriangles = new Triangle[resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    baseTriangles[i] = new SolidTriangle(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                }
                baseTriangles[resolution - 1] = new SolidTriangle(Vertices[resolution - 1], Vertices[0], Vertices[2]);
                Faces[0] = new Face(baseTriangles);

                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i + 1] = new Face(new SolidTriangle(Vertices[i + 2], Vertices[i + 3], Vertices[1]));
                }
                Faces[2 * resolution - 1] = new Face(new SolidTriangle(Vertices[resolution - 1], Vertices[2], Vertices[1]));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cone"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cone"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cone"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cone"/>.</param>
        /// <param name="height">The height of the <see cref="Cone"/>.</param>
        /// <param name="radius">The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        /// <param name="resolution">The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        public Cone(Vector3D worldOrigin,
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