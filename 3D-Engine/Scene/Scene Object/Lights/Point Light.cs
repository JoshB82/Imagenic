namespace _3D_Engine
{
    public sealed class Point_Light : Light
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public Point_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double strength) : base(origin, direction_forward, direction_up)
        {
            
        }

        #endregion

        #region Methods

        internal override void Calculate_Light_View_Clipping_Planes(Camera camera)
        {
            double semi_width = camera.Width / 2, semi_height = camera.Height / 2, z_ratio = camera.Z_Far / camera.Z_Near;

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, camera.Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, camera.Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, camera.Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, camera.Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width * z_ratio, semi_height * z_ratio, camera.Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width * z_ratio, -semi_height * z_ratio, camera.Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_bottom_right_point, far_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_bottom_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_left_point, near_top_right_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_bottom_right_point, near_top_right_point, far_bottom_right_point); //make order look nice

            Light_View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near z
                new Clipping_Plane(far_top_left_point, Vector3D.Unit_Negative_Z), // Far z
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, top_normal), // Top
                new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }

        #endregion
    }
}
