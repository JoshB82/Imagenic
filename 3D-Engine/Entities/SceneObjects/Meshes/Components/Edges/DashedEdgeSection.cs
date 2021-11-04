/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a dashed edge section.
 */

using System.Drawing;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges
{
    public class DashedEdgeSection
    {
        #region Fields and Properties

        public bool IsTransparent { get; set; } = false;
        public Color? Colour { get; set; }
        public float Percentage { get; set; }

        #endregion

        #region Constructors

        public DashedEdgeSection(float percentage, bool isTransparent = true, Color? colour = null)
        {
            IsTransparent = isTransparent;
            Colour = colour;
            Percentage = percentage;
        }

        #endregion
    }
}