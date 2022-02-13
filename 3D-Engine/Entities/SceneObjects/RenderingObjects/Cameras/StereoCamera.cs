using static _3D_Engine.Properties.Settings;
using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;

namespace Imagenic.Core.Entities.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="StereoCamera"/>.
    /// </summary>
    public sealed class StereoCamera : Camera
    {
        #region Constructors

        public StereoCamera(Vector3D worldOrigin, Orientation worldOrientation, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar) { }

        #endregion

        #region Methods

        internal override void ProcessLighting(Group sceneToRender)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}