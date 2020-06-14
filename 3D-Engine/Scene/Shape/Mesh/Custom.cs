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

        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up,
            Vector4D[] vertices,
            Vector3D[] texture_vertices,
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
            Texture_Vertices = texture_vertices;

            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up, string file_path)
        {
            // Check if the file exists
            if (!File.Exists(file_path))
            {
                Debug.WriteLine($"{file_path} not found.");
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
                            x = Double.Parse(data[1]);
                            y = Double.Parse(data[2]);
                            z = Double.Parse(data[3]);
                            w = (data.Length == 5) ? Double.Parse(data[4]) : 1;
                            vertices.Add(new Vector4D(x, y, z, w));
                            break;
                        case "vt":
                            // Texture vertex
                            u = Double.Parse(data[1]);
                            v = (data.Length > 2) ? Double.Parse(data[2]) : 0;
                            w = (data.Length == 4) ? Double.Parse(data[3]) : 0;
                            texture_vertices.Add(new Vector3D(u, v, w));
                            break;
                        case "l":
                            // Line (or polyline)
                            int no_end_points = data.Length - 1;
                            do
                            {
                                p1 = Int32.Parse(data[no_end_points]) - 1;
                                p2 = Int32.Parse(data[no_end_points - 1]) - 1;
                                edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1], Color.Black));
                                no_end_points--;
                            }
                            while (no_end_points > 1);
                            break;
                        case "f":
                            // Face
                            p1 = Int32.Parse(data[1]) - 1;
                            p2 = Int32.Parse(data[2]) - 1;
                            p3 = Int32.Parse(data[3]) - 1;
                            faces.Add(new Face(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1], Color.BlueViolet));
                            break;
                    }
                }

                
            }
            // Will it stop in the event of an error?
            catch (Exception error) { Debug.WriteLine($"Error: {error.Message}"); }

            Vertices = vertices.ToArray();
            Draw_Spots = false;
            Edges = edges.ToArray();
            Faces = faces.ToArray();
            //TexturesTexture_Vertices
            
            Debug.WriteLine($"Custom mesh created at {origin}");
        }

        public Custom(Vector3D origin, Vector3D direction, Vector3D direction_up, Mesh m1, Mesh m2)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Vertices = m1.Vertices.Concat(m2.Vertices).ToArray(); // Not entirely sure how this works?
            Spots = m1.Spots.Concat(m2.Spots).ToArray();
            Edges = m1.Edges.Concat(m2.Edges).ToArray();
            Faces = m1.Faces.Concat(m2.Faces).ToArray();
            Textures = m1.Textures.Concat(m2.Textures).ToArray();
            Texture_Vertices = m1.Texture_Vertices.Concat(m2.Texture_Vertices).ToArray();

            Debug.WriteLine($"Custom mesh created at {origin}");
        }
    }
}