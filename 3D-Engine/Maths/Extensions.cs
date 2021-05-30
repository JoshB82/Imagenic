using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using System;
using System.Drawing;

namespace _3D_Engine
{
    public static class Extensions
    {
        // Color extensions
        public static Color Brighten(this Color colour, float fraction)
        {
            fraction++;

            byte new_a = RoundToByte(colour.A * fraction);
            byte new_r = RoundToByte(colour.R * fraction);
            byte new_g = RoundToByte(colour.G * fraction);
            byte new_b = RoundToByte(colour.B * fraction);

            new_a = new_a > 255 ? (byte)255 : new_a;
            new_r = new_r > 255 ? (byte)255 : new_r;
            new_g = new_g > 255 ? (byte)255 : new_g;
            new_b = new_b > 255 ? (byte)255 : new_b;

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }

        public static Color BrightenPercentage(this Color colour, float percentage) => Brighten(colour, percentage / 100);
        public static Color Darken(this Color colour, float fraction) => Brighten(colour, fraction - 1);
        public static Color DarkenPercentage(this Color colour, float percentage) => Darken(colour, percentage / 100);

        public static Color Mix(this Color colour, Color mixingColour)
        {
            byte new_a = RoundToByte(0.5f * (colour.A + mixingColour.A));
            byte new_r = RoundToByte(0.5f * (colour.R + mixingColour.R));
            byte new_g = RoundToByte(0.5f * (colour.G + mixingColour.G));
            byte new_b = RoundToByte(0.5f * (colour.B + mixingColour.B));

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }

        // Float extensions
        // Equality
        //source
        public static bool ApproxEquals(this float v1, float v2, float epsilon = float.Epsilon) => Math.Abs(v1 - v2) <= epsilon;
        public static bool ApproxLessThanEquals(this float v1, float v2, float epsilon = float.Epsilon) => v1 <= v2 + epsilon;
        public static bool ApproxMoreThanEquals(this float v1, float v2, float epsilon = float.Epsilon) => v1 >= v2 - epsilon;
        public static bool ApproxLessThan(this float v1, float v2, float epsilon = float.Epsilon) => v1 < v2 + epsilon;
        public static bool Approx_More_Than(this float v1, float v2, float epsilon = float.Epsilon) => v1 > v2 - epsilon;

        // Rounding
        internal static byte RoundToByte(this float num) => (byte)(num >= 0 ? num + 0.5f : num - 0.5f);
        internal static int RoundToInt(this float num) => (int)(num >= 0 ? num + 0.5f : num - 0.5f);

        internal static float TruncateToRange(this float num, float lowest, float highest)
        {
            if (num < lowest) return lowest;
            if (num > highest) return highest;
            return num;
        }

        public static T ColourAllSolidFaces<T>(this T input, Color colour) where T : Mesh
        {
            foreach (Face face in input.Faces)
            {
                if (face is SolidFace solidFace)
                {
                    solidFace.Colour = colour;
                }
            }

            return input;
        }
    }
}