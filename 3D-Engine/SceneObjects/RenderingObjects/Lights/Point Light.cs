using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.RenderingObjects.Lights
{
    public sealed class PointLight : Light
    {
        #region Fields and Properties

        #endregion

        #region Constructors
        
        public PointLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength, float viewWidth, float viewHeight, float zNear, float zFar) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar) { }

        public override float ViewWidth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float ViewHeight { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float ZNear { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float ZFar { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        #endregion
    }
}