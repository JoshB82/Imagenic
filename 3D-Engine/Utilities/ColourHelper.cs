using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine.Utilities;

internal static class ColourHelper
{
    public static Color MixColours(Color[] colours)
    {
        Color colourMix = colours[0];
        for (int i = 1; i < colours.Length; i++)
        {
            colourMix += colours[i];
        } 
        return colourMix;
    }
}