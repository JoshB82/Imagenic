using System;

namespace Imagenic.Core.Entities;

public abstract class Transition : Entity { }

public class Transition<TEntity, TValue> : Transition where TEntity : Entity
{
    #region Fields and Parameters

    public TValue Start { get; set; }

    public TValue End { get; set; }

    public float TimeStart { get; set; }

    public float TimeEnd { get; set; }

    internal Action<TEntity, TValue> Transformation { get; set; }

    #endregion

    #region Constructors

    public Transition(TValue start, TValue end, float timeStart, float timeEnd, Action<TEntity, TValue> transformation)
    {
        Start = start;
        End = end;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        Transformation = transformation;
    }

    #endregion
}

public static class TransitionExtensions
{
    #region Methods

    public static Transition<T, int> ToTransition<T>(this Range range, float timeStart, float timeEnd, Action<T, int> transformation) where T : Entity
    {
        return new(range.Start.Value, range.End.Value + 1, timeStart, timeEnd, transformation);
    }

    #endregion
}