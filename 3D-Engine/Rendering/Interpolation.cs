using System;

namespace _3D_Engine.Rendering
{
    internal static class Interpolation
    {
        // Interpolation (source!)
        internal static void InterpolateTriangle(
            Action<object, int, int, float> action,
            object @object,
            int x1, int y1, float z1,
            int x2, int y2, float z2,
            int x3, int y3, float z3)
        {
            // Create steps
            float dyStep1 = y1 - y2;
            float dyStep2 = y1 - y3;
            float dyStep3 = y2 - y3;

            float xStep1 = 0, zStep1 = 0;
            float xStep3 = 0, zStep3 = 0;

            if (dyStep1 != 0)
            {
                xStep1 = (x1 - x2) / dyStep1; // dx from point 1 to point 2
                zStep1 = (z1 - z2) / dyStep1; // dz from point 1 to point 2
            }
            float xStep2 = (x1 - x3) / dyStep2; // dx from point 1 to point 3
            float zStep2 = (z1 - z3) / dyStep2; // dz from point 1 to point 3
            if (dyStep3 != 0)
            {
                xStep3 = (x2 - x3) / dyStep3; // dx from point 2 to point 3
                zStep3 = (z2 - z3) / dyStep3; // dz from point 2 to point 3
            }

            // Draw a flat-bottom triangle
            if (dyStep1 != 0)
            {
                for (int y = y2; y <= y1; y++)
                {
                    int sx = ((y - y2) * xStep1 + x2).RoundToInt();
                    float sz = (y - y2) * zStep1 + z2;

                    int ex = ((y - y3) * xStep2 + x3).RoundToInt();
                    float ez = (y - y3) * zStep2 + z3;

                    if (sx > ex)
                    {
                        NumericManipulation.Swap(ref sx, ref ex);
                        NumericManipulation.Swap(ref sz, ref ez);
                    }

                    float t = 0, tStep = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);

                        t += tStep;
                    }
                }
            }

            // Draw a flat-top triangle
            if (dyStep3 != 0)
            {
                for (int y = y3; y <= y2; y++)
                {
                    int sx = ((y - y3) * xStep3 + x3).RoundToInt();
                    float sz = (y - y3) * zStep3 + z3;

                    int ex = ((y - y3) * xStep2 + x3).RoundToInt();
                    float ez = (y - y3) * zStep2 + z3;

                    if (sx > ex)
                    {
                        NumericManipulation.Swap(ref sx, ref ex);
                        NumericManipulation.Swap(ref sz, ref ez);
                    }

                    float t = 0, tStep = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);

                        t += tStep;
                    }
                }
            }
        }
    }
}