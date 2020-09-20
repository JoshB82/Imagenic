using System;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
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