using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public sealed class AmbientLight : Light
{
    #region Constructors

    public AmbientLight(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation) : base(worldOrigin, worldOrientation)
    {

    }

    #endregion
}