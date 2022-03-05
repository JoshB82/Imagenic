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
using System.Collections.Generic;
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

    private IEnumerable<string> filePaths;
    public IEnumerable<string> FilePaths
    {
        get => filePaths;
        set
        {
            switch (fileType)
            {
                case FileType.Binary:

                    break;
                case FileType.Text:
                    break;
            }

            filePaths = value;
        }
    }

    #endregion

    #region Constructors

    protected Loader(IEnumerable<string> filePaths, FileType fileType)
    {
        foreach (string filePath in filePaths)
        {
            ExceptionHelper.ThrowIfFileNotFound(filePath);
        }
        this.fileType = fileType;
        FilePaths = filePaths;
    }

    #endregion

    #region Methods

    #endregion
}