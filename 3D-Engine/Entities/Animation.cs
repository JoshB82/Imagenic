using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

public sealed class Animation
{
    public bool IsActive { get; set; } = true;

    public IEnumerable<Frame> Frames { get; set; }

    public Animation(IEnumerable<Frame> frames)
    {
        Frames = frames;
    }
}

public abstract class Frame
{
    public bool IsActive { get; set; } = true;
}

public sealed class Frame<T> : Frame
{
    public Func<IAnimatable, T> Selector { get; set; }
    public T Value { get; set; }
    public float Time { get; set; }

    public Frame(Func<IAnimatable, T> selector, T value, float time)
    {
        Selector = selector;
        Value = value;
        Time = time;
    }
}