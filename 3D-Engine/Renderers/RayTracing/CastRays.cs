using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Renderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.RayTracing
{
    public partial class RayTracer
    {
        /*
        private Buffer2D<Task<bool>> taskBuffer;
        private Buffer2D<Color> colourBuffer;

        internal const int maxRayCount = 5;

        internal Task<Buffer2D<Color>> CastRays(List<Triangle> triangles, IEnumerable<Light> lights, Camera camera, CancellationToken token)
        {
            taskBuffer.SetAllToDefault();
            colourBuffer.SetAllToDefault();

            taskBuffer = new Buffer2D<Task<bool>>(camera.RenderWidth, camera.RenderHeight);
            colourBuffer = new Buffer2D<Color>(camera.RenderWidth, camera.RenderHeight);

            taskBuffer.ForEach(task =>
            {
                task = Task.Factory.StartNew(() =>
                {
                    if (CastRay(triangles, token))
                    {
                        colourBuffer.Values[][] = ;
                    }
                });
            });

            Task.WaitAll((Task<bool>[])taskBuffer);

            return colourBuffer;
        }

        internal static bool? CastRay(List<Triangle> triangles, Vector3D point1, Vector3D point2, out int rayCount, CancellationToken token)
        {
            rayCount = 0;
            if (token.IsCancellationRequested)
            {
                return null;
            }

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
        */
    }
}