/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines static methods for clipping edges and faces against specified planes.
 */

using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using System.Collections.Generic;
using static _3D_Engine.Maths.Vectors.Vector3D;

namespace Imagenic.Core.Renderers.Rasterising
{
    internal static class Clipping
    {
        // Edges
        internal static bool ClipEdges(ClippingPlane[] clippingPlanes, ref Vector4D point1, ref Vector4D point2)
        {
            foreach (ClippingPlane clippingPlane in clippingPlanes)
            {
                if (!ClipEdge(clippingPlane.Point, clippingPlane.Normal, ref point1, ref point2))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool ClipEdge(Vector3D planePoint, Vector3D planeNormal, ref Vector4D point1, ref Vector4D point2)
        {
            float point1Distance = PointDistanceFromPlane((Vector3D)point1, planePoint, planeNormal);
            float point2Distance = PointDistanceFromPlane((Vector3D)point2, planePoint, planeNormal);

            if (point1Distance >= 0)
            {
                if (point2Distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the line
                    point2 = LineIntersectPlane((Vector3D)point1, (Vector3D)point2, planePoint, planeNormal, out _);
                }
                // If above condition fails, both points are on the inside, so return line unchanged
                return true;
            }

            if (point2Distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector3D intersection = LineIntersectPlane((Vector3D)point2, (Vector3D)point1, planePoint, planeNormal, out _);
                point1 = point2;
                point2 = intersection;
                return true;
            }

            // Both points are on the outside, so discard the line
            return false;
        }

        // Triangles
        internal static bool ClipTriangles(Queue<Triangle> triangleQueue, ClippingPlane[] clippingPlanes)
        {
            foreach (ClippingPlane clippingPlane in clippingPlanes)
            {
                int noFaces = triangleQueue.Count;
                while (noFaces-- > 0) ClipTriangle(triangleQueue.Dequeue(), triangleQueue, clippingPlane.Point, clippingPlane.Normal);
            }

            return triangleQueue.Count > 0;
        }

        // check clockwise/anticlockwise stuff
        // source (for everything in file)
        internal static void ClipTriangle(Triangle triangleToClip, Queue<Triangle> trianglesQueue, Vector3D planePoint, Vector3D planeNormal)
        {
            Vector4D[] insidePoints = new Vector4D[3], outsidePoints = new Vector4D[3];
            Vector3D[] insideTexturePoints = new Vector3D[3], outsideTexturePoints = new Vector3D[3];
            int insidePointCount = 0, outsidePointCount = 0;

            if (PointDistanceFromPlane((Vector3D)triangleToClip.P1, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangleToClip.P1;
                if (triangleToClip is TextureTriangle textureFace) insideTexturePoints[insidePointCount] = textureFace.T1;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangleToClip.P1;
                if (triangleToClip is TextureTriangle textureFace) outsideTexturePoints[outsidePointCount] = textureFace.T1;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)triangleToClip.P2, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangleToClip.P2;
                if (triangleToClip is TextureTriangle textureFace) insideTexturePoints[insidePointCount] = textureFace.T2;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangleToClip.P2;
                if (triangleToClip is TextureTriangle textureFace) outsideTexturePoints[outsidePointCount] = textureFace.T2;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)triangleToClip.P3, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangleToClip.P3;
                if (triangleToClip is TextureTriangle textureFace) insideTexturePoints[insidePointCount] = textureFace.T3;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangleToClip.P3;
                if (triangleToClip is TextureTriangle textureFace) outsideTexturePoints[outsidePointCount] = textureFace.T3;
            }

            switch (insidePointCount)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    Vector4D intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out float d1), 1);
                    Vector4D intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[1], planePoint, planeNormal, out float d2), 1);

                    Triangle triangle1;
                    if (triangleToClip is TextureTriangle)
                    {
                        Vector3D tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                        Vector3D tIntersection2 = (outsideTexturePoints[1] - insideTexturePoints[0]) * d2 + insideTexturePoints[0];

                        triangle1 = new TextureTriangle(insidePoints[0], intersection1, intersection2, insideTexturePoints[0], tIntersection1, tIntersection2, ((TextureTriangle)triangleToClip).TextureObject);
                    }
                    else
                    {
                        triangle1 = new SolidTriangle(insidePoints[0], intersection1, intersection2) { Colour = ((SolidTriangle)triangleToClip).Colour };
                    }

                    trianglesQueue.Enqueue(triangle1);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d1), 1);
                    intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[1], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d2), 1);

                    Triangle triangle2;
                    if (triangleToClip is TextureTriangle)
                    {
                        Vector3D tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                        Vector3D tIntersection2 = (outsideTexturePoints[0] - insideTexturePoints[1]) * d2 + insideTexturePoints[1];

                        triangle1 = new TextureTriangle(insidePoints[0], intersection1, insidePoints[1], insideTexturePoints[0], tIntersection1, insideTexturePoints[1], ((TextureTriangle)triangleToClip).TextureObject);
                        triangle2 = new TextureTriangle(insidePoints[1], intersection1, intersection2, insideTexturePoints[1], tIntersection1, tIntersection2, ((TextureTriangle)triangleToClip).TextureObject);
                    }
                    else
                    {
                        triangle1 = new SolidTriangle(insidePoints[0], intersection1, insidePoints[1]) { Colour = ((SolidTriangle)triangleToClip).Colour };
                        triangle2 = new SolidTriangle(insidePoints[1], intersection1, intersection2) { Colour = ((SolidTriangle)triangleToClip).Colour };
                    }

                    trianglesQueue.Enqueue(triangle1);
                    trianglesQueue.Enqueue(triangle2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    trianglesQueue.Enqueue(triangleToClip);
                    break;
            }
        }
    }
}