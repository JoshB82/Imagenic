/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for calculating direction vectors for scene objects.
 */

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates static methods for calculating notable vectors, matrices and quaternions.
    /// </summary>
    public static partial class Transform
    {
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Calculate_Direction_Forward(_3D_Engine.Vector3D,_3D_Engine.Vector3D)']/*"/>
        public static Vector3D Calculate_Direction_Forward(Vector3D direction_up, Vector3D direction_right) => direction_right.Cross_Product(direction_up);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Calculate_Direction_Up(_3D_Engine.Vector3D,_3D_Engine.Vector3D)']/*"/>
        public static Vector3D Calculate_Direction_Up(Vector3D direction_right, Vector3D direction_forward) => direction_forward.Cross_Product(direction_right);
        
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Calculate_Direction_Right(_3D_Engine.Vector3D,_3D_Engine.Vector3D)']/*"/>
        public static Vector3D Calculate_Direction_Right(Vector3D direction_forward, Vector3D direction_up) => direction_up.Cross_Product(direction_forward);
    }
}