using System;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Cameras
{
    public abstract partial class Camera : SceneObject
    {
        

        private void TexturedCheckAgainstZBuffer(Bitmap texture, int x, int y, float z, int tx, int ty)
        {
            try
            {
                if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
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