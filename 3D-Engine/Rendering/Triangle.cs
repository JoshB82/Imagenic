using System;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        // Swap
        private static void Swap<T>(ref T x1, ref T x2)
        {
            T temp = x1;
            x1 = x2;
            x2 = temp;
        }

        // Sorting
        private static void Sort_By_Y(
            ref int x1, ref int y1, ref float z1,
            ref int x2, ref int y2, ref float z2,
            ref int x3, ref int y3, ref float z3)
        {
            // y1 highest; y3 lowest
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

        private static void Textured_Sort_By_Y(
            ref int x1, ref int y1, ref float tx1, ref float ty1, ref float tz1,
            ref int x2, ref int y2, ref float tx2, ref float ty2, ref float tz2,
            ref int x3, ref int y3, ref float tx3, ref float ty3, ref float tz3)
        {
            // y1 lowest; y3 highest
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

        // Interpolation (source!)
        private static void Interpolate_Triangle(object @object, Action<object, int, int, float> action,
            int x1, int y1, float z1,
            int x2, int y2, float z2,
            int x3, int y3, float z3)
        {
            // Create steps
            float dy_step_1 = y1 - y2;
            float dy_step_2 = y1 - y3;
            float dy_step_3 = y2 - y3;

            float x_step_1 = 0, z_step_1 = 0;
            float x_step_3 = 0, z_step_3 = 0;

            if (dy_step_1 != 0)
            {
                x_step_1 = (x1 - x2) / dy_step_1; // dx from point 1 to point 2
                z_step_1 = (z1 - z2) / dy_step_1; // dz from point 1 to point 2
            }
            float x_step_2 = (x1 - x3) / dy_step_2; // dx from point 1 to point 3
            float z_step_2 = (z1 - z3) / dy_step_2; // dz from point 1 to point 3
            if (dy_step_3 != 0)
            {
                x_step_3 = (x2 - x3) / dy_step_3; // dx from point 2 to point 3
                z_step_3 = (z2 - z3) / dy_step_3; // dz from point 2 to point 3
            }

            // Draw a flat-bottom triangle
            if (dy_step_1 != 0)
            {
                for (int y = y2; y <= y1; y++)
                {
                    int sx = ((y - y2) * x_step_1 + x2).Round_to_Int();
                    float sz = (y - y2) * z_step_1 + z2;

                    int ex = ((y - y3) * x_step_2 + x3).Round_to_Int();
                    float ez = (y - y3) * z_step_2 + z3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref sz, ref ez);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);

                        t += t_step;
                    }
                }
            }

            // Draw a flat-top triangle
            if (dy_step_3 != 0)
            {
                for (int y = y3; y <= y2; y++)
                {
                    int sx = ((y - y3) * x_step_3 + x3).Round_to_Int();
                    float sz = (y - y3) * z_step_3 + z3;

                    int ex = ((y - y3) * x_step_2 + x3).Round_to_Int();
                    float ez = (y - y3) * z_step_2 + z3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref sz, ref ez);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);

                        t += t_step;
                    }
                }
            }
        }

        // floats or ints for everything? Should it take an average of texels (or pixels in method above)?
        private void Textured_Triangle(Bitmap texture,
            int x1, int y1, float tx1, float ty1, float tz1,
            int x2, int y2, float tx2, float ty2, float tz2,
            int x3, int y3, float tx3, float ty3, float tz3)
        {
            // Create steps
            float dy_step_1 = y1 - y2;
            float dy_step_2 = y1 - y3;
            float dy_step_3 = y2 - y3;

            float x_step_1 = 0, tx_step_1 = 0, ty_step_1 = 0, tz_step_1 = 0;
            float x_step_3 = 0, tx_step_3 = 0, ty_step_3 = 0, tz_step_3 = 0;

            if (dy_step_1 != 0)
            {
                x_step_1 = (x1 - x2) / dy_step_1; // dx from point 1 to point 2
                tx_step_1 = (tx1 - tx2) / dy_step_1; // dtx from point 1 to point 2
                ty_step_1 = (ty1 - ty2) / dy_step_1; // dty from point 1 to point 2
                tz_step_1 = (tz1 - tz2) / dy_step_1; // dtz from point 1 to point 2
            }
            float x_step_2 = (x1 - x3) / dy_step_2; // dx from point 1 to point 3
            float tx_step_2 = (tx1 - tx3) / dy_step_2; // dtx from point 1 to point 3
            float ty_step_2 = (ty1 - ty3) / dy_step_2; // dty from point 1 to point 3
            float tz_step_2 = (tz1 - tz3) / dy_step_2; // dtz from point 1 to point 3
            if (dy_step_3 != 0)
            {
                x_step_3 = (x2 - x3) / dy_step_3; // dx from point 2 to point 3
                tx_step_3 = (tx2 - tx3) / dy_step_3; // dtx from point 2 to point 3
                ty_step_3 = (ty2 - ty3) / dy_step_3; // dty from point 2 to point 3
                tz_step_3 = (tz2 - tz3) / dy_step_3; // dtz from point 2 to point 3
            }

            // Draw a flat-bottom triangle
            if (dy_step_1 != 0)
            {
                for (int y = y2; y <= y1; y++)
                {
                    int sx = ((y - y2) * x_step_1 + x2).Round_to_Int();
                    int ex = ((y - y3) * x_step_2 + x3).Round_to_Int();

                    float stx = (y - y2) * tx_step_1 + tx2;
                    float sty = (y - y2) * ty_step_1 + ty2;
                    float stz = (y - y2) * tz_step_1 + tz2;

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;
                    float etz = (y - y3) * tz_step_2 + tz3;

                    // ?
                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref stx, ref etx);
                        Swap(ref sty, ref ety);
                        Swap(ref stz, ref etz);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        int tx = (stx + t * (etx - stx)).Round_to_Int();
                        int ty = (sty + t * (ety - sty)).Round_to_Int();
                        float tz = stz + t * (etz - stz);

                        Textured_Check_Against_Z_Buffer(texture, x, y, tz, tx, ty);

                        t += t_step;
                    }
                }
            }

            // Draw a flat-top triangle
            if (dy_step_3 != 0)
            {
                for (int y = y3; y <= y2; y++)
                {
                    int sx = ((y - y3) * x_step_3 + x3).Round_to_Int();
                    int ex = ((y - y3) * x_step_2 + x3).Round_to_Int();

                    float stx = (y - y3) * tx_step_3 + tx3;
                    float sty = (y - y3) * ty_step_3 + ty3;
                    float stz = (y - y3) * tz_step_3 + tz3;

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;
                    float etz = (y - y3) * tz_step_2 + tz3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref stx, ref etx);
                        Swap(ref sty, ref ety);
                        Swap(ref stz, ref etz);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        int tx = (stx + t * (etx - stx)).Round_to_Int();
                        int ty = (sty + t * (ety - sty)).Round_to_Int();
                        float tz = stz + t * (etz - stz);

                        Textured_Check_Against_Z_Buffer(texture, x, y, tz, tx, ty);

                        t += t_step;
                    }
                }
            }
        }
    }
}