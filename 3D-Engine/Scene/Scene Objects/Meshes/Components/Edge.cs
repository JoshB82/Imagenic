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
        internal Vertex Model_P1;
        internal Vertex Model_P2;

        internal Vector4D P1;
        internal Vector4D P2;

        // Appearance
        public Color Colour { get; set; } = Properties.Settings.Default.Edge_Colour;
        public bool Visible { get; set; } = true;

        #endregion

        internal void Reset_Vertices()
        {
            P1 = Model_P1.Point;
            P2 = Model_P2.Point;
        }
        internal void Apply_Matrix(Matrix4x4 matrix)
        {
            P1 = matrix * P1;
            P2 = matrix * P2;
        }

        #region Constructors

        internal Edge() { }

        /// <summary>
        /// Creates an <see cref="Edge"/>.
        /// </summary>
        /// <param name="model_p1">The position of the first point on the <see cref="Edge"/>.</param>
        /// <param name="model_p2">The position of the second point on the <see cref="Edge"/>.</param>
        public Edge(Vertex model_p1, Vertex model_p2)
        {
            Model_P1 = model_p1; Model_P2 = model_p2;
        }

        #endregion
    }
}