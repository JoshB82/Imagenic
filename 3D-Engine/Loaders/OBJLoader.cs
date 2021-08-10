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
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace _3D_Engine.Loaders
{
    public class OBJLoader : Loader
    {
        public string[] Lines { get; set; }

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
        }

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
    }
}