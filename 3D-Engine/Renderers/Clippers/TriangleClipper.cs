using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using Imagenic.Core.Entities;
using System.Collections.Generic;
using System;
using static Imagenic.Core.Maths.Vectors.Vector3D;

namespace Imagenic.Core.Renderers.Clippers;

internal class TriangleClipper
{
    #region Fields and Properties

    internal ClippingPlane[] ClippingPlanes { get; set; }
    internal Queue<Triangle> TriangleQueue { get; }

    #endregion

    #region Constructors

    internal TriangleClipper(Triangle startingTriangle, ClippingPlane[] clippingPlanes)
    {
        ClippingPlanes = clippingPlanes;
        TriangleQueue = new Queue<Triangle>();
        TriangleQueue.Enqueue(startingTriangle);
    }

    internal TriangleClipper(Queue<Triangle> triangleQueue, ClippingPlane[] clippingPlanes)
    {
        ClippingPlanes = clippingPlanes;
        TriangleQueue = triangleQueue;
    }

    #endregion

    #region Methods

    internal Queue<Triangle> Clip()
    {
        Vector3D[] insidePoints = new Vector3D[3], outsidePoints = new Vector3D[3];
        int insidePointCount = 0, outsidePointCount = 0;

        var triangle = TriangleQueue.Dequeue();

        if (triangle.FrontStyle is TextureStyle frontStyle && triangle.BackStyle is TextureStyle backStyle)
        {
            foreach (ClippingPlane clippingPlane in ClippingPlanes)
            {
                int noTrianglesRemaining = TriangleQueue.Count;
                while (noTrianglesRemaining-- > 0) ClipDoubleTextureStyle(triangle, frontStyle, backStyle, clippingPlane.Point, clippingPlane.Normal);
            }
            return TriangleQueue;
        }
        if (triangle.FrontStyle is TextureStyle textureStyle && triangle.BackStyle is SolidStyle solidStyle)
        {
            foreach (ClippingPlane clippingPlane in ClippingPlanes)
            {
                int noTrianglesRemaining = TriangleQueue.Count;
                while (noTrianglesRemaining-- > 0) ClipTextureSolidStyle(textureStyle, clippingPlane.Point, clippingPlane.Normal);
            }
        }
        if (triangle.FrontStyle is SolidStyle frontSolidStyle && triangle.BackStyle is TextureStyle backTextureStyle)
        {
            foreach (ClippingPlane clippingPlane in ClippingPlanes)
            {
                int noTrianglesRemaining = TriangleQueue.Count;
                while (noTrianglesRemaining-- > 0) ClipSolidTriangle(solidTriangleQueue.Dequeue(), clippingPlane.Point, clippingPlane.Normal);
            }
        }

        

        

        //if (TriangleQueue.Count > 0)
        //{

        //}
        

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

        void ClipDoubleTextureStyle(Triangle triangle, TextureStyle frontTextureStyle, TextureStyle backTextureStyle, Vector3D planePoint, Vector3D planeNormal)
        {
            Vector3D[] insideFrontTexturePoints = new Vector3D[3], outsideFrontTexturePoints = new Vector3D[3];
            Vector3D[] insideBackTexturePoints = new Vector3D[3], outsideBackTexturePoints = new Vector3D[3];

            // Determine what vertices of the TextureStyle are inside and outside.
            if (PointDistanceFromPlane(triangle.P1.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P1.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T1;
                insideBackTexturePoints[insidePointCount] = backTextureStyle.T1;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P1.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T1;
                outsideBackTexturePoints[outsidePointCount] = backTextureStyle.T1;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane(triangle.P2.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P2.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T2;
                insideBackTexturePoints[insidePointCount] = backTextureStyle.T2;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P2.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T2;
                outsideBackTexturePoints[outsidePointCount] = backTextureStyle.T2;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane(triangle.P3.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P3.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T3;
                insideBackTexturePoints[insidePointCount] = backTextureStyle.T3;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P3.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T3;
                outsideBackTexturePoints[outsidePointCount] = backTextureStyle.T3;
            }

            switch (insidePointCount)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    Vector3D intersection1 = LineIntersectPlane(insidePoints[0], outsidePoints[0], planePoint, planeNormal, out float d1);
                    Vector3D intersection2 = LineIntersectPlane(insidePoints[0], outsidePoints[1], planePoint, planeNormal, out float d2);

                    Vector3D tFrontIntersection1 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[0]) * d1 + insideFrontTexturePoints[0];
                    Vector3D tFrontIntersection2 = (outsideFrontTexturePoints[1] - insideFrontTexturePoints[0]) * d2 + insideFrontTexturePoints[0];

                    Vector3D tBackIntersection1 = (outsideBackTexturePoints[0] - insideBackTexturePoints[0]) * d1 + insideBackTexturePoints[0];
                    Vector3D tBackIntersection2 = (outsideBackTexturePoints[1] - insideBackTexturePoints[0]) * d2 + insideBackTexturePoints[0];

                    //var triangle1 = new TextureTriangle(insidePoints[0], intersection1, intersection2, insideTexturePoints[0], tIntersection1, tIntersection2, tt.TextureObject);

                    triangle.P1.WorldOrigin = insidePoints[0];
                    triangle.P2.WorldOrigin = intersection1;
                    triangle.P3.WorldOrigin = intersection2;
                    frontTextureStyle.T1 = insideFrontTexturePoints[0];
                    frontTextureStyle.T2 = tFrontIntersection1;
                    frontTextureStyle.T3 = tFrontIntersection2;
                    backTextureStyle.T1 = insideBackTexturePoints[0];
                    backTextureStyle.T2 = tBackIntersection1;
                    backTextureStyle.T3 = tBackIntersection2;

                    TriangleQueue.Enqueue(triangle);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection1 = LineIntersectPlane(insidePoints[0], outsidePoints[0], planePoint, planeNormal, out d1);
                    intersection2 = LineIntersectPlane(insidePoints[1], outsidePoints[0], planePoint, planeNormal, out d2);

                    tFrontIntersection1 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[0]) * d1 + insideFrontTexturePoints[0];
                    tFrontIntersection2 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[1]) * d2 + insideFrontTexturePoints[1];

                    tBackIntersection1 = (outsideBackTexturePoints[0] - insideBackTexturePoints[0]) * d1 + insideBackTexturePoints[0];
                    tBackIntersection2 = (outsideBackTexturePoints[0] - insideBackTexturePoints[1]) * d2 + insideBackTexturePoints[1];

                    //triangle1 = new TextureTriangle(insidePoints[0], intersection1, insidePoints[1], insideTexturePoints[0], tIntersection1, insideTexturePoints[1], ((TextureTriangle)triangleToClip).TextureObject);

                    triangle.P1.WorldOrigin = insidePoints[0];
                    triangle.P2.WorldOrigin = intersection1;
                    triangle.P3.WorldOrigin = insidePoints[1];
                    frontTextureStyle.T1 = insideFrontTexturePoints[0];
                    frontTextureStyle.T2 = tFrontIntersection1;
                    frontTextureStyle.T3 = insideFrontTexturePoints[1];
                    backTextureStyle.T1 = insideBackTexturePoints[0];
                    backTextureStyle.T2 = tBackIntersection1;
                    backTextureStyle.T3 = insideBackTexturePoints[1];

                    var frontTextureStyle2 = new TextureStyle(frontTextureStyle.DisplayTexture, insideFrontTexturePoints[1], tFrontIntersection1, tFrontIntersection2);
                    var backTextureStyle2 = new TextureStyle(backTextureStyle.DisplayTexture, insideBackTexturePoints[1], tBackIntersection1, tBackIntersection2);

                    var triangle2 = new Triangle(frontTextureStyle2, backTextureStyle2, new Vertex(insidePoints[1]), new Vertex(intersection1), new Vertex(intersection2));

                    TriangleQueue.Enqueue(triangle);
                    TriangleQueue.Enqueue(triangle2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    TriangleQueue.Enqueue(triangle);
                    break;
            }
        }

        void ClipTextureSolidStyle(Triangle triangle, TextureStyle frontTextureStyle, SolidStyle backSolidStyle, Vector3D planePoint, Vector3D planeNormal)
        {
            Vector3D[] insideFrontTexturePoints = new Vector3D[3], outsideFrontTexturePoints = new Vector3D[3];

            // Determine what vertices of the TextureStyle are inside and outside.
            if (PointDistanceFromPlane(triangle.P1.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P1.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T1;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P1.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T1;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane(triangle.P2.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P2.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T2;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P2.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T2;
                outsidePointCount++;
            }

            if (PointDistanceFromPlane(triangle.P3.WorldOrigin, planePoint, planeNormal) >= 0)
            {
                insidePoints[insidePointCount] = triangle.P3.WorldOrigin;
                insideFrontTexturePoints[insidePointCount] = frontTextureStyle.T3;
                insidePointCount++;
            }
            else
            {
                outsidePoints[outsidePointCount] = triangle.P3.WorldOrigin;
                outsideFrontTexturePoints[outsidePointCount] = frontTextureStyle.T3;
            }

            switch (insidePointCount)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    Vector3D intersection1 = LineIntersectPlane(insidePoints[0], outsidePoints[0], planePoint, planeNormal, out float d1);
                    Vector3D intersection2 = LineIntersectPlane(insidePoints[0], outsidePoints[1], planePoint, planeNormal, out float d2);

                    Vector3D tFrontIntersection1 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[0]) * d1 + insideFrontTexturePoints[0];
                    Vector3D tFrontIntersection2 = (outsideFrontTexturePoints[1] - insideFrontTexturePoints[0]) * d2 + insideFrontTexturePoints[0];

                    //var triangle1 = new TextureTriangle(insidePoints[0], intersection1, intersection2, insideTexturePoints[0], tIntersection1, tIntersection2, tt.TextureObject);

                    triangle.P1.WorldOrigin = insidePoints[0];
                    triangle.P2.WorldOrigin = intersection1;
                    triangle.P3.WorldOrigin = intersection2;
                    frontTextureStyle.T1 = insideFrontTexturePoints[0];
                    frontTextureStyle.T2 = tFrontIntersection1;
                    frontTextureStyle.T3 = tFrontIntersection2;

                    TriangleQueue.Enqueue(triangle);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection1 = LineIntersectPlane(insidePoints[0], outsidePoints[0], planePoint, planeNormal, out d1);
                    intersection2 = LineIntersectPlane(insidePoints[1], outsidePoints[0], planePoint, planeNormal, out d2);

                    tFrontIntersection1 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[0]) * d1 + insideFrontTexturePoints[0];
                    tFrontIntersection2 = (outsideFrontTexturePoints[0] - insideFrontTexturePoints[1]) * d2 + insideFrontTexturePoints[1];

                    //triangle1 = new TextureTriangle(insidePoints[0], intersection1, insidePoints[1], insideTexturePoints[0], tIntersection1, insideTexturePoints[1], ((TextureTriangle)triangleToClip).TextureObject);

                    triangle.P1.WorldOrigin = insidePoints[0];
                    triangle.P2.WorldOrigin = intersection1;
                    triangle.P3.WorldOrigin = insidePoints[1];
                    frontTextureStyle.T1 = insideFrontTexturePoints[0];
                    frontTextureStyle.T2 = tFrontIntersection1;
                    frontTextureStyle.T3 = insideFrontTexturePoints[1];

                    var frontTextureStyle2 = new TextureStyle(frontTextureStyle.DisplayTexture, insideFrontTexturePoints[1], tFrontIntersection1, tFrontIntersection2);
                    var backSolidStyle2 = new SolidStyle(backSolidStyle.Colour);

                    var triangle2 = new Triangle(frontTextureStyle2, backSolidStyle2, new Vertex(insidePoints[1]), new Vertex(intersection1), new Vertex(intersection2));

                    TriangleQueue.Enqueue(triangle);
                    TriangleQueue.Enqueue(triangle2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    TriangleQueue.Enqueue(triangle);
                    break;
            }
        }

        void ClipSolidTextureStyle(Triangle triangle, SolidStyle frontSolidStyle, TextureStyle backTextureStyle, Vector3D planePoint, Vector3D planeNormal)
        {

        }

        void ClipGradientTriangle(GradientTriangle gt, Vector3D planePoint, Vector3D planeNormal)
        {

        }
    }

    #endregion
}