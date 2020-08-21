using System;
using System.Drawing;

namespace _3D_Engine
{
    public static class Extensions
    {
        public static Color Brighten(this Color colour, double percentage) // ?
        {
            byte new_a = (byte)Math.Round(colour.A * 1 + (percentage / 100)); //??
            byte new_r = (byte)Math.Round(colour.R * 1 + (percentage / 100));
            byte new_g = (byte)Math.Round(colour.G * 1 + (percentage / 100));
            byte new_b = (byte)Math.Round(colour.B * 1 + (percentage / 100));

            new_a = (new_a > 255) ? (byte)255 : new_a;
            new_r = (new_r > 255) ? (byte)255 : new_r;
            new_g = (new_g > 255) ? (byte)255 : new_g;
            new_b = (new_b > 255) ? (byte)255 : new_b;

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }

        public static Color Darken(this Color colour, double percentage) // ? (use above?)
        {
            byte new_a = (byte)Math.Round(colour.A * 1 - (percentage / 100)); //??
            byte new_r = (byte)Math.Round(colour.R * 1 - (percentage / 100));
            byte new_g = (byte)Math.Round(colour.G * 1 - (percentage / 100));
            byte new_b = (byte)Math.Round(colour.B * 1 - (percentage / 100));

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }

        public static Color Mix(this Color colour, Color other_colour)
        {
            byte new_a = (byte)Math.Round(0.5 * (colour.A + other_colour.A)); //??
            byte new_r = (byte)Math.Round(0.5 * (colour.R + other_colour.R));
            byte new_g = (byte)Math.Round(0.5 * (colour.G + other_colour.G));
            byte new_b = (byte)Math.Round(0.5 * (colour.B + other_colour.B));

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }
    }
}