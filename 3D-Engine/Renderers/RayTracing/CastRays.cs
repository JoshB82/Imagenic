using _3D_Engine.Entities;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using _3D_Engine.Maths.Vectors;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers.RayTracing
{
    public partial class RayTracer
    {
        internal const int maxRayCount = 5;

        internal static async Task<Image> CastRays(IEnumerable<Triangle> triangles, Camera camera)
        {
            SceneObject scene = sceneObject.DeepCopy(); // ??
            sceneObject.RemoveChildren(x => !x.Visible || x is Camera); // ??

            Buffer2D<Task<bool>> taskBuffer = new Buffer2D<Task<bool>>(camera.RenderWidth, camera.RenderHeight);


            taskBuffer.ForEach(task =>
            {
                task = Task.Factory.StartNew(() =>
                {
                    return CastRay();
                });
            });

            Task.WaitAll((Task<bool>[])taskBuffer);

            for (int i = 0; i < camera.RenderWidth; i++)
            {
                for (int j = 0; j < camera.RenderHeight; j++)
                {
                    
                    //CastRay(camera.WorldOrigin, );

                    //tasks[i].ContinueWith(task =>
                    //{

                    //});
                }
            }
        }

        internal static bool CastRay(IEnumerable<Triangle> triangles, Vector3D point1, Vector3D point2, out int rayCount)
        {
            rayCount = 0;

            Ray ray = new Ray(point1, point2);

            int numTasks = 4; // Make configurable.
            Task<Vector3D?>[] tasks = new Task<Vector3D?>[numTasks];
            int maxSize = triangles.Count() / numTasks;
            List<Triangle[]> triangleBatches = triangles.Chunk(maxSize).ToList();

            Vector3D? closestIntersection = null;
            float smallestDistance = 100; // ??

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    foreach (Triangle triangle in triangleBatches[i])
                    {
                        if (ray.DoesIntersect(triangle, out Vector3D intersection))
                        {
                            float distance = intersection.DistanceFrom(point1);
                            if (distance < smallestDistance)
                            {
                                smallestDistance = distance;
                                closestIntersection = intersection;
                            }
                        }

                        return closestIntersection;
                    }
                });

                
            }

            Task.WaitAll(tasks);

            if (closestIntersection is not null && rayCount < maxRayCount)
            {
                rayCount++;
                return CastRay(triangles, closestIntersection, out rayCount);
            }
            else
            {
                return false;
            }
        }
    }
}