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

            byte newA = RoundToByte(colour.A * fraction);
            byte newR = RoundToByte(colour.R * fraction);
            byte newG = RoundToByte(colour.G * fraction);
            byte newB = RoundToByte(colour.B * fraction);

            newA = newA > 255 ? (byte)255 : newA;
            newR = newR > 255 ? (byte)255 : newR;
            newG = newG > 255 ? (byte)255 : newG;
            newB = newB > 255 ? (byte)255 : newB;

            return Color.FromArgb(newA, newR, newG, newB);
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
        public static bool ApproxMoreThan(this float v1, float v2, float epsilon = float.Epsilon) => v1 > v2 - epsilon;

        // Rounding
        internal static byte RoundToByte(this float num) => (byte)(num >= 0 ? num + 0.5f : num - 0.5f);
        internal static int RoundToInt(this float num) => (int)(num >= 0 ? num + 0.5f : num - 0.5f);

        internal static float TruncateToRange(this float num, float lowest, float highest)
        {
            if (num < lowest) return lowest;
            if (num > highest) return highest;
            return num;
        }
    }
}