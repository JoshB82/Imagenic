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

public abstract class MeshLoader : Loader
{
    #region Constuctors

    protected MeshLoader(IEnumerable<string> filePaths, FileType fileType) : base(filePaths, fileType)
    {

    }

    #endregion

    #region Methods

    public abstract Task<IList<Vertex>> GetVerticesAsync(CancellationToken ct);
    public abstract Task<IList<Edge>> GetEdgesAsync(CancellationToken ct);
    public abstract Task<IList<Face>> GetFacesAsync(CancellationToken ct);
    public abstract Task<MeshStructure> ParseAsync(CancellationToken ct);

    #endregion
}