using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of an <see cref="Edge"/>.
    /// </summary>
    public sealed class Edge
    {
        #region Fields and Properties

        // Positions
        internal Vertex P1 { get; set; }
        internal Vertex P2 { get; set; }
        /// <summary>
        /// The position of the first point of the <see cref="Edge"/> in world space.
        /// </summary>
        public Vector3D World_P1 { get; internal set; }
        /// <summary>
        /// The position of the second point of the <see cref="Edge"/> in world space.
        /// </summary>
        public Vector3D World_P2 { get; internal set; }

        // Appearance
        public Color Colour { get; set; } = Settings.Default_Edge_Colour;
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an <see cref="Edge"/>.
        /// </summary>
        /// <param name="p1">The position of the first point on the <see cref="Edge"/>.</param>
        /// <param name="p2">The position of the second point on the <see cref="Edge"/>.</param>
        public Edge(Vertex p1, Vertex p2)
        {
            P1 = p1; P2 = p2;
            Visible = true;
        }

        #endregion
    }
}