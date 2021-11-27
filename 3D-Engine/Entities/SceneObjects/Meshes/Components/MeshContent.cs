using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components
{
    public class MeshContent
    {
        #region Fields and Properties

        private IList<Vertex> vertices;
        public IList<Vertex> Vertices
        {
            get => vertices;
            set
            {
                vertices = value ?? throw new ParameterCannotBeNullException();
            }
        }
        public IList<Edge> Edges { get; set; }
        public IList<Face> Faces { get; set; }

        public IEnumerable<Texture> Textures { get; set; }

        #endregion

        #region Constructors

        public MeshContent(IList<Vertex> vertices, IList<Edge> edges = null, IList<Face> faces = null)
        {
            Vertices = vertices;
            Edges = edges;
            Faces = faces;
        }

        #endregion
    }
}