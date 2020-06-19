using System.Diagnostics;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Torus"/> mesh.
    /// </summary>
    public sealed class Torus
    {
        #region Fields and Properties

        public double Inner_Radius { get; set; }
        public double Outer_Radius { get; set; }

        #endregion

        #region Constructors

        public Torus(Vector3D origin, Vector3D direction, Vector3D direction_up, double radius, double inner_radius, double outer_radius)
        {

        }

        #endregion
    }
}
