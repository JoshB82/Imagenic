using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;

namespace Imagenic.Core.Entities.SceneObjects.Meshes
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='TImage:_3D_Engine.Custom']/*"/>
    public sealed class Custom : Mesh
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public Custom(Vector3D worldOrigin, Orientation worldOrientation, MeshStructure meshStructure) : base(worldOrigin, worldOrientation, meshStructure)
        {

        }

        ///<summary>
            ///Creates a<see cref= "TImage:_3D_Engine.Custom" /> mesh.
            ///</ summary >
            ///< param name= "origin" > The position of the<see cref="TImage:_3D_Engine.Custom" /> mesh.</param>
            ///<param name = "direction_forward" > The direction the <see cref = "TImage:_3D_Engine.Custom" /> mesh faces.</param>
            ///<param name = "direction_up" > The upward orientation of the <see cref = "TImage:_3D_Engine.Custom" /> mesh.</ param >
            ///< param name= "vertices" > The vertices that make up the<see cref="TImage:_3D_Engine.Custom" /> mesh.</param><param name = "edges" > The < see cref= "TImage:_3D_Engine.Edge" /> s that make up the <see cref = "TImage:_3D_Engine.Custom" /> mesh.</ param >
            ///< param name= "faces" > The < see cref= "TImage:_3D_Engine.Face" /> s that make up the <see cref = "TImage:_3D_Engine.Custom" /> mesh.</ param >

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Custom.#ctor(_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vector3D,_3D_Engine.Vertex[],_3D_Engine.Edge[],_3D_Engine.Face[],_3D_Engine.Texture[])']/*"/>
    public Custom(Vector3D origin,
    Vector3D directionForward,
    Vector3D directionUp,
    Vertex[] vertices,
    Edge[] edges,
    Triangle[] faces,
    Texture[] textures) : base(origin, directionForward, directionUp)
    {
    Vertices = vertices;
    Edges = edges;
    Triangles = faces;
    HasTexture = true;
    Textures = textures;
    }

    /// <summary>
    /// Creates a <see cref="Custom"/> mesh from an OBJ file.
    /// </summary>
    /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
    /// <param name="directionForward">The direction the <see cref="Custom"/> mesh faces.</param>
    /// <param name="directionUp">The upward orientation of the <see cref="Custom"/> mesh.</param>
    /// <param name="filePath">The path of the OBJ file.</param>
    public Custom(Vector3D origin,
    Vector3D directionForward,
    Vector3D directionUp,
    string filePath) : base(origin, directionForward, directionUp)
    {
    // Check if the file exists
    if (!File.Exists(filePath))
    {
    Trace.WriteLine($"Error generating Custom mesh: The file {filePath} was not found.");
    return;
    }

    // Obtain data from file
    string[] lines;
    try
    {
    lines = File.ReadAllLines(filePath);
    }
    catch (Exception error)
    {
    Trace.WriteLine($"Error generating Custom mesh: {error.Message}");
    return;
    }

    // Create the mesh
    GenerateCustomFromOBJ(lines);
    }
    public Custom(Vector3D origin,
                    Vector3D directionForward,
                    Vector3D directionUp,
                      string[] lines) : base(origin, directionForward, directionUp) => GenerateCustomFromOBJ(lines);

        private void GenerateCustomFromOBJ(string[] lines)
        {

        }
        //b
        /// <summary>
        /// Creates a textured <see cref="Custom"/> mesh from an OBJ file.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Custom"/> mesh.</param>
        /// <param name="directionForward">The direction the <see cref="Custom"/> mesh faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Custom"/> mesh.</param>
        /// <param name="filePath">The path of the OBJ file.</param>
        /// <param name="texture">The <see cref="Bitmap"/> that makes up the surface of the <see cref="Custom"/> mesh.</param>
        public Custom(Vector3D origin,
                      Vector3D directionForward,
                      Vector3D directionUp,
                      string filePath,
                      Bitmap texture) : base(origin, directionForward, directionUp)
        {
            HasTexture = true;

            List<Vertex> vertices = new();
            List<Vector3D> textureVertices = new();
            List<Edge> edges = new();
            List<Triangle> faces = new();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
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
                            w = data.Length == 5 ? float.Parse(data[4]) : 1;
                            vertices.Add(new Vertex(new Vector4D(x, y, z, w)));
                            break;
                        case "vt":
                            // Texture vertex
                            u = float.Parse(data[1]);
                            v = data.Length > 2 ? float.Parse(data[2]) : 0;
                            w = data.Length == 4 ? float.Parse(data[3]) : 0;
                            textureVertices.Add(new Vector3D(u, v, w));
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
                            faces.Add(new SolidTriangle(vertices[p1 - 1], vertices[p2 - 1], vertices[p3 - 1])); // TODO: fix
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
            Triangles = faces.ToArray();
            Textures = new Texture[] { new Texture(texture, textureVertices.ToArray()) };
        }

        /// <summary>
        /// Creates a <see cref="Custom"/> mesh from joining two other meshes.
        /// </summary>
        /// <param name="origin">The position of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="directionForward">The direction the resultant <see cref="Custom"/> mesh faces.</param>
        /// <param name="directionUp">The upward orientation of the resultant <see cref="Custom"/> mesh.</param>
        /// <param name="m1">The first <see cref="Mesh"/> to be joined.</param>
        /// <param name="m2">The second <see cref="Mesh"/> to be joined.</param>
        public Custom(Vector3D origin,
                      Vector3D directionForward,
                      Vector3D directionUp,
                      Mesh m1,
                      Mesh m2) : base(origin, directionForward, directionUp)
        {
            Vertices = m1.Vertices.Concat(m2.Vertices).ToArray(); // Not entirely sure how this works?
            Edges = m1.Edges.Concat(m2.Edges).ToArray();
            Triangles = m1.Triangles.Concat(m2.Triangles).ToArray();
            Textures = m1.Textures.Concat(m2.Textures).ToArray();
        }

        #endregion

        #region Methods

        protected override IList<Vertex> GenerateVertices()
        {

        }

        protected override IList<Edge> GenerateEdges()
        {

        }

        protected override IList<Face> GenerateFaces()
        {

        }

        #endregion
    }
}