using _3D_Engine.Rendering;
using System;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Cameras
{
    public abstract partial class Camera : SceneObject
    {
        // sources for everything in file
        private void Line(Color colour,
            int x1, int y1, float z1,
            int x2, int y2, float z2)
        {
            if (x1 == x2)
            {
                Vertical_Line(x1, y1, z1, x2, y2, z2, colour);
            }
            else
            {
                if (y1 == y2)
                {
                    Horizontal_Line(x1, y1, z1, x2, y2, z2, colour);
                }
                else
                {
                    float z_increase_x = (z1 - z2) / (x1 - x2), z_increase_y = (z1 - z2) / (y1 - y2);

                    int delta_x = x2 - x1;
                    int delta_y = y2 - y1;

                    int increment_x = Math.Sign(delta_x);
                    int increment_y = Math.Sign(delta_y);

                    delta_x = Math.Abs(delta_x);
                    delta_y = Math.Abs(delta_y);

                    int x = x1, y = y1, R = 0, D = Math.Max(delta_x, delta_y);
                    float z_value = z1;

                    if (delta_x > delta_y)
                    {
                        for (int i = 0; i <= D; i++)
                        {
                            ZBufferCheck(colour, x, y, z_value);
                            x += increment_x;
                            z_value += z_increase_x * increment_x;
                            R += 2 * delta_y;
                            if (R >= delta_x)
                            {
                                R -= 2 * delta_x;
                                y += increment_y;
                                z_value += z_increase_y * increment_y;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= D; i++)
                        {
                            ZBufferCheck(colour, x, y, z_value);
                            y += increment_y;
                            z_value += z_increase_y * increment_y;
                            R += 2 * delta_x;
                            if (R >= delta_y)
                            {
                                R -= 2 * delta_y;
                                x += increment_x;
                                z_value += z_increase_x * increment_x;
                            }
                        }
                    }
                }
            }
        }

        private void Horizontal_Line(int x1, int y1, float z1, int x2, int y2, float z2, Color colour)
        {
            float z_increase_x = (z1 - z2) / (x1 - x2);
            if (x2 < x1)
            {
                NumericManipulation.Swap(ref x1, ref x2);
                NumericManipulation.Swap(ref z1, ref z2);
            }

            for (int x = x1; x <= x2; x++)
            {
                ZBufferCheck(colour, x, y1, z1);
                z1 += z_increase_x;
            }
        }

        private void Vertical_Line(int x1, int y1, float z1, int x2, int y2, float z2, Color colour)
        {
            float z_increase_y = (z1 - z2) / (y1 - y2);
            if (y2 < y1)
            {
                NumericManipulation.Swap(ref y1, ref y2);
                NumericManipulation.Swap(ref z1, ref z2);
            }

            for (int y = y1; y <= y2; y++)
            {
                ZBufferCheck(colour, x1, y, z1);
                z1 += z_increase_y;
            }
        }

        private int[] Line_2(int x1, int y1, int x2, int y2)
        {
            int delta_x = x2 - x1;
            int delta_y = y2 - y1;

            int increment_x = Math.Sign(delta_x);
            int increment_y = Math.Sign(delta_y);

            delta_x = Math.Abs(delta_x);
            delta_y = Math.Abs(delta_y);

            int D = Math.Max(delta_x, delta_y);

            int[] x_values = new int[delta_y + 1];
            x_values[0] = x1;

            int x = x1;
            int y = y1;
            int R = 0;
            int y_count = 0;

            if (delta_x > delta_y)
            {
                for (int i = 0; i <= D; i++)
                {
                    x += increment_x;
                    R += 2 * delta_y;
                    if (R >= delta_x)
                    {
                        R -= 2 * delta_x;
                        y += increment_y;
                        if (i != D)
                        {
                            y_count++;
                            x_values[y_count] = x;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i <= D; i++)
                {
                    y += increment_y;
                    R += 2 * delta_x;
                    if (R >= delta_y)
                    {
                        R -= 2 * delta_y;
                        x += increment_x;
                    }
                    if (i != D)
                    {
                        y_count++;
                        x_values[y_count] = x;
                    }
                }
            }
            return x_values;
        }
    }
}