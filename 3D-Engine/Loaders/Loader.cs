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

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Utilities;
using System.IO;
using System.Threading.Tasks;

namespace _3D_Engine.Loaders;

public abstract class Loader
{
    #region Fields and Properties

    public string FilePath { get; set; }

    #endregion

    #region Constructors

    public Loader(string filePath)
    {
        ExceptionHelper.ThrowIfFileNotFound(filePath);
        FilePath = filePath;
    }

    #endregion

    #region Methods

    public abstract Task<MeshStructure> Parse();

    #endregion
}