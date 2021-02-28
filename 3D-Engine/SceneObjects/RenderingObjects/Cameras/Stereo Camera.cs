using _3D_Engine.Maths.Vectors;

using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="StereoCamera"/>.
    /// </summary>
    public sealed class StereoCamera : Camera
    {
        #region Constructors

        public StereoCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        #endregion

        #region Methods

        internal override void ProcessLighting()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}