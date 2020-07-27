namespace _3D_Engine
{
    public abstract partial class Mesh
    {
        #region Scaling

        public void Scale_X(double scale_factor) => Scaling = new Vector3D(Scaling.X * scale_factor, 0, 0);
        public void Scale_Y(double scale_factor) => Scaling = new Vector3D(0, Scaling.Y * scale_factor, 0);
        public void Scale_Z(double scale_factor) => Scaling = new Vector3D(0, 0, Scaling.Z * scale_factor);
        public void Scale(double scale_factor_x, double scale_factor_y, double scale_factor_z) => Scaling = new Vector3D(Scaling.X * scale_factor_x, Scaling.Y * scale_factor_y, Scaling.Z * scale_factor_z);
        public void Scale(double scale_factor) => Scaling = new Vector3D(Scaling.X * scale_factor, Scaling.Y * scale_factor, Scaling.Z * scale_factor);

        #endregion
    }
}