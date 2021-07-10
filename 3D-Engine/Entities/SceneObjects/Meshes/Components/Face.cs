using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components
{
    public class Face
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }
        public IList<Triangle> Triangles { get; set; }

        public Face(IList<Vertex> vertices)
        {
            Vertices = vertices;

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Edges.Add(new Edge(vertices[i], vertices[i + 1]));
            }
            Edges.Add(new Edge(vertices[vertices.Count - 1], vertices[0]));

            for (int i = 0; i < vertices.Count; i++)
            {
                Triangles.Add(new SolidFace())
            }
        }

        public Face(IList<Edge> edges)
        {
            Edges = edges;
        }

        public Face(IList<Triangle> triangles)
        {
            Triangles = triangles;
        }

        public Mesh Extrude(Face face, Vector3D displacement)
        {

        }

        public Face Join(Face face1, Face face2)
        {

        }

        internal bool DoesOverlap(Triangle t1, Triangle t2)
        {

        }
    }
}
