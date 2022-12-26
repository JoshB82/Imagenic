using Imagenic.Core.Entities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Writers;

public abstract class MeshWriter : IDisposable
{
    #region Fields and Properties

    public StreamWriter Writer { get; }

    #endregion

    #region Constructors

    public MeshWriter(string filePath)
	{
        Writer = new StreamWriter(filePath);
	}

    #endregion

    #region Methods

    public void Dispose() => Writer.Dispose(); // ?

    public abstract Task<bool> WriteAsync(Mesh mesh, CancellationToken ct);

    #endregion
}