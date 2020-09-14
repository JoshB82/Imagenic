using System;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        // Number and variable manipulation

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
            ref int x1, ref int y1, ref float z1, ref float tx1, ref float ty1,
            ref int x2, ref int y2, ref float z2, ref float tx2, ref float ty2,
            ref int x3, ref int y3, ref float z3, ref float tx3, ref float ty3)
        {
            // y1 lowest; y3 highest
            if (y1 < y2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
                Swap(ref z1, ref z2);
                Swap(ref tx1, ref tx2);
                Swap(ref ty1, ref ty2);
            }
            if (y1 < y3)
            {
                Swap(ref x1, ref x3);
                Swap(ref y1, ref y3);
                Swap(ref z1, ref z3);
                Swap(ref tx1, ref tx3);
                Swap(ref ty1, ref ty3);
            }
            if (y2 < y3)
            {
                Swap(ref x2, ref x3);
                Swap(ref y2, ref y3);
                Swap(ref z2, ref z3);
                Swap(ref tx2, ref tx3);
                Swap(ref ty2, ref ty3);
            }
        }

        // Interpolation (source!)
        private void Interpolate_Triangle(object @object, Action<object, int, int, float> action,
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
                x_step_1 = (x1 - x2) / dy_step_1; // dx from point 2 to point 1
                z_step_1 = (z1 - z2) / dy_step_1; // dz from point 2 to point 1
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
                    int ex = ((y - y3) * x_step_2 + x3).Round_to_Int();
                    float sz = (y - y2) * z_step_1 + z2;
                    float ez = (y - y3) * z_step_2 + z3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref sz, ref ez);
                    }

                    float t = 0, t_step = (float)1 / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);//?

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
                    float sz = (y - y3) * z_step_3 + z3;
                    float ez = (y - y3) * z_step_2 + z3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref sz, ref ez);
                    }

                    float t = 0, t_step = (float)1 / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float z = sz + t * (ez - sz);
                        action(@object, x, y, z);//?

                        t += t_step;
                    }
                }
            }
        }

        // floats or ints for everything? Should it take an average of texels?
        private void Textured_Triangle(Bitmap texture,
            int x1, int y1, float z1, float tx1, float ty1,
            int x2, int y2, float z2, float tx2, float ty2,
            int x3, int y3, float z3, float tx3, float ty3)
        {
            // Create steps
            int dy_step_1 = y1 - y2;
            int dy_step_2 = y1 - y3;
            int dy_step_3 = y2 - y3;

            float x_step_1 = 0, tx_step_1 = 0, ty_step_1 = 0;
            float x_step_3 = 0, tx_step_3 = 0, ty_step_3 = 0;

            if (dy_step_1 != 0)
            {
                x_step_1 = (x1 - x2) / dy_step_1; // dx from point 2 to point 1
                tx_step_1 = (tx1 - tx2) / dy_step_1; // dtx from point 2 to point 1
                ty_step_1 = (ty1 - ty2) / dy_step_1; // dty from point 2 to point 1
            }
            float x_step_2 = (x1 - x3) / dy_step_2; // dx from point 1 to point 3
            float tx_step_2 = (tx1 - tx3) / dy_step_2; // dtx from point 1 to point 3
            float ty_step_2 = (ty1 - ty3) / dy_step_2; // dty from point 1 to point 3
            if (dy_step_3 != 0)
            {
                x_step_3 = (x2 - x3) / dy_step_3; // dx from point 2 to point 3
                tx_step_3 = (tx2 - tx3) / dy_step_3; // dtx from point 2 to point 3
                ty_step_3 = (ty2 - ty3) / dy_step_3; // dty from point 2 to point 3
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

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;

                    // ?
                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref stx, ref etx);
                        Swap(ref sty, ref ety);
                    }

                    float t = 0, t_step = (float)1 / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float tx = stx + t * (etx - stx);
                        float ty = sty + t * (ety - sty);
                        t += t_step;
                        Textured_Check_Against_Z_Buffer(x, y, 1, tx.Round_to_Int(), ty.Round_to_Int(), texture); // ?
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

                    float etx = (y - y3) * tx_step_2 + tx3;
                    float ety = (y - y3) * ty_step_2 + ty3;

                    if (sx > ex)
                    {
                        Swap(ref sx, ref ex);
                        Swap(ref stx, ref etx);
                        Swap(ref sty, ref ety);
                    }

                    float t = 0, t_step = (float)1 / (ex - sx);
                    for (int x = sx; x <= ex; x++)
                    {
                        float tx = stx + t * (etx - stx);
                        float ty = sty + t * (ety - sty);
                        t += t_step;
                        Textured_Check_Against_Z_Buffer(x, y, 1, tx.Round_to_Int(), ty.Round_to_Int(), texture);
                    }
                }
            }
        }
    }
}
        /*
        private void Textured_Triangle(int x1, int y1, float z1, int x2, int y2, float z2, int x3, int y3, float z3, int tx1, int ty1, int tx2, int ty2, int tx3, int ty3, Bitmap texture)
        {
            Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(x1, y1, z1), new Vector3D(x2, y2, z2), new Vector3D(x3, y3, z3));
            float z_increase_x = -normal.X / normal.Z, z_increase_y = -normal.Y / normal.Z;
            /*
            Vector3D point_1 = new Vector3D(x1, y1, z1);
            Vector3D point_2 = new Vector3D(x2, y2, z2);
            Vector3D point_3 = new Vector3D(x3, y3, z3);
            Vector3D normal = Vector3D.Normal_From_Plane(point_1, point_2, point_3);
            */
            //Sort_By_Y_2(ref x1, ref y1, ref z1, ref x2, ref y2, ref z2, ref x3, ref y3, ref z3, ref tx1, ref ty1, ref tx2, ref ty2, ref tx3, ref ty3);
            /*
            // need abs if y1 is always (?) greater than y2?

            // dx from 2 to 1
            float tx_step_1 = (float)(tx1 - tx2) / Math.Abs(y2 - y1);
            // dy from 2 to 1
            float ty_step_1 = (float)(ty1 - ty2) / Math.Abs(y2 - y1);
            // dx from 3 to 1
            float tx_step_2 = (float)(tx1 - tx3) / Math.Abs(y3 - y1);
            // dy from 3 to 1
            float ty_step_2 = (float)(ty1 - ty3) / Math.Abs(y3 - y1);
            // dx from 3 to 2
            float tx_step_3 = (float)(tx2 - tx3) / Math.Abs(y3 - y2);
            // dy from 3 to 2 
            float ty_step_3 = (float)(ty2 - ty3) / Math.Abs(y3 - y2);

            int x4, y4;

            if (y1 == y2 && y2 == y3)
            {
                // Just draw a black line instead
                int start_x_value = Math.Min(Math.Min(x1, x2), x3), final_x_value = Math.Max(Math.Max(x1, x2), x3);
                float z_value = (start_x_value == x1) ? z1 : (start_x_value == x2) ? z2 : z3;
                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y1, z_value, Color.Black);
                    z_value += z_increase_x;
                }
            }
            else
            {
                if (y2 == y3)
                {
                    float z_value = (x2 < x3) ? z2 : z3;
                    x4 = x3; y4 = y3;
                    Textured_Flat_Bottom_Triangle(x1, y1, x2, y2, x3, y3, x4, y4, tx_step_1, ty_step_1, tx_step_2, ty_step_2, tx2, ty2, tx3, ty3, z_value, z_increase_x, z_increase_y, texture);
                }
                else
                {
                    if (y1 == y2)
                    {
                        float z_value = z3;
                        x4 = x1; y4 = y1;
                        Textured_Flat_Top_Triangle(x1, y1, x2, y2, x3, y3, x4, y4, tx_step_2, ty_step_2, tx_step_3, ty_step_3, tx3, ty3, z_value, z_increase_x, z_increase_y, texture);
                    }
                    else
                    {
                        // Needs tending to (SHOULD BE ROUNDED AT ALL?)
                        x4 = (int)Math.Round((float)((y2 - y1) * (x3 - x1) / (y3 - y1) + x1), MidpointRounding.AwayFromZero);
                        y4 = y2;
                        float z_value = z3;

                        Textured_Flat_Top_Triangle(x1, y1, x2, y2, x3, y3, x4, y4, tx_step_2, ty_step_2, tx_step_3, ty_step_3, tx3, ty3, z_value, z_increase_x, z_increase_y, texture);
                        Textured_Flat_Bottom_Triangle(x1, y1, x2, y2, x3, y3, x4, y4, tx_step_1, ty_step_1, tx_step_2, ty_step_2, tx2, ty2, tx3, ty3, z_value, z_increase_x, z_increase_y, texture);
                    }
                }
            }
        }

        private void Textured_Flat_Bottom_Triangle(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, float tx_step_1, float ty_step_1, float tx_step_2, float ty_step_2, float tx2, float ty2, float tx3, float ty3, float z_value, float z_increase_x, float z_increase_y, Bitmap texture)
        {
            // y1 must equal y2
            int[] start_x_values, final_x_values;

            if (x2 < x4)
            {
                start_x_values = Line_2(x2, y2, x1, y1);
                final_x_values = Line_2(x4, y4, x1, y1);
            }
            else
            {
                start_x_values = Line_2(x4, y4, x1, y1);
                final_x_values = Line_2(x2, y2, x1, y1);
            }

            int start_x_value, final_x_value;

            // Both horizontal lines? Nope!
            // WHy not use t stuff for above?
            //

            int prev_x = 0;
            for (int y = y2; y <= y1; y++)
            {
                start_x_value = start_x_values[y - y2];
                final_x_value = final_x_values[y - y2];

                float stx = (y - y2) * tx_step_1 + tx2;
                float sty = (y - y2) * ty_step_1 + ty2;

                float etx = (y - y3) * tx_step_2 + tx3;
                float ety = (y - y3) * ty_step_2 + ty3;

                if (stx > etx)
                {
                    //Swap(ref start_x_value, ref final_x_value);
                    Swap(ref stx, ref etx);
                    Swap(ref sty, ref ety);
                }

                float t = 0;

                if (y != y3) z_value += (start_x_value - prev_x) * z_increase_x;

                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    float tx = stx + t * (etx - stx);
                    float ty = sty + t * (ety - sty);
                    t += (float)1 / (final_x_value - start_x_value);
                    Check_Against_Z_Buffer_Texture(x, y, Round_To_Int(tx), Round_To_Int(ty), z_value, texture);
                    z_value += z_increase_x;
                }

                z_value -= z_increase_x * (final_x_value - start_x_value + 1);
                prev_x = start_x_value;
                if (y != y1) z_value += z_increase_y;
            }
        }

        private void Textured_Flat_Top_Triangle(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, float tx_step_1, float ty_step_1, float tx_step_2, float ty_step_2, float tx3, float ty3, float z_value, float z_increase_x, float z_increase_y, Bitmap texture)
        {
            // y1 must equal y2
            int[] start_x_values, final_x_values;

            if (x2 < x4)
            {
                start_x_values = Line_2(x3, y3, x2, y2);
                final_x_values = Line_2(x3, y3, x4, y4);
            }
            else
            {
                start_x_values = Line_2(x3, y3, x4, y4);
                final_x_values = Line_2(x3, y3, x2, y2);
            }

            int start_x_value, final_x_value;

            int prev_x = 0;
            for (int y = y3; y <= y2; y++)
            {
                start_x_value = start_x_values[y - y3];
                final_x_value = final_x_values[y - y3];

                float stx = (y - y3) * tx_step_2 + tx3;
                float sty = (y - y3) * ty_step_2 + ty3;

                float etx = (y - y3) * tx_step_1 + tx3;
                float ety = (y - y3) * ty_step_1 + ty3;

                if (stx > etx)
                {
                    //Swap(ref start_x_value, ref final_x_value);
                    Swap(ref stx, ref etx);
                    Swap(ref sty, ref ety);
                }

                float t = 0;

                if (y != y1) z_value += (start_x_value - prev_x) * z_increase_x;

                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    float tx = stx + t * (etx - stx);
                    float ty = sty + t * (ety - sty);
                    t += (float)1 / (final_x_value - start_x_value);
                    Check_Against_Z_Buffer_Texture(x, y, Round_To_Int(tx), Round_To_Int(ty), z_value, texture);
                    z_value += z_increase_x;
                }

                z_value -= z_increase_x * (final_x_value - start_x_value + 1);
                prev_x = start_x_value;
                z_value += z_increase_y;
            }
        }

        private void Triangle(int x1, int y1, float z1, int x2, int y2, float z2, int x3, int y3, float z3, Color colour)
        {
            Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(x1, y1, z1), new Vector3D(x2, y2, z2), new Vector3D(x3, y3, z3));
            float z_increase_x = -normal.X / normal.Z, z_increase_y = -normal.Y / normal.Z;

            Sort_By_Y(ref x1, ref y1, ref z1, ref x2, ref y2, ref z2, ref x3, ref y3, ref z3);

            int x4;
            if (y1 == y2 && y2 == y3)
            {
                int start_x_value = Math.Min(Math.Min(x1, x2), x3), final_x_value = Math.Max(Math.Max(x1, x2), x3);
                float z_value = (start_x_value == x1) ? z1 : (start_x_value == x2) ? z2 : z3;
                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y1, z_value, colour);
                    z_value += z_increase_x;
                }
            }
            else
            {
                if (y2 == y3)
                {
                    float z_value = (x2 < x3) ? z2 : z3;
                    Flat_Bottom_Triangle(x2, y2, x3, y3, x1, y1, z_value, z_increase_x, z_increase_y, colour);
                }
                else
                {
                    if (y1 == y2)
                    {
                        float z_value = z3;
                        Flat_Top_Triangle(x1, y1, x2, y2, x3, y3, z_value, z_increase_x, z_increase_y, colour);
                    }
                    else
                    {
                        x4 = (int)Math.Round((float)((y2 - y1) * (x3 - x1) / (y3 - y1) + x1), MidpointRounding.AwayFromZero);
                        int y4 = y2;
                        float z_value = z3;

                        Flat_Top_Triangle(x2, y2, x4, y4, x3, y3, z_value, z_increase_x, z_increase_y, colour);
                        Flat_Bottom_Triangle(x2, y2, x4, y4, x1, y1, z_value, z_increase_x, z_increase_y, colour);
                    }
                }
            }
        }

        private void Flat_Bottom_Triangle(int x1, int y1, int x2, int y2, int x3, int y3, float z_value, float z_increase_x, float z_increase_y, Color colour)
        {
            // y1 must equal y2
            int[] start_x_values, final_x_values;

            if (x1 < x2)
            {
                start_x_values = Line_2(x1, y1, x3, y3);
                final_x_values = Line_2(x2, y2, x3, y3);
            }
            else
            {
                start_x_values = Line_2(x2, y2, x3, y3);
                final_x_values = Line_2(x1, y1, x3, y3);
            }

            int start_x_value, final_x_value, prev_x = 0;
            for (int y = y1; y <= y3; y++)
            {
                start_x_value = start_x_values[y - y1];
                final_x_value = final_x_values[y - y1];

                if (y != y1) z_value += (start_x_value - prev_x) * z_increase_x;

                for (int x = start_x_value; x <= final_x_value; x++)
                {
                    Check_Against_Z_Buffer(x, y, z_value, colour);
                    z_value += z_increase_x;
                }
                z_value -= z_increase_x * (final_x_value - start_x_value + 1);
                prev_x = start_x_value;
                z_value += z_increase_y;
            }
        }

        */

/*
                        // Adjust point colour based on lighting
                        Color adjusted_colour = face.Colour;
                        foreach (Light light in Lights)
                        {
                            Vector3D light_to_face = new Vector3D() - light.World_Origin;
                            float intensity_at_face = 1 / Math.Pow(light_to_face.Magnitude(), 2) * light.Intensity;

                            switch (light.GetType().Name)
                            {
                                case "Point_Light":
                                    adjusted_colour = Mix_Colour();
                                    adjusted_colour = new Color() + new Color();
                                    break;
                            }
                        }*/