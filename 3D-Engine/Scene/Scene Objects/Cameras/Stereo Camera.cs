using static _3D_Engine.Properties.Settings;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Stereo_Camera"/>.
    /// </summary>
    public sealed class Stereo_Camera : Camera
    {
        #region Fields and Properties

        private float width = Default.Camera_Width;
        private float height = Default.Camera_Height;
        private float z_near = Default.Camera_Z_Near;
        private float z_far = Default.Camera_Z_Far;

        public override float Width { get; set; }
        public override float Height { get; set; }
        public override float Z_Near { get; set; }
        public override float Z_Far { get; set; }

        #endregion

        #region Constructors

        public Stereo_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up)
        {
            
        }

        #endregion
    }
}
