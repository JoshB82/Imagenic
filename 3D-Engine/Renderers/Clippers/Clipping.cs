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

using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities.OrientatedEntities.RenderingEntities;
using System;
using System.Collections.Generic;


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

/*
internal sealed class TriangleClipper<TTriangle> where TTriangle : Triangle
{
    #region Fields and Properties

    

    #endregion

    #region Constructors

    

    #endregion

    #region Methods

    

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
//}*/