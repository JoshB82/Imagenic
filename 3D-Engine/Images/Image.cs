using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine.Images;

public abstract class Image
{
    #region Fields and Properties

    public int Width { get; set; }
    public int Height { get; set; }

    #endregion

    #region Constructors

    public Image(int width, int height)
    {
        Width = width;
        Height = height;
    }

    #endregion

    #region Methods

    public bool Export(string filePath)
    {

    }

    #endregion
}