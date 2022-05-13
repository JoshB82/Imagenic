using _3D_Engine.Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Imagenic.Core.Renderers;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    public abstract partial class Camera : RenderingObject
    {
        private static Bitmap CreateBitmap(int width, int height, Buffer2D<Color> colourBuffer, PixelFormat pixelFormat)
        {
            Bitmap newFrame = new(width, height, pixelFormat);
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

        private static unsafe void Format24bppRgb(
            int width,
            int height,
            BitmapData data,
            Buffer2D<Color> colourBuffer)
        {
            const int noTasks = 4; // TODO: Move to configuration

            int smallHeight = height / noTasks;
            int noSmallHeights = noTasks - height % noTasks;
            int largeHeight = smallHeight + 1;
            int noLargeHeights = height % noTasks;

            Task[] renderTasks = new Task[noTasks];

            for (int i = 0; i < noSmallHeights; i++)
            {
                int ii = i;
                renderTasks[i] = Task.Factory.StartNew(() =>
                {
                    for (int y = ii * smallHeight; y < (ii + 1) * smallHeight; y++)
                    {
                        byte* rowStart = (byte*)data.Scan0 + y * data.Stride;
                        for (int x = 0; x < width; x++)
                        {
                            rowStart[x * 3] = colourBuffer.Values[x][y * -1 + height - 1].B; // Blue
                            rowStart[x * 3 + 1] = colourBuffer.Values[x][y * -1 + height - 1].G; // Green
                            rowStart[x * 3 + 2] = colourBuffer.Values[x][y * -1 + height - 1].R; // Red
                        }
                    }

                    #if DEBUG

                    ConsoleOutput.DisplayMessage("Task completed.");

                    #endif
                });
            }

            for (int i = 0; i < noLargeHeights; i++)
            {
                int ii = i;
                renderTasks[i + noSmallHeights] = Task.Factory.StartNew(() =>
                {
                    for (int y = ii * largeHeight + noSmallHeights * smallHeight; y < (ii + 1) * largeHeight + noSmallHeights * smallHeight; y++)
                    {
                        byte* rowStart = (byte*)data.Scan0 + y * data.Stride;
                        for (int x = 0; x < width; x++)
                        {
                            rowStart[x * 3] = colourBuffer.Values[x][y * -1 + height - 1].B; // Blue
                            rowStart[x * 3 + 1] = colourBuffer.Values[x][y * -1 + height - 1].G; // Green
                            rowStart[x * 3 + 2] = colourBuffer.Values[x][y * -1 + height - 1].R; // Red
                        }
                    }

                    #if DEBUG

                    ConsoleOutput.DisplayMessage("Task completed.");

                    #endif
                });
            }

            Task.WaitAll(renderTasks);
        }
    }
}