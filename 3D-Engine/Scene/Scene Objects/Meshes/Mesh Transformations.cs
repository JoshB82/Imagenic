namespace _3D_Engine
{
    public abstract partial class Mesh : Scene_Object
    {
        #region Scaling

        public void Scale_X(float scale_factor) => Scaling = new Vector3D(Scaling.x * scale_factor, 0, 0);
        public void Scale_Y(float scale_factor) => Scaling = new Vector3D(0, Scaling.y * scale_factor, 0);
        public void Scale_Z(float scale_factor) => Scaling = new Vector3D(0, 0, Scaling.z * scale_factor);
        public void Scale(float scale_factor_x, float scale_factor_y, float scale_factor_z) => Scaling = new Vector3D(Scaling.x * scale_factor_x, Scaling.y * scale_factor_y, Scaling.z * scale_factor_z);
        public void Scale(float scale_factor) => Scaling = new Vector3D(Scaling.x * scale_factor, Scaling.y * scale_factor, Scaling.z * scale_factor);

        #endregion
    }
}