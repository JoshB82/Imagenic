using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Meshes.ThreeDimensions.Cuboids;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.TranslatableEntities.OrientatedEntities.PhysicalEntities.Meshes.ThreeDimensions.Cuboids;

public sealed class AnimatedCube : Cube, IAnimatable
{
    #region Fields and Properties

    private IEnumerable<Frame<Vector3D>> worldOrigin;
    public new IEnumerable<Frame<Vector3D>> WorldOrigin
    {
        get => worldOrigin;
        set
        {
            worldOrigin = value;
        }
    }

    private IEnumerable<Frame<Orientation>> worldOrientation;
    public new IEnumerable<Frame<Orientation>> WorldOrientation
    {
        get => worldOrientation;
        set
        {
            worldOrientation = value;
        }
    }

    private IEnumerable<Frame<float>> sideLength;
    public new IEnumerable<Frame<float>> SideLength
    {
        get => sideLength;
        set
        {
            sideLength = value;
        }
    }

    #endregion

    public AnimatedCube(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float sideLength) : base(worldOrigin, worldOrientation, sideLength)
    {

    }
}