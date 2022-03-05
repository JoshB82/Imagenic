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

using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Meshes
{
    public class OBJLoader : MeshLoader
    {
        #region Fields and Properties

        public string[] Lines { get; set; }

        #endregion

        #region Constructors

        public OBJLoader(IEnumerable<string> filePaths) : base(filePaths, FileType.Text)
        {
            try
            {
                Lines = File.ReadAllLines(filePaths);
            }
            catch (Exception ex)
            {
                // Throw exception
            }
        }

        public OBJLoader(params string[] filePaths) : this((IEnumerable<string>)filePaths) { }

        #endregion

        public async override Task<IList<Vertex>> GetVerticesAsync(CancellationToken ct = default)
        {
            List<Vertex> vertices = new();

            await Task.Run(() =>
            {
                foreach (string line in Lines)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    else
                    {
                        string[] data = line.Split();
                        if (data[0] == "v")
                        {
                            var vertex = ParseVertex(data);
                            if (vertex != null)
                            {
                                vertices.Add(vertex);
                            }
                        }
                    }
                }
            }, ct);

            return vertices;
        }

        public async override Task<IList<Edge>> GetEdgesAsync(CancellationToken ct = default)
        {
            IList<Vertex> vertices = await GetVerticesAsync(ct);
            List<Edge> edges = new();

            await Task.Run(() =>
            {
                foreach (string line in Lines)
                {
                    string[] data = line.Split();

                    int noEndPoints = data.Length - 1;
                    do
                    {
                        if (ct.IsCancellationRequested)
                        {
                            //break; ??
                        }
                        else
                        {
                            edges.Add(ParseEdge(data, noEndPoints, vertices));
                            noEndPoints--;
                        }
                    }
                    while (noEndPoints > 1);
                }
            }, ct);

            return edges;
        }

        public async override Task<IList<Face>> GetFacesAsync(CancellationToken ct = default)
        {
            IList<Vertex> vertices = await GetVerticesAsync(ct);
            List<Face> faces = new();

            await Task.Run(() =>
            {
                foreach (string line in Lines)
                {
                    faces.Add(new Face(ParseTriangle(line.Split(), vertices)));
                }
            }, ct);

            return faces;
        }

        #region Methods

        public async override Task<MeshStructure> ParseAsync(CancellationToken ct = default)
        {
            MeshStructure meshStructure = null;

            await Task.Run(() =>
            {
                List<Vertex> vertices = new();
                List<Edge> edges = new();
                List<Triangle> triangles = new();
                List<Face> faces = new();

                foreach (string line in Lines)
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
                            w = data.Length == 5 ? float.Parse(data[4]) : 1;
                            vertices.Add(new Vertex(new Vector4D(x, y, z, w)));
                            break;
                        case "l":
                            // Line (or polyline)
                            int noEndPoints = data.Length - 1;
                            do
                            {
                                p1 = int.Parse(data[noEndPoints]) - 1;
                                p2 = int.Parse(data[noEndPoints - 1]) - 1;
                                edges.Add(new SolidEdge(vertices[p1 - 1], vertices[p2 - 1]));
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

                meshStructure = new MeshStructure(vertices, edges, faces);
            });

            return meshStructure;
        }

        private Vertex ParseVertex(string[] data)
        {
            try
            {
                IEnumerable<float> args = data.Skip(1).Select(x => float.Parse(x));
                return new Vertex(new Vector3D(args), data.Length == 5 ? args.Last() : 1);
            }
            catch (Exception ex)
            {
                if (SkipMalformedData)
                {
                    return null;
                }
                else
                {
                    throw new MessageBuilder<InvalidFileContentMessage>()
                        .AddParameters(data)
                        .AddType<OBJLoader>()
                        .BuildIntoException<InvalidDataException>(ex);
                }
            }
        }

        private static Edge ParseEdge(string[] data, int noEndPoints, IList<Vertex> vertices)
        {
            int p1 = int.Parse(data[noEndPoints]) - 1;
            int p2 = int.Parse(data[noEndPoints - 1]) - 1;

            return new SolidEdge(vertices[p1 - 1], vertices[p2 - 1]);
        }

        private static Triangle ParseTriangle(string[] data, IList<Vertex> vertices)
        {
            int p1 = int.Parse(data[1]);
            int p2 = int.Parse(data[2]);
            int p3 = int.Parse(data[3]);

            return new SolidTriangle(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1]);
        }

        #endregion
    }
}