namespace _3D_Engine
{
    /// <include file="Comments.xml" path="doc/members/member[@name='T:_3D_Engine.Constants']/*"/>
    public static class Constants
    {
        #region Physics
        
        // Gravitational Acceleration
        /// <include file="Comments.xml" path="doc/members/member[@name='F:_3D_Engine.Constants.Grav_Acc']/*"/>
        public const double Grav_Acc = -9.81;
        public static Vector3D Grav_Acc_Vector = new Vector3D(0, Constants.Grav_Acc, 0);

        #endregion
    }
}