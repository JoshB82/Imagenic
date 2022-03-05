using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Fonts;

public abstract class FontLoader : Loader
{
    protected FontLoader(IEnumerable<string> filePaths, FileType fileType) : base(filePaths, fileType)
    {

    }
}