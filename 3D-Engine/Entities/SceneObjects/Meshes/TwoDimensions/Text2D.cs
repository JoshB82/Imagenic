﻿using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class Text2D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public float Size { get; set; }
        public char Style { get; set; }

        #endregion

        #region Constructors

        public Text2D(Vector3D origin,
                      Vector3D directionForward,
                      Vector3D directionUp,
                      IEnumerable<string> fonts,
                      float size,
                      char style) : base(origin, directionForward, directionUp, 2)
        {
            Dimension = 2;
        }

        #endregion
    }
}