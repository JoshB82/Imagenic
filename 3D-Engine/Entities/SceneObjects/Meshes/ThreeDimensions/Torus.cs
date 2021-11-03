/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a torus mesh.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Torus"/> mesh.
    /// </summary>
    public sealed class Torus : Mesh
    {
        #region Fields and Properties

        public float InnerRadius { get; set; }

        public float OuterRadius { get; set; }

        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Torus(Vector3D worldOrigin,
                     Orientation worldOrientation,
                     float radius,
                     float innerRadius,
                     float outerRadius) : base(worldOrigin, worldOrientation, 3)
        {

        }

        #endregion
    }
}