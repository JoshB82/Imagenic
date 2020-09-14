namespace _3D_Engine
{
    /// <include file="Help_5.xml" path="doc/members/member[@name='T:_3D_Engine.Constants']/*"/>
    public static class Constants
    {
        #region Physics
        
        // Gravitational Acceleration
        /// <include file="Help_5.xml" path="doc/members/member[@name='F:_3D_Engine.Constants.Grav_Acc']/*"/>
        public const float Grav_Acc = -9.81f;
        public static Vector3D Grav_Acc_Vector = new Vector3D(0, Grav_Acc, 0);

        #endregion
    }
}