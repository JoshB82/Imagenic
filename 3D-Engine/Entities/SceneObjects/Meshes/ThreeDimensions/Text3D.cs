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

using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public sealed class Text3D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public float Size { get; set; }
        public char Style { get; set; }
        public float Depth { get; set; }

        #endregion

        #region Constructors

        public Text3D(Vector3D origin,
                      Vector3D directionForward,
                      Vector3D directionUp,
                      IEnumerable<string> fonts,
                      float size,
                      char style,
                      float depth) : base(origin, directionForward, directionUp, 3)
        {
            Dimension = 3;


        }

        #endregion
    }
}