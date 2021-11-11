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

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class Text2D : Mesh
    {
        #region Fields and Properties

        public IEnumerable<string> Fonts { get; set; }

        private float size;
        public float Size
        {
            get => size;
            set
            {
                if (value == size) return;
                size = value;
                RequestNewRenders();
            }
        }

        private char style;
        public char Style
        {
            get => style;
            set
            {
                if (value == style) return;
                style = value;
                RequestNewRenders();
            }
        }

        private string content;
        public string Content
        {
            get => content;
            set
            {
                if (value == content) return;
                content = value;
                RequestNewRenders();
            }
        }

        #endregion

        #region Constructors

        public Text2D(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      IEnumerable<string> fonts,
                      float size,
                      char style,
                      string content) : base(worldOrigin, worldOrientation, 2)
        {
            Fonts = fonts;
            Size = size;
            Style = style;
            Content = content;
        }

        #endregion
    }
}