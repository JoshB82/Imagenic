using _3D_Engine.Rendering;
using System.Drawing;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    
    public abstract partial class Camera : RenderingObject
    {
        

        

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
                    int sx = ((y - y2) * x_step_1 + x2).RoundToInt();
                    int ex = ((y - y3) * x_step_2 + x3).RoundToInt();

                    float stx = (y - y2) * tx_step_1 + tx2;
                    float sty = (y - y2) * ty_step_1 + ty2;
                    float stz = (y - y2) * tz_step_1 + tz2;

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;
                    float etz = (y - y3) * tz_step_2 + tz3;

                    // ?
                    if (sx > ex)
                    {
                        NumericManipulation.Swap(ref sx, ref ex);
                        NumericManipulation.Swap(ref stx, ref etx);
                        NumericManipulation.Swap(ref sty, ref ety);
                        NumericManipulation.Swap(ref stz, ref etz);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        int tx = (stx + t * (etx - stx)).RoundToInt();
                        int ty = (sty + t * (ety - sty)).RoundToInt();
                        float tz = stz + t * (etz - stz);

                        TexturedCheckAgainstZBuffer(texture, x, y, tz, tx, ty);

                        t += t_step;
                    }
                }
            }

            // Draw a flat-top triangle
            if (dy_step_3 != 0)
            {
                for (int y = y3; y <= y2; y++)
                {
                    int sx = ((y - y3) * x_step_3 + x3).RoundToInt();
                    int ex = ((y - y3) * x_step_2 + x3).RoundToInt();

                    float stx = (y - y3) * tx_step_3 + tx3;
                    float sty = (y - y3) * ty_step_3 + ty3;
                    float stz = (y - y3) * tz_step_3 + tz3;

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;
                    float etz = (y - y3) * tz_step_2 + tz3;

                    if (sx > ex)
                    {
                        NumericManipulation.Swap(ref sx, ref ex);
                        NumericManipulation.Swap(ref stx, ref etx);
                        NumericManipulation.Swap(ref sty, ref ety);
                        NumericManipulation.Swap(ref stz, ref etz);
                    }

                    float t = 0, t_step = 1f / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        int tx = (stx + t * (etx - stx)).RoundToInt();
                        int ty = (sty + t * (ety - sty)).RoundToInt();
                        float tz = stz + t * (etz - stz);

                        TexturedCheckAgainstZBuffer(texture, x, y, tz, tx, ty);

                        t += t_step;
                    }
                }
            }
        }
    }
}