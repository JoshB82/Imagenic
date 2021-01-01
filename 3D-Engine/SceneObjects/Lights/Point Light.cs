using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.Lights
{
    public sealed class Point_Light : Light
    {
        #region Fields and Properties

        #endregion

        #region Constructors
        
        public Point_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : base(origin, direction_forward, direction_up)
        {
            
        }

        public override int Shadow_Map_Width { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override int Shadow_Map_Height { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float Shadow_Map_Z_Near { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public override float Shadow_Map_Z_Far { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        #endregion
    }
}