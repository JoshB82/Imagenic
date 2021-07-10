using System;
using System.Collections.Generic;
using System.Drawing;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces
{
    public sealed class GradientFace : Triangle
    {
        #region Fields and Properties

        public List<Color> Colours { get; set; }

        #endregion

        #region Constructors



        #endregion

        #region Methods

        internal override void Interpolator(RenderingObject renderingObject, Action<object, int, int, float> bufferAction)
        {

        }

        internal override void ResetVertices() => (P1, P2, P3) = (ModelP1.Point, ModelP2.Point, ModelP3.Point);

        #endregion
    }
}
