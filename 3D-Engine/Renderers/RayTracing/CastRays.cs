using _3D_Engine.Entities;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers.RayTracing
{
    public partial class RayTracer
    {
        internal static async Task<Bitmap> CastRays(SceneObject sceneObject, Camera camera)
        {
            SceneObject scene = sceneObject.DeepCopy(); // ??
            sceneObject.RemoveChildren(x => !x.Visible || x is Camera); // ??

            foreach (SceneObject child in sceneObject.GetAllChildren())
            {
                
                

                
            }
            for (int i = 0; i < camera.RenderWidth; i++)
            {
                for (int j = 0; j < camera.RenderHeight; j++)
                {
                    CastRay(camera.WorldOrigin, );
                }
            }
        }

        internal static void CastRay(Vector3D startPosition, Vector3D direction)
        {
            Ray ray = new Ray(startPosition, direction);
        }
    }
}