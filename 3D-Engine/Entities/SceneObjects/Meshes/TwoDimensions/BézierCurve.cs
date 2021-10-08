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

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class BézierCurve : Mesh
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public BézierCurve(Vector3D origin,
                           Vector3D directionForward,
                           Vector3D directionUp) : base(origin, directionForward, directionUp, 2)
        {

        }

        #endregion
    }
}