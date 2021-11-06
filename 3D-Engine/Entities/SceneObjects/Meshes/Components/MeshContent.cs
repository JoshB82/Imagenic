using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components
{
    public class MeshContent
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }
        public IList<Face> Faces { get; set; }
    }
}