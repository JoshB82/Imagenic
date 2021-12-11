using _3D_Engine.Entities;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
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
        internal static async Task<Image> CastRays(IEnumerable<Triangle> triangles, Camera camera)
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
                    //CastRay(camera.WorldOrigin, );
                }
            }
        }

        internal static void CastRay(IEnumerable<Triangle> triangles, Vector3D startPosition, Vector3D direction, out int rayCount)
        {
            rayCount = 0;

            Ray ray = new Ray(startPosition, direction);

            int numTasks = 4; // Make configurable.
            IEnumerable<Triangle[]> triangleBatches = triangles.Chunk(triangles.Count() / numTasks);


            Task[] tasks = new Task[numTasks];

            foreach (Triangle[] triangleBatch in triangleBatches)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    if (ray.DoesIntersect())
                    {
                        return new Ray(,, out rayCount++);
                    }
                });
            }

            for (int i = 0; i < numTasks - 1; i++)
            {
                
            }

            Task.WaitAll(tasks);
        }
    }
}