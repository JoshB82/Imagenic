using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Check_Against_Z_Buffer(int x, int y, double z, Color new_colour)
        {
            
        }

        private void Textured_Check_Against_Z_Buffer(int x, int y, double z, int tx, int ty, Bitmap texture)
        {
            try
            {
                if (z < z_buffer[x][y])
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Attempted to draw outside the canvas.");
            }
        }

        private void Light_Check_Against_Z_Buffer(Light light, int x, int y, double z)
        {
            
        }
    }
}