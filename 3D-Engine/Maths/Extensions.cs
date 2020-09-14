using System.Drawing;

namespace _3D_Engine
{
    public static class Extensions
    {
        // Float extensions
        internal static byte Round_to_Byte(this float num) => (byte)((num >= 0) ? (num + 0.5) : (num - 0.5));
        internal static int Round_to_Int(this float num) => (int)((num >= 0) ? (num + 0.5) : (num - 0.5));

        // Color extensions
        public static Color Brighten(this Color colour, float fraction)
        {
            fraction++;

            byte new_a = Round_to_Byte(colour.A * fraction);
            byte new_r = Round_to_Byte(colour.R * fraction);
            byte new_g = Round_to_Byte(colour.G * fraction);
            byte new_b = Round_to_Byte(colour.B * fraction);

            new_a = (new_a > 255) ? (byte)255 : new_a;
            new_r = (new_r > 255) ? (byte)255 : new_r;
            new_g = (new_g > 255) ? (byte)255 : new_g;
            new_b = (new_b > 255) ? (byte)255 : new_b;

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }

        public static Color Brighten_Percentage(this Color colour, float percentage) => Brighten(colour, percentage / 100);
        public static Color Darken(this Color colour, float fraction) => Brighten(colour, fraction - 1);
        public static Color Darken_Percentage(this Color colour, float percentage) => Darken(colour, percentage / 100);

        public static Color Mix(this Color colour, Color mixing_colour)
        {
            byte new_a = Round_to_Byte(0.5f * (colour.A + mixing_colour.A));
            byte new_r = Round_to_Byte(0.5f * (colour.R + mixing_colour.R));
            byte new_g = Round_to_Byte(0.5f * (colour.G + mixing_colour.G));
            byte new_b = Round_to_Byte(0.5f * (colour.B + mixing_colour.B));

            return Color.FromArgb(new_a, new_r, new_g, new_b);
        }
    }
}