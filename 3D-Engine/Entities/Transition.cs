using System;

namespace Imagenic.Core.Entities;

public abstract class Transition : Entity { }

public class Transition<T> : Transition
{
    #region Fields and Parameters

    public T Start { get; set; }

    public T End { get; set; }

    public float TimeStart { get; set; }

    public float TimeEnd { get; set; }

    internal Func<Entity, T> PropertySelector { get; set; }

    #endregion

    #region Constructors

    public Transition(T start, T end, float timeStart, float timeEnd)
    {
        Start = start;
        End = end;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
    }

    #endregion

    #region Methods

    public static Transition<int> ToTransition(Range range, float timeStart, float timeEnd) => new(range.Start.Value, range.End.Value + 1, timeStart, timeEnd);

    #endregion
}