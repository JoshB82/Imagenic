/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines static methods for loading and processing data from .OBJ files.
 */

using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace _3D_Engine.Loaders
{
    public class OBJLoader : Loader
    {
        #region Fields and Properties

        public string[] Lines { get; set; }

        #endregion

        #region Constructors

        public OBJLoader(string filePath) : base(filePath)
        {
            try
            {
                Lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                // Throw exception
            }

            Task.Run(() => Parse());
        }

        #endregion

        public async Task<IList<Vertex>> GetVertices()
        {
            List<Vertex> vertices = new();
            float x, y, z, w;

            await Task.Run(() =>
            {
                foreach (string line in Lines)
                {
                    string[] data = line.Split();
                    if (data[0] == "v")
                    {
                        x = float.Parse(data[1]);
                        y = float.Parse(data[2]);
                        z = float.Parse(data[3]);
                        w = (data.Length == 5) ? float.Parse(data[4]) : 1;
                        vertices.Add(new Vertex(new Vector4D(x, y, z, w)));
                    }
                }
            });

            return vertices;
        }

        public IList<Face> GetFaces()
        {
            List<Face> faces = new();

        }

        public Custom GenerateCustomMesh()
        {

        }

        #region Methods

        public async override Task<MeshContent> Parse()
        {
            MeshContent meshContent = null;

            await Task.Run(() =>
            {
                List<Vertex> vertices = new();
                List<Edge> edges = new();
                List<Triangle> triangles = new();
                List<Face> faces = new();

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
                            int noEndPoints = data.Length - 1;
                            do
                            {
                                p1 = int.Parse(data[noEndPoints]) - 1;
                                p2 = int.Parse(data[noEndPoints - 1]) - 1;
                                edges.Add(new Edge(vertices[p1 - 1], vertices[p2 - 1]));
                                noEndPoints--;
                            }
                            while (noEndPoints > 1);
                            break;
                        case "f":
                            // Face
                            p1 = int.Parse(data[1]);
                            p2 = int.Parse(data[2]);
                            p3 = int.Parse(data[3]);
                            triangles.Add(new SolidTriangle(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1]));
                            break;
                    }
                }

                meshContent = new MeshContent
                {
                    Vertices = vertices,
                    Edges = edges,
                    Faces = faces
                };
            });

            return meshContent;
        }

        #endregion
    }
}