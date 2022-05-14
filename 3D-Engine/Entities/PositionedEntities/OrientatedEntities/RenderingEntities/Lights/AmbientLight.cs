using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;

namespace Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.RenderingEntities.Lights;

public sealed class AmbientLight : Light
{
    #region Constructors

    public AmbientLight(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation)
    {

    }

    #endregion
}