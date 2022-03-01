using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Loaders.Meshes;

public abstract class MeshLoader : Loader
{
    public MeshLoader(string filePath) : base(filePath)
    {

    }
}