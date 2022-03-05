using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Meshes;

public class _3DSLoader : MeshLoader
{
    #region Constructors

    public _3DSLoader(IEnumerable<string> filePaths) : base(filePaths, FileType.Binary)
    {

    }

    public _3DSLoader(params string[] filePaths) : this((IEnumerable<string>)filePaths) { }

    #endregion

    #region Methods

    #endregion
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