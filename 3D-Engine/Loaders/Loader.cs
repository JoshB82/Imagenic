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

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.IO;

namespace Imagenic.Core.Loaders;

public abstract class Loader : IDisposable
{
    #region Fields and Properties

    public int NumberToSkip { get; set; }
    public int? NumberToTake { get; set; }

    public bool SkipMalformedData { get; set; }

    internal FileType fileType;
    internal FileStream fileStream;
    internal BinaryReader binaryReader;
    internal BinaryWriter binaryWriter;

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

    #endregion
}