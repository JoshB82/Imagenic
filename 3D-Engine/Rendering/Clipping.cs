/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for clipping edges and faces against specified planes.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.RenderingObjects;
using System.Collections.Generic;
using static _3D_Engine.Maths.Vectors.Vector3D;

namespace _3D_Engine.Rendering
{
    internal static class Clipping
    {
        internal static bool ClipEdges(ClippingPlane[] clippingPlanes, ref Vector4D point1, ref Vector4D point2)
        {
            foreach (ClippingPlane clippingPlane in clippingPlanes)
            {
                if (!ClipEdge(clippingPlane.Point, clippingPlane.Normal, ref point1, ref point2)) return false;
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

        //source!
        internal static bool ClipFaces(Queue<Face> faceQueue, ClippingPlane[] clippingPlanes)
        {
            foreach (ClippingPlane clippingPlane in clippingPlanes)
            {
                int noFaces = faceQueue.Count;
                while (noFaces-- > 0) ClipFace(faceQueue.Dequeue(), faceQueue, clippingPlane.Point, clippingPlane.Normal);
            }

            return faceQueue.Count > 0;
        }

        // check clockwise/anticlockwise stuff
        // source (for everything in file)
        internal static void ClipFace(Face faceToClip, Queue<Face> facesQueue, Vector3D planePoint, Vector3D planeNormal)
        {
            Vector4D[] insidePoints = new Vector4D[3], outsidePoints = new Vector4D[3];
            Vector3D[] insideTexturePoints = new Vector3D[3], outsideTexturePoints = new Vector3D[3];
            int insidePointCount = 0, outsidePointCount = 0;

            if (PointDistanceFromPlane((Vector3D)faceToClip.p1, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = faceToClip.p1;
                insideTexturePoints[insidePointCount++] = faceToClip.t1;
            }
            else
            {
                outsidePoints[outsidePointCount] = faceToClip.p1;
                outsideTexturePoints[outsidePointCount++] = faceToClip.t1;
            }

            if (PointDistanceFromPlane((Vector3D)faceToClip.p2, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = faceToClip.p2;
                insideTexturePoints[insidePointCount++] = faceToClip.t2;
            }
            else
            {
                outsidePoints[outsidePointCount] = faceToClip.p2;
                outsideTexturePoints[outsidePointCount++] = faceToClip.t2;
            }

            if (PointDistanceFromPlane((Vector3D)faceToClip.p3, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = faceToClip.p3;
                insideTexturePoints[insidePointCount++] = faceToClip.t3;
            }
            else
            {
                outsidePoints[outsidePointCount] = faceToClip.p3;
                outsideTexturePoints[outsidePointCount] = faceToClip.T3;
            }

            switch (insidePointCount)
            {
                case 0:
                    // All points are on the outside, so no valid faces to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller face is needed
                    Vector4D intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out float d1), 1);
                    Vector4D intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[1], planePoint, planeNormal, out float d2), 1);

                    Face face1;
                    if (faceToClip.HasTexture)
                    {
                        Vector3D tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                        Vector3D tIntersection2 = (outsideTexturePoints[1] - insideTexturePoints[0]) * d2 + insideTexturePoints[0];

                        face1 = new Face(insidePoints[0], intersection1, intersection2, insideTexturePoints[0], tIntersection1, tIntersection2, faceToClip.Texture_Object) { HasTexture = true };
                    }
                    else
                    {
                        face1 = new Face(insidePoints[0], intersection1, intersection2) { Colour = faceToClip.Colour };
                    }

                    facesQueue.Enqueue(face1);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two faces
                    intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d1), 1);
                    intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[1], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d2), 1);

                    Face face2;
                    if (faceToClip.HasTexture)
                    {
                        Vector3D tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                        Vector3D tIntersection2 = (outsideTexturePoints[0] - insideTexturePoints[1]) * d2 + insideTexturePoints[1];

                        face1 = new Face(insidePoints[0], intersection1, insidePoints[1], insideTexturePoints[0], tIntersection1, insideTexturePoints[1], faceToClip.Texture_Object) { HasTexture = true };
                        face2 = new Face(insidePoints[1], intersection1, intersection2, insideTexturePoints[1], tIntersection1, tIntersection2, faceToClip.Texture_Object) { HasTexture = true };
                    }
                    else
                    {
                        face1 = new Face(insidePoints[0], intersection1, insidePoints[1]) { Colour = faceToClip.Colour };
                        face2 = new Face(insidePoints[1], intersection1, intersection2) { Colour = faceToClip.Colour };
                    }

                    facesQueue.Enqueue(face1);
                    facesQueue.Enqueue(face2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the face unchanged
                    facesQueue.Enqueue(faceToClip);
                    break;
            }
        }
    }
}