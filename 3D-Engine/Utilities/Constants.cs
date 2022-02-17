/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines frequently used constants.
 */

namespace Imagenic.Core.Utilities
{
    /// <summary>
    /// Encapsulates frequently used <see cref="Constants"/>.
    /// </summary>
    public static class Constants
    {
        public const string ProjectName = "Imagenic.Core";

        #region Physics

        // Gravitational Acceleration
        /// <summary>
        /// Gravitational acceleration near the surface of the Earth as a <see cref="float"/>.
        /// </summary>
        public const float GravAcc = -9.81f;
        /// <summary>
        /// Gravitational acceleration near the surface of the Earth as a <see cref="Vector3D"/>.
        /// </summary>
        public static readonly Vector3D GravAccVector = new(0, GravAcc, 0);

        #endregion
    }
}