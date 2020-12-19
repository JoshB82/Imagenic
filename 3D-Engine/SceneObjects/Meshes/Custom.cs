using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace _3D_Engine.SceneObjects.Meshes
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Custom']/*"/>
    public sealed class Custom : Mesh
    {
        #region Constructors

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Custom.#ctor(_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vertex[],_3D_Engine.Edge[],_3D_Engine.Face[])']/*"/>
        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, Vertex[] vertices, Edge[] edges, Face[] faces) : base(origin, direction_forward, direction_up)
        {
            Vertices = vertices;
            Edges = edges;
            Faces = faces;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Custom.#ctor(_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vertex[],_3D_Engine.Edge[],_3D_Engine.Face[],_3D_Engine.Texture[])']/*"/>
        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up,
            Vertex[] vertices,
            Edge[] edges,
            Face[] faces,
            Texture[] textures) : base(origin, direction_forward, direction_up)
        {
            Vertices = vertices;
            Edges = edges;
            Faces = faces;

            Has_Texture = true;
            Textures = textures;
        }

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh from an OBJ file.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction_forward">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="file_path">The path of the OBJ file.</param>
        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string file_path) : base(origin, direction_forward, direction_up)
        {
            // Check if the file exists
            if (!File.Exists(file_path))
            {
                Trace.WriteLine($"Error generating Custom mesh: The file {file_path} was not found.");
                return;
            }

            // Obtain data from file
            string[] lines;
            try
            {
                lines = File.ReadAllLines(file_path);
            }
            catch (Exception error)
            {
                Trace.WriteLine($"Error generating Custom mesh: {error.Message}");
                return;
            }

            // Create the mesh
            Generate_Custom_From_OBJ(lines);
        }

        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string[] lines) : base(origin, direction_forward, direction_up) => Generate_Custom_From_OBJ(lines);

        private void Generate_Custom_From_OBJ(string[] lines)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<Edge> edges = new List<Edge>();
            List<Face> faces = new List<Face>();

            foreach (string line in lines)
            {
                string[] data = line.Split();
                int p1, p2, p3;
                float x, y, z, w;

                switch (data[0])
                {
                    case "#":
                        // Comment; ignore line
                        break;
                    case "v":
                        // Vertex
                        x = float.Parse(data[1]);
                        y = float.Parse(data[2]);
                        z = float.Parse(data[3]);
                        w = (data.Length == 5) ? float.Parse(data[4]) : 1;
                        vertices.Add(new Vertex(new Vector4D(x, y, z, w)));
                        break;
                    case "l":
                        // Line (or polyline)
                        int no_end_points = data.Length - 1;
                        do
                        {
                            p1 = int.Parse(data[no_end_points]) - 1;
                            p2 = int.Parse(data[no_end_points - 1]) - 1;
                            edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1]));
                            no_end_points--;
                        }
                        while (no_end_points > 1);
                        break;
                    case "f":
                        // Face
                        p1 = int.Parse(data[1]);
                        p2 = int.Parse(data[2]);
                        p3 = int.Parse(data[3]);
                        faces.Add(new Face(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1]));
                        break;
                }
            }

            Vertices = vertices.ToArray();
            Edges = edges.ToArray();
            Faces = faces.ToArray();
        }
//b
        /// <summary>
        /// Creates a textured <see cref="Custom"/> mesh from an OBJ file.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="direction_forward">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="file_path">The path of the OBJ file.</param>
        /// <param name="texture">The <see cref="Bitmap"/> that makes up the surface of the <see cref="Custom"/> mesh.</param>
        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string file_path, Bitmap texture) : base(origin, direction_forward, direction_up)
        {
            Has_Texture = true;

            List<Vertex> vertices = new List<Vertex>();
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
                    float x, y, z, u, v, w;

                    switch (data[0])
                    {
                        case "#":
                            // Comment; ignore line
                            break;
                        case "v":
                            // Vertex
                            x = float.Parse(data[1]);
                            y = float.Parse(data[2]);
                            z = float.Parse(data[3]);
                            w = (data.Length == 5) ? float.Parse(data[4]) : 1;
                            vertices.Add(new Vertex(new Vector4D(x, y, z, w)));
                            break;
                        case "vt":
                            // Texture vertex
                            u = float.Parse(data[1]);
                            v = (data.Length > 2) ? float.Parse(data[2]) : 0;
                            w = (data.Length == 4) ? float.Parse(data[3]) : 0;
                            texture_vertices.Add(new Vector3D(u, v, w));
                            break;
                        case "l":
                            // Line (or polyline)
                            int no_end_points = data.Length - 1;
                            do
                            {
                                p1 = int.Parse(data[no_end_points]) - 1;
                                p2 = int.Parse(data[no_end_points - 1]) - 1;
                                edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1]));
                                no_end_points--;
                            }
                            while (no_end_points > 1);
                            break;
                        case "f":
                            // Face
                            p1 = int.Parse(data[1]);
                            p2 = int.Parse(data[2]);
                            p3 = int.Parse(data[3]);
                            faces.Add(new Face(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1]));
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                Trace.WriteLine($"Error generating Custom mesh: {error.Message}");
                return;
            }

            Vertices = vertices.ToArray();
            Edges = edges.ToArray();
            Faces = faces.ToArray();
            Textures = new Texture[] { new Texture(texture, texture_vertices.ToArray()) };
        }

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh from joining two other meshes.
        /// </summary>
        /// <param name="origin">The position of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="direction_forward">The direction the resultant <see cref="Custom"/> mesh faces.</param>
        /// <param name="direction_up">The upward orientation of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="m1">The first <see cref="Mesh"/> to be joined.</param>
        /// <param name="m2">The second <see cref="Mesh"/> to be joined.</param>
        public Custom(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, Mesh m1, Mesh m2) : base(origin, direction_forward, direction_up)
        {
            Vertices = m1.Vertices.Concat(m2.Vertices).ToArray(); // Not entirely sure how this works?
            Edges = m1.Edges.Concat(m2.Edges).ToArray();
            Faces = m1.Faces.Concat(m2.Faces).ToArray();
            Textures = m1.Textures.Concat(m2.Textures).ToArray();
        }

        #endregion
    }
}