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

using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Renderers;
using System;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces
{
    public abstract class Triangle : Entity
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
}