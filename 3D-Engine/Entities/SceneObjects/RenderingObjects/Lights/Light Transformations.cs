using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights
{
    public abstract partial class Light : RenderingObject
    {
        public override void SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            base.SetDirection1(newWorldDirectionForward, newWorldDirectionUp);

            //CreateShadowMap
        }
    }
}