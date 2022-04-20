using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Maths;

namespace Imagenic.Core.Entities.SceneObjects.RenderingObjects.Lights
{
    public sealed class AmbientLight : Light
    {
        #region Constructors

        public AmbientLight(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation)
        {

        }

        #endregion
    }
}