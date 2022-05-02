/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 *
 */

using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Renderers;
using Imagenic.Core.Maths;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles
{
    public abstract class Triangle : SceneEntity
    {
        #region Fields and Properties

        internal List<RendererBase> Renderers { get; set; }

        // Appearance
        public bool DrawOutline { get; set; } = false;
        public bool Visible { get; set; } = true;

        // Model space values
        public Vertex ModelP1 { get; set; }
        public Vertex ModelP2 { get; set; }
        public Vertex ModelP3 { get; set; }

        // Calculation values
        internal Vector4D P1 { get; set; }
        internal Vector4D P2 { get; set; }
        internal Vector4D P3 { get; set; }

        #endregion

        #region Methods

        internal void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);
        internal abstract void Interpolator(RenderingObject renderingObject, Action<object, int, int, float> bufferAction);
        internal abstract void ResetVertices();

        #endregion
    }

    public struct PlanePoints
    {
        public Vector3D P1 { get; private set; }
        public Vector3D P2 { get; private set; }
        public Vector3D P3 { get; private set; }

        public PlanePoints()
        {
            P1 = Vector3D.Zero;
            P2 = Vector3D.One;
            P3 = Vector3D.UnitX;
        }

        public PlanePoints(Vector3D p1, Vector3D p2, Vector3D p3)
        {
            if (p1 == p2 || p2 == p3 || p1 == p3)
            {
                // Cannot define plane, therefore throw exception
            }

            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        //internal void ApplyMatrix(Matrix4x4 matrix) => (P1, P2, P3) = (matrix * P1, matrix * P2, matrix * P3);
    }
}