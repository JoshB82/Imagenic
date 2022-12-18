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
using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using System;
using System.Collections.Generic;
using static Imagenic.Core.Maths.Vectors.Vector3D;

namespace Imagenic.Core.Renderers.Rasterising;

internal sealed class EdgeClipper<TEdge> where TEdge : Edge
{
    #region Fields and Properties

    internal ClippingPlane[] ClippingPlanes { get; set; }
    internal TEdge EdgeToClip { get; set; }

    #endregion

    #region Constructors

    internal EdgeClipper(TEdge edgeToClip, ClippingPlane[] clippingPlanes)
    {
        EdgeToClip = edgeToClip;
        ClippingPlanes = clippingPlanes;
    }

    #endregion

    #region Methods

    internal bool Clip(ref Vector4D point1, ref Vector4D point2)
    {
        foreach (ClippingPlane clippingPlane in ClippingPlanes)
        {
            if (!ClipEdge(clippingPlane.Point, clippingPlane.Normal))
            {
                return false;
            }
        }
        return true;

        bool ClipEdge(Vector3D planePoint, Vector3D planeNormal)
        {
            switch (EdgeToClip)
            {
                case SolidEdge se:
                    return ClipSolidEdge(se, planePoint, planeNormal);
                case DashedEdge de:
                    return ClipDashedEdge(de, planePoint, planeNormal);
                default:
                    throw new Exception("Unsupported edge type.");
            }
        }

        bool ClipSolidEdge(SolidEdge se, Vector3D planePoint, Vector3D planeNormal)
        {
            var point1 = se.P1.Point;
            var point2 = se.P2.Point;

            float point1Distance = PointDistanceFromPlane(point1, planePoint, planeNormal);
            float point2Distance = PointDistanceFromPlane(point2, planePoint, planeNormal);

            if (point1Distance >= 0)
            {
                if (point2Distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the edge
                    se.P2.Point = LineIntersectPlane(point1, point2, planePoint, planeNormal, out _);
                }
                // If above condition fails, both points are on the inside, so return line unchanged
                return true;
            }

            if (point2Distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the edge
                Vector3D intersection = LineIntersectPlane(point2, point1, planePoint, planeNormal, out _);
                se.P1.Point = point2;
                se.P2.Point = intersection;
                return true;
            }

            // Both points are on the outside, so discard the line
            return false;
        }

        bool ClipDashedEdge(DashedEdge de, Vector3D planePoint, Vector3D planeNormal)
        {
            var point1 = de.P1.Point;
            var point2 = de.P2.Point;

            float point1Distance = PointDistanceFromPlane(point1, planePoint, planeNormal);
            float point2Distance = PointDistanceFromPlane(point2, planePoint, planeNormal);

            if (point1Distance >= 0)
            {
                if (point2Distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the edge
                    de.P2.Point = LineIntersectPlane(point1, point2, planePoint, planeNormal, out _);
                }
                // If above condition fails, both points are on the inside, so return line unchanged
                return true;
            }

            if (point2Distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the edge
                Vector3D intersection = LineIntersectPlane(point2, point1, planePoint, planeNormal, out _);
                de.P1.Point = point2;
                de.P2.Point = intersection;
                return true;
            }

            // Both points are on the outside, so discard the line
            return false;
        }
    
        bool ClipGradientEdge(GradientEdge ge, Vector3D planePoint, Vector3D planeNormal)
        {
            var point1 = ge.P1.Point;
            var point2 = ge.P2.Point;

            float point1Distance = PointDistanceFromPlane(point1, planePoint, planeNormal);
            float point2Distance = PointDistanceFromPlane(point2, planePoint, planeNormal);

            if (point1Distance >= 0)
            {
                if (point2Distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the edge
                    ge.P2.Point = LineIntersectPlane(point1, point2, planePoint, planeNormal, out _);
                }
                // If above condition fails, both points are on the inside, so return line unchanged
                return true;
            }

            if (point2Distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the edge
                Vector3D intersection = LineIntersectPlane(point2, point1, planePoint, planeNormal, out _);
                ge.P1.Point = point2;
                ge.P2.Point = intersection;
                return true;
            }

            // Both points are on the outside, so discard the line
            return false;
        }
    }

    #endregion
}

internal sealed class TriangleClipper<TTriangle> where TTriangle : Triangle
{
    #region Fields and Properties

    internal ClippingPlane[] ClippingPlanes { get; set; }
    internal Queue<TTriangle> TriangleQueue => new();

    #endregion

    #region Constructors

    internal TriangleClipper(TTriangle startingTriangle, ClippingPlane[] clippingPlanes)
    {
        ClippingPlanes = clippingPlanes;
        TriangleQueue.Enqueue(startingTriangle);
    }

    #endregion

    #region Methods

    internal Queue<TTriangle> Clip()
    {
        Vector4D[] insidePoints = new Vector4D[3], outsidePoints = new Vector4D[3];
        int insidePointCount = 0, outsidePointCount = 0;

        //if (TriangleQueue.Count > 0)
        //{

        //}
        switch (TriangleQueue)
        {
            case Queue<SolidTriangle> solidTriangleQueue:
                foreach (ClippingPlane clippingPlane in ClippingPlanes)
                {
                    int noTrianglesRemaining = TriangleQueue.Count;
                    while (noTrianglesRemaining-- > 0) ClipSolidTriangle(solidTriangleQueue.Dequeue(), clippingPlane.Point, clippingPlane.Normal);
                }
                break;
            case Queue<TextureTriangle> textureTriangleQueue:
                foreach (ClippingPlane clippingPlane in ClippingPlanes)
                {
                    int noTrianglesRemaining = TriangleQueue.Count;
                    while (noTrianglesRemaining-- > 0) ClipTextureTriangle(textureTriangleQueue.Dequeue(), clippingPlane.Point, clippingPlane.Normal);
                }
                break;
            default:
                throw new Exception("Unsupported triangle type.");
        }

        return TriangleQueue;


        void ClipSolidTriangle(SolidTriangle st, Vector3D planePoint, Vector3D planeNormal)
        {
            // Determine what vertices of the SolidTriangle are inside and outside.
            if (PointDistanceFromPlane((Vector3D)st.P1, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = st.P1;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = st.P1;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)st.P2, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = st.P2;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = st.P2;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)st.P3, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = st.P3;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = st.P3;
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

                    st.P1 = insidePoints[0];
                    st.P2 = intersection1;
                    st.P3 = intersection2;
                    TriangleQueue.Enqueue(st);

                    //Triangle triangle1;
                    //triangle1 = new SolidTriangle(insidePoints[0], intersection1, intersection2) { Colour = ((SolidTriangle)triangleToClip).Colour };
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d1), 1);
                    intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[1], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d2), 1);

                    //triangle1 = new SolidTriangle(insidePoints[0], intersection1, insidePoints[1]) { Colour = ((SolidTriangle)triangleToClip).Colour };
                    st.P1 = insidePoints[0];
                    st.P2 = intersection1;
                    st.P3 = insidePoints[1];
                    var st2 = new SolidTriangle(insidePoints[1], intersection1, intersection2) { Colour = st.Colour };
                    
                    TriangleQueue.Enqueue(st);
                    TriangleQueue.Enqueue(st2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    TriangleQueue.Enqueue(st);
                    break;
            }
        }

        void ClipTextureTriangle(TextureTriangle tt, Vector3D planePoint, Vector3D planeNormal)
        {
            Vector3D[] insideTexturePoints = new Vector3D[3], outsideTexturePoints = new Vector3D[3];

            // Determine what vertices of the TextureTriangle are inside and outside.
            if (PointDistanceFromPlane((Vector3D)tt.P1, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = tt.P1;
                insideTexturePoints[insidePointCount] = tt.T1;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = tt.P1;
                outsideTexturePoints[outsidePointCount] = tt.T1;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)tt.P2, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = tt.P2;
                insideTexturePoints[insidePointCount] = tt.T2;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = tt.P2;
                outsideTexturePoints[outsidePointCount] = tt.T2;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane((Vector3D)tt.P3, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = tt.P3;
                insideTexturePoints[insidePointCount] = tt.T3;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = tt.P3;
                outsideTexturePoints[outsidePointCount] = tt.T3;
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
                    
                    Vector3D tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                    Vector3D tIntersection2 = (outsideTexturePoints[1] - insideTexturePoints[0]) * d2 + insideTexturePoints[0];

                    //var triangle1 = new TextureTriangle(insidePoints[0], intersection1, intersection2, insideTexturePoints[0], tIntersection1, tIntersection2, tt.TextureObject);

                    tt.P1 = insidePoints[0];
                    tt.P2 = intersection1;
                    tt.P3 = insideTexturePoints[0];
                    tt.T1 = insideTexturePoints[0];
                    tt.T2 = tIntersection1;
                    tt.T3 = tIntersection2; 

                    TriangleQueue.Enqueue(tt);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection1 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[0], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d1), 1);
                    intersection2 = new Vector4D(LineIntersectPlane((Vector3D)insidePoints[1], (Vector3D)outsidePoints[0], planePoint, planeNormal, out d2), 1);
                    
                    tIntersection1 = (outsideTexturePoints[0] - insideTexturePoints[0]) * d1 + insideTexturePoints[0];
                    tIntersection2 = (outsideTexturePoints[0] - insideTexturePoints[1]) * d2 + insideTexturePoints[1];

                    //triangle1 = new TextureTriangle(insidePoints[0], intersection1, insidePoints[1], insideTexturePoints[0], tIntersection1, insideTexturePoints[1], ((TextureTriangle)triangleToClip).TextureObject);

                    tt.P1 = insidePoints[0];
                    tt.P2 = intersection1;
                    tt.P3 = insidePoints[1];
                    tt.T1 = insideTexturePoints[0];
                    tt.T2 = tIntersection1;
                    tt.T3 = insideTexturePoints[1];
                    var tt2 = new TextureTriangle(insidePoints[1], intersection1, intersection2, insideTexturePoints[1], tIntersection1, tIntersection2, tt.TextureObject);

                    TriangleQueue.Enqueue(tt);
                    TriangleQueue.Enqueue(tt2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    TriangleQueue.Enqueue(tt);
                    break;
            }
        }
    
        void ClipGradientTriangle(GradientTriangle gt, Vector3D planePoint, Vector3D planeNormal)
        {

        }
    }

    #endregion

    // Triangles
    internal static bool ClipTriangles(Queue<VectorTriple<Vector4D>> triangleQueue, ClippingPlane[] clippingPlanes)
    {
        

        return triangleQueue.Count > 0;
    }
}

//internal static class Clipping
//{
    // check clockwise/anticlockwise stuff
    // source (for everything in file)
//}