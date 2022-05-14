using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Readers;

public abstract class ImagenicReader
{
    public StreamReader Reader { get; set; }
}