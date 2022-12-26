using Imagenic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Writers;

public sealed class OBJWriter : MeshWriter
{
    #region Constructors

    public OBJWriter(string filePath) : base(filePath)
    {

    }

    #endregion

    #region Methods

    public async override Task<bool> WriteAsync(Mesh mesh, CancellationToken ct)
    {

    }

    #endregion
}