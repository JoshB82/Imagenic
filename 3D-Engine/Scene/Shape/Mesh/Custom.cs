using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Custom"/> mesh.
    /// </summary>
    public sealed class Custom : Mesh
    {
        #region Constructors

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="vertices">The vertices that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="spots">The <see cref="Spot"/>s that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="edges">The <see cref="Edge"/>s that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="faces">The <see cref="Face"/>s that make up the <see cref="Custom"/> mesh.</param>
        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up,
            Vector4D[] vertices,
            Spot[] spots,
            Edge[] edges,
            Face[] faces)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Vertices = vertices;
            Spots = spots;
            Edges = edges;
            Faces = faces;

            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        /// <summary>
        /// Creates a textured <see cref="Custom"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="vertices">The vertices that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="spots">The <see cref="Spot"/>s that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="edges">The <see cref="Edge"/>s that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="faces">The <see cref="Face"/>s that make up the <see cref="Custom"/> mesh.</param>
        /// <param name="textures">The <see cref="Texture"/>s that make up the surface of the <see cref="Custom"/> mesh.</param>
        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up,
            Vector4D[] vertices,
            Spot[] spots,
            Edge[] edges,
            Face[] faces,
            Texture[] textures)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Vertices = vertices;
            Spots = spots;
            Edges = edges;
            Faces = faces;
            Textures = textures;

            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh from an OBJ file.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="file_path">The path to the OBJ file.</param>
        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up, string file_path)
        {
            // Check if the file exists
            if (!File.Exists(file_path))
            {
                Debug.WriteLine($"Error generating Custom mesh: {file_path} not found.");
                return;
            }

            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);
            
            List<Vector4D> vertices = new List<Vector4D>();
            List<Edge> edges = new List<Edge>();
            List<Face> faces = new List<Face>();

            try
            {
                string[] lines = File.ReadAllLines(file_path);
                foreach (string line in lines)
                {
                    string[] data = line.Split();
                    int p1, p2, p3;
                    double x, y, z, w;

                    switch (data[0])
                    {
                        case "#":
                            // Comment; ignore line
                            break;
                        case "v":
                            // Vertex
                            x = double.Parse(data[1]);
                            y = double.Parse(data[2]);
                            z = double.Parse(data[3]);
                            w = (data.Length == 5) ? Double.Parse(data[4]) : 1;
                            vertices.Add(new Vector4D(x, y, z, w));
                            break;
                        case "l":
                            // Line (or polyline)
                            int no_end_points = data.Length - 1;
                            do
                            {
                                p1 = int.Parse(data[no_end_points]) - 1;
                                p2 = int.Parse(data[no_end_points - 1]) - 1;
                                edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1], Color.Black));
                                no_end_points--;
                            }
                            while (no_end_points > 1);
                            break;
                        case "f":
                            // Face
                            p1 = int.Parse(data[1]) - 1;
                            p2 = int.Parse(data[2]) - 1;
                            p3 = int.Parse(data[3]) - 1;
                            faces.Add(new Face(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1], Color.BlueViolet));
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error generating Custom mesh: {error.Message}");
                return;
            }  

            Vertices = vertices.ToArray();
            Draw_Spots = false;
            Edges = edges.ToArray();
            Faces = faces.ToArray();
            
            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        /// <summary>
        /// Creates a textured <see cref="Custom"/> mesh from an OBJ file.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="file_path">The path to the OBJ file.</param>
        /// <param name="texture">The <see cref="Bitmap"/> that makes up the surface of the <see cref="Custom"/> mesh.</param>
        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up, string file_path, Bitmap texture)
        {
            // Check if the file exists
            if (!File.Exists(file_path))
            {
                Debug.WriteLine($"Error generating Custom mesh: {file_path} not found.");
                return;
            }

            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            List<Vector4D> vertices = new List<Vector4D>();
            List<Vector3D> texture_vertices = new List<Vector3D>();
            List<Edge> edges = new List<Edge>();
            List<Face> faces = new List<Face>();

            try
            {
                string[] lines = File.ReadAllLines(file_path);
                foreach (string line in lines)
                {
                    string[] data = line.Split();
                    int p1, p2, p3;
                    double x, y, z, u, v, w;

                    switch (data[0])
                    {
                        case "#":
                            // Comment; ignore line
                            break;
                        case "v":
                            // Vertex
                            x = double.Parse(data[1]);
                            y = double.Parse(data[2]);
                            z = double.Parse(data[3]);
                            w = (data.Length == 5) ? Double.Parse(data[4]) : 1;
                            vertices.Add(new Vector4D(x, y, z, w));
                            break;
                        case "vt":
                            // Texture vertex
                            u = double.Parse(data[1]);
                            v = (data.Length > 2) ? double.Parse(data[2]) : 0;
                            w = (data.Length == 4) ? double.Parse(data[3]) : 0;
                            texture_vertices.Add(new Vector3D(u, v, w));
                            break;
                        case "l":
                            // Line (or polyline)
                            int no_end_points = data.Length - 1;
                            do
                            {
                                p1 = int.Parse(data[no_end_points]) - 1;
                                p2 = int.Parse(data[no_end_points - 1]) - 1;
                                edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1], Color.Black));
                                no_end_points--;
                            }
                            while (no_end_points > 1);
                            break;
                        case "f":
                            // Face
                            p1 = int.Parse(data[1]) - 1;
                            p2 = int.Parse(data[2]) - 1;
                            p3 = int.Parse(data[3]) - 1;
                            faces.Add(new Face(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1], Color.BlueViolet));
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error generating Custom mesh: {error.Message}");
                return;
            }

            Vertices = vertices.ToArray();
            Draw_Spots = false;
            Edges = edges.ToArray();
            Faces = faces.ToArray();
            Textures = new Texture[] { new Texture(texture, texture_vertices.ToArray()) };

            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh from joining two other meshes.
        /// </summary>
        /// <param name="origin">The position of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="direction">The direction the resultant <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="m1">The first <see cref="Mesh"/> to be joined.</param>
        /// <param name="m2">The second <see cref="Mesh"/> to be joined.</param>
        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up, Mesh m1, Mesh m2)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Vertices = m1.Vertices.Concat(m2.Vertices).ToArray(); // Not entirely sure how this works?
            Spots = m1.Spots.Concat(m2.Spots).ToArray();
            Edges = m1.Edges.Concat(m2.Edges).ToArray();
            Faces = m1.Faces.Concat(m2.Faces).ToArray();
            Textures = m1.Textures.Concat(m2.Textures).ToArray();

            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        #endregion
    }
}