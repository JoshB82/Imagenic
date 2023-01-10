using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public sealed class PointLight : Light
{
    #region Fields and Properties

    #endregion

    #region Constructors

    public PointLight(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float strength, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(worldOrigin, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight)
    {
        Strength = strength;
    }

    #endregion
}