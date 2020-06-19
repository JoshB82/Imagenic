using System;
using System.Diagnostics;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Circle"/> mesh.
    /// </summary>
    public sealed class Circle : Mesh
    {
        #region Fields and Properties

        private double radius;
        private int resolution;

        /// <summary>
        /// The radius of the <see cref="Circle"/>.
        /// </summary>
        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, 1, radius);
            }
        }
        /// <summary>
        /// The number of points that are on the perimeter of the <see cref="Circle"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                Vertices = new Vector4D[resolution + 1]; // ?
                Vertices[0] = Vector4D.Zero; // ?

                double angle = 2 * Math.PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vector4D(Math.Cos(angle * i), 0, Math.Sin(angle * i));

                if (Textures != null)
                {
                    Textures[0].Vertices = new Vector3D[resolution + 1];
                    Textures[0].Vertices[0] = new Vector3D(0.5, 0.5, 1);

                    for (int i = 0; i < resolution; i++) Textures[0].Vertices[i + 1] = new Vector3D(Math.Cos(angle * i) * 0.5, Math.Sin(angle * i) * 0.5, 1);
                }

                Spots = new Spot[1] { new Spot(Vertices[0]) };

                Edges = new Edge[resolution];
                for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 1], Vertices[i + 2]);
                Edges[resolution - 1] = new Edge(Vertices[resolution], Vertices[1]);

                Faces = new Face[resolution];
                for (int i = 0; i < resolution - 1; i++) Faces[i] = new Face(Vertices[i + 1], Vertices[0], Vertices[i + 2]);
                Faces[resolution - 1] = new Face(Vertices[resolution], Vertices[0], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Circle"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Circle"/>.</param>
        /// <param name="direction">The direction the <see cref="Circle"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Circle"/>. This is also a normal to the surface of the <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of points that are on the perimeter of the <see cref="Circle"/>.</param>
        public Circle(Vector3D origin, Vector3D direction, Vector3D normal, double radius, int resolution)
        {
            Radius = radius;
            Resolution = resolution;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, normal);

            Debug.WriteLine($"Circle created at {origin}");
        }

        /// <summary>
        /// Creates a textured <see cref="Circle"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Circle"/>.</param>
        /// <param name="direction">The direction the <see cref="Circle"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Circle"/>. This is also a normal to the surface of the <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of points that are on the perimeter of the <see cref="Circle"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on the surface of the <see cref="Circle"/>.</param>
        public Circle(Vector3D origin, Vector3D direction, Vector3D normal, double radius, int resolution, Texture texture)
        {
            Radius = radius;
            Textures = new Texture[1] { texture };
            Resolution = resolution;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, normal);
            
            Debug.WriteLine($"Circle created at {origin}");
        }

        #endregion
    }
}