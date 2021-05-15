using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.RenderingObjects.Lights
{
    public sealed class PointLight : Light
    {
        #region Fields and Properties

        #endregion

        #region Constructors

        public PointLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight)
        {
            Strength = strength;
        }

        #endregion
    }
}