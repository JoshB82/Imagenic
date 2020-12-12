/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates frequently used constants.
 */

using _3D_Engine.Maths.Vectors;

namespace _3D_Engine
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Constants']/*"/>
    public static class Constants
    {
        #region Physics
        
        // Gravitational Acceleration
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Constants.Grav_Acc']/*"/>
        public const float Grav_Acc = -9.81f;
        /// <summary>
        /// Gravitational acceleration near the surface of the Earth as a <see cref="Vector3D"/>.
        /// </summary>
        public static Vector3D Grav_Acc_Vector = new Vector3D(0, Grav_Acc, 0);

        #endregion
    }
}