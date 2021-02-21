using _3D_Engine.Rendering;
using System.Drawing;
using System.Drawing.Imaging;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    public abstract partial class Camera : RenderingObject
    {
        // How works?
        private static Bitmap CreateBitmap(int width, int height, Buffer2D<Color> colourBuffer, PixelFormat pixelFormat)
        {
            Bitmap newFrame = new Bitmap(width, height, pixelFormat);
            BitmapData data = newFrame.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat); //????????????

            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    Format24bppRgb(width, height, data, colourBuffer);
                    break;
            }

            newFrame.UnlockBits(data);
            return newFrame;
        }

        private static unsafe void Format24bppRgb(int width, int height, BitmapData data, Buffer2D<Color> colourBuffer)
        {
            for (int y = 0; y < height; y++)
            {
                byte* rowStart = (byte*)data.Scan0 + y * data.Stride;
                for (int x = 0; x < width; x++)
                {
                    rowStart[x * 3] = colourBuffer.Values[x][y * -1 + height - 1].B; // Blue
                    rowStart[x * 3 + 1] = colourBuffer.Values[x][y * -1 + height - 1].G; // Green
                    rowStart[x * 3 + 2] = colourBuffer.Values[x][y * -1 + height - 1].R; // Red
                }
            }
        }
    }
}