using Imagenic.Core.Renderers.Animations;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

public interface IAnimatable
{
    IEnumerable<Frame<Vector3D>> WorldOrigin { get; set; }
    IEnumerable<Frame<Orientation>> WorldOrientation { get; set; }
}

public static class IAnimatableExtensions
{
    public static T Animate<T, U>(this T animatable, Func<T, IEnumerable<Frame<U>>> selector, IEnumerable<Frame<U>> frames) where T : IAnimatable
    {
        var animatableSelection = selector(animatable);
        animatableSelection = frames;
        return animatable;
    }
}