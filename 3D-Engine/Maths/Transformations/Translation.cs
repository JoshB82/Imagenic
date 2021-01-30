/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for calculating matrices representing translations.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Transformations
{
    public static partial class Transform
    {
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Translate_X(System.Single)']/*"/>
        public static Matrix4x4 TranslateX(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distance;
            return translation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Translate_Y(System.Single)']/*"/>
        public static Matrix4x4 TranslateY(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m13 = distance;
            return translation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Translate_Z(System.Single)']/*"/>
        public static Matrix4x4 TranslateZ(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m23 = distance;
            return translation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Translate(System.Single,System.Single,System.Single)']/*"/>
        public static Matrix4x4 Translate(float distanceX, float distanceY, float distanceZ)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distanceX;
            translation.m13 = distanceY;
            translation.m23 = distanceZ;
            return translation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Translate(_3D_Engine.Vector3D)']/*"/>
        public static Matrix4x4 Translate(Vector3D displacement)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = displacement.x;
            translation.m13 = displacement.y;
            translation.m23 = displacement.z;
            return translation;
        }
    }
}