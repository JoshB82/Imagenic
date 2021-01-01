using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.Lights
{
    public abstract partial class Light : SceneObject
    {
        public override void SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            base.SetDirection1(newWorldDirectionForward, newWorldDirectionUp);

            CreateShadowMap
        }
    }
}