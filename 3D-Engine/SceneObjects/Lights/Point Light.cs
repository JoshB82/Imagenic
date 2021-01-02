using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.Lights
{
    public sealed class PointLight : Light
    {
        #region Fields and Properties

        #endregion

        #region Constructors
        
        public PointLight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : base(origin, direction_forward, direction_up)
        {
            
        }

        public override int ShadowMapWidth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override int ShadowMapHeight { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float ShadowMapZNear { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float Shadow_Map_Z_Far { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        #endregion
    }
}