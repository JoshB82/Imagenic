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
                if (z.Approx_Less_Than(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                    colourBuffer.Values[x][y] = (Color)colour;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException($"Attempted to render outside the canvas at ({x}, {y}, {z})", e);
            }
        }

        private void Textured_Check_Against_Z_Buffer(Bitmap texture, int x, int y, float z, int tx, int ty)
        {
            try
            {
                if (z.Approx_Less_Than(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                    colourBuffer.Values[x][y] = texture.GetPixel(tx, ty * -1 + texture.Height - 1);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException($"Attempted to render outside the canvas at ({x}, {y}, {z})", e);
            }
        }
    }
}