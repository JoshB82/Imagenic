using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// Encapsulates creation of a <see cref="StereoCamera"/>.
/// </summary>
public sealed class StereoCamera : Camera
{
    #region Constructors

    public StereoCamera(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar) { }

    #endregion

    #region Methods

    /*
    internal override void ProcessLighting(Group sceneToRender)
    {
        throw new System.NotImplementedException();
    }
    */

    #endregion
}