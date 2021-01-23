namespace _3D_Engine.Rendering
{
    internal static class NumericManipulation
    {
        // Swap
        internal static void Swap<T>(ref T x1, ref T x2)
        {
            T temp = x1;
            x1 = x2;
            x2 = temp;
        }

        // Sorting
        internal static void SortByY(
            ref int x1, ref int y1, ref float z1,
            ref int x2, ref int y2, ref float z2,
            ref int x3, ref int y3, ref float z3)
        {
            // y1 highest; y3 lowest (?)
            if (y1 < y2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
                Swap(ref z1, ref z2);
            }
            if (y1 < y3)
            {
                Swap(ref x1, ref x3);
                Swap(ref y1, ref y3);
                Swap(ref z1, ref z3);
            }
            if (y2 < y3)
            {
                Swap(ref x2, ref x3);
                Swap(ref y2, ref y3);
                Swap(ref z2, ref z3);
            }
        }

        internal static void TexturedSortByY(
            ref int x1, ref int y1, ref float tx1, ref float ty1, ref float tz1,
            ref int x2, ref int y2, ref float tx2, ref float ty2, ref float tz2,
            ref int x3, ref int y3, ref float tx3, ref float ty3, ref float tz3)
        {
            // y1 lowest; y3 highest (?)
            if (y1 < y2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
                Swap(ref tx1, ref tx2);
                Swap(ref ty1, ref ty2);
                Swap(ref tz1, ref tz2);
            }
            if (y1 < y3)
            {
                Swap(ref x1, ref x3);
                Swap(ref y1, ref y3);
                Swap(ref tx1, ref tx3);
                Swap(ref ty1, ref ty3);
                Swap(ref tz1, ref tz3);
            }
            if (y2 < y3)
            {
                Swap(ref x2, ref x3);
                Swap(ref y2, ref y3);
                Swap(ref tx2, ref tx3);
                Swap(ref ty2, ref ty3);
                Swap(ref tz2, ref tz3);
            }
        }
    }
}