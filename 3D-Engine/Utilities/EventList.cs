using Imagenic.Core.Entities;
using Imagenic.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Utilities;

public class EventList<T> : Entity, IList<T>
{
    #region Fields and Properties

    private IList<T> internalList = new List<T>();

    public RenderUpdate RenderUpdateStyle { get; set; } = RenderUpdate.NewRender & RenderUpdate.NewShadowMap;

    public T this[int index]
    {
        get => internalList[index];
        set
        {
            internalList[index] = value;
            InvokeRenderEvent(RenderUpdateStyle);
        }
    }

    public int Count => internalList.Count;

    public bool IsReadOnly => internalList.IsReadOnly;

    #endregion

    #region Constructors

    public EventList() { }

    public EventList(IList<T> list)
    {
        internalList = list;
    }

    #endregion

    #region Methods

    public void AddRange(IEnumerable<T> range)
    {
        ((List<T>)internalList).AddRange(range);
    }

    public void For(Action<T, int> action)
    {
        for (int i = 0; i < internalList.Count; i++)
        {
            action(internalList[i], i);
        }
    }

    public void ForEach(Action<T> action)
    {
        foreach (T item in internalList)
        {
            action(item);
        }
    }

    public void Add(T item)
    {
        internalList.Add(item);
        InvokeRenderEvent(RenderUpdateStyle);
    }

    public void Clear()
    {
        internalList.Clear();
        InvokeRenderEvent(RenderUpdateStyle);
    }

    public bool Contains(T item)
    {
        return internalList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        internalList.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return internalList.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return internalList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        internalList.Insert(index, item);
        InvokeRenderEvent(RenderUpdateStyle);
    }

    public bool Remove(T item)
    {
        InvokeRenderEvent(RenderUpdateStyle);
        return internalList.Remove(item);
    }

    public void RemoveAt(int index)
    {
        internalList.RemoveAt(index);
        InvokeRenderEvent(RenderUpdateStyle);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)internalList).GetEnumerator();
    }

    #endregion
}

public static class EventListExtensions
{
    public static EventList<T> ToEventList<T>(this IEnumerable<T> source)
    {
        return new EventList<T>(source.ToList());
    }
}