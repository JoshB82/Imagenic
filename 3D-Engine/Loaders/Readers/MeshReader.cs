using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Imagenic.Core.Loaders.Readers;

public abstract class MeshReader : IDisposable
{
    #region Fields and Properties

    protected StreamReader Reader { get; }

    #endregion

    #region Constructors

    public MeshReader(string filePath)
    {
        Reader = new StreamReader(filePath);
    }

    #endregion

    #region Methods

    public void Dispose() => Reader.Dispose(); // ?

    public abstract Task<IList<Vertex>> GetVerticesAsync(CancellationToken ct);
    public abstract Task<IList<Edge>> GetEdgesAsync(CancellationToken ct);
    public abstract Task<IList<Triangle>> GetTrianglesAsync(CancellationToken ct);
    public abstract Task<IList<Face>> GetFacesAsync(CancellationToken ct);
    public abstract Task<MeshStructure> ParseAsync(CancellationToken ct);

    #endregion
}