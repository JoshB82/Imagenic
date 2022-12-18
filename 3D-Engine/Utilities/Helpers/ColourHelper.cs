using System.Drawing;

namespace Imagenic.Core.Utilities.Helpers;

internal static class ColourHelper
{
    public static Color MixColours(params Color[] colours)
    {
        Color colourMix = colours[0];
        for (int i = 1; i < colours.Length; i++)
        {
            colourMix += colours[i];
        }
        return colourMix;
    }
}