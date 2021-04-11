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
            
        }
    }
}