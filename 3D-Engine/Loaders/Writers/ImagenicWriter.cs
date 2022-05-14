using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Writers;

public abstract class ImagenicWriter
{
    public StreamWriter Writer { get; set; }
}