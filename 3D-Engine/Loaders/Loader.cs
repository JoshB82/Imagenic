/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a base class for loaders.
 */

using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders;

public abstract class Loader
{
    #region Fields and Properties

    public bool SkipMalformedData { get; set; }

    internal FileType fileType;

    private string filePath;
    public string FilePath
    {
        get => filePath;
        set
        {
            switch (fileType)
            {
                case FileType.Binary:

                    break;
            }

            filePath = value;
        }
    }

    #endregion

    #region Constructors

    public Loader(string filePath, FileType fileType)
    {
        ExceptionHelper.ThrowIfFileNotFound(filePath);
        this.fileType = fileType;
        FilePath = filePath;
    }

    #endregion

    #region Methods

    public abstract Task<IList<Vertex>> GetVerticesAsync(CancellationToken ct);
    public abstract Task<IList<Edge>> GetEdgesAsync(CancellationToken ct);
    public abstract Task<IList<Face>> GetFacesAsync(CancellationToken ct);
    public abstract Task<MeshStructure> ParseAsync(CancellationToken ct);

    #endregion
}