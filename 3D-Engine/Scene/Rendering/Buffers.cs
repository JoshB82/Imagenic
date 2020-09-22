using System;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        // Check if point is visible from the camera
        private void Z_Buffer_Check(object colour, int x, int y, float z)
        {
            try
            {
                if (z.Approx_Less_Than(z_buffer[x][y], 1E-4f))
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = (Color)colour;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException($"Attempted to render outside the canvas at ({x}, {y}, {z})", e);
            }
        }



        private void Textured_Check_Against_Z_Buffer(int x, int y, float z, int tx, int ty, Bitmap texture)
        {
            try
            {
                if (z < z_buffer[x][y])
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Attempted to draw outside the canvas.", e);
            }
        }
    }
}