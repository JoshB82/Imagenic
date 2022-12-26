using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Enums;
using System.Collections.Generic;
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

    

    #endregion
}