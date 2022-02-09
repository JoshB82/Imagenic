using Imagenic.Core.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine.Images;

public class Bitmap : Image
{
    public Bitmap(int width, int height) : base(width, height)
    {

    }

    public Bitmap(Buffer2D<Color> buffer)
    {

    }
}