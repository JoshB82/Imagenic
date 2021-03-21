/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines static methods for calculating direction vectors.
 */

using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Transformations
{
    /// <summary>
    /// Provides static methods for calculating notable vectors, matrices and quaternions.
    /// </summary>
    public static partial class Transform
    {
        /// <summary>
        /// Calculates the forward direction given the up direction and the right direction.
        /// </summary>
        /// <param name="directionUp">The up direction.</param>
        /// <param name="directionRight">The right direction.</param>
        /// <returns>The forward direction.</returns>
        public static Vector3D CalculateDirectionForward(Vector3D directionUp, Vector3D directionRight) => directionRight.CrossProduct(directionUp);

        /// <summary>
        /// Calculates the up direction given the right direction and the forward direction.
        /// </summary>
        /// <param name="directionRight">The right direction.</param>
        /// <param name="directionForward">The forward direction.</param>
        /// <returns>The up direction.</returns>
        public static Vector3D CalculateDirectionUp(Vector3D directionRight, Vector3D directionForward) => directionForward.CrossProduct(directionRight);

        /// <summary>
        /// Calculates the right direction given the forward direction and the up direction.
        /// </summary>
        /// <param name="directionForward">The forward direction.</param>
        /// <param name="directionUp">The up direction.</param>
        /// <returns>The right direction.</returns>
        public static Vector3D CalculateDirectionRight(Vector3D directionForward, Vector3D directionUp) => directionUp.CrossProduct(directionForward);
    }
}