/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Handles creation of an edge.
 */

using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of an <see cref="Edge"/>.
    /// </summary>
    public sealed class Edge
    {
        #region Fields and Properties

        // Vertices
        internal Vertex P1, P2;

        // Appearance
        public Color Colour { get; set; } = Properties.Settings.Default.Edge_Colour;
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an <see cref="Edge"/>.
        /// </summary>
        /// <param name="model_p1">The position of the first point on the <see cref="Edge"/>.</param>
        /// <param name="model_p2">The position of the second point on the <see cref="Edge"/>.</param>
        public Edge(Vertex model_p1, Vertex model_p2)
        {
            P1 = model_p1; P2 = model_p2;
        }

        #endregion
    }
}