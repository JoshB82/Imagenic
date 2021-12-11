using _3D_Engine.Entities;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
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

        internal static void CastRay(IEnumerable<Triangle> triangles, Vector3D point1, Vector3D point2, out int rayCount)
        {
            rayCount = 0;

            Ray ray = new Ray(point1, point2);

            int numTasks = 4; // Make configurable.
            Task[] tasks = new Task[numTasks];
            List<Triangle[]> triangleBatches = triangles.Chunk(triangles.Count() / numTasks).ToList();

            for (int i = 0; i < triangleBatches.Count; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    foreach (Triangle triangle in triangleBatches[i])
                    {
                        Vector3D? closestIntersection = null;
                        float smallestDistance = 100; // ??

                        if (ray.DoesIntersect(triangle, out Vector3D intersection))
                        {
                            float distance = intersection.DistanceFrom(point1);
                            if (distance < smallestDistance)
                            {
                                smallestDistance = distance;
                                closestIntersection = intersection;
                            }

                            rayCount++;
                            
                        }

                        return closestIntersection;
                    }
                });

                tasks[i].ContinueWith(task =>
                {
                    CastRay(task.Result, out rayCount);
                });
            }

            Task.WaitAll(tasks);
        }
    }
}