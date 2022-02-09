using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Meshes
{
    public class _3DSLoader : Loader
    {
        public async override Task<IList<Edge>> GetEdgesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async override Task<IList<Face>> GetFacesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async override Task<IList<Vertex>> GetVerticesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async override Task<MeshStructure> ParseAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}