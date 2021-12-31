using _3D_Engine.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace _3D_Engine.Utilities;

public class EventList<T> : Entity, IList<T>
{
    #region Fields and Properties

    private IList<T> internalList = new List<T>();
    public bool ActivateRenderingEvent { get; set; } = true;
    public bool ActivateShadowMapEvent { get; set; } = true;

    public T this[int index]
    {
        get => internalList[index];
        set
        {
            internalList[index] = value;
            InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
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
        (internalList as List<T>).AddRange(range);
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
        InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
    }

    public void Clear()
    {
        internalList.Clear();
        InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
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
        InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
    }

    public bool Remove(T item)
    {
        InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
        return internalList.Remove(item);
    }

    public void RemoveAt(int index)
    {
        internalList.RemoveAt(index);
        InvokeRenderingEvents(ActivateRenderingEvent, ActivateShadowMapEvent);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)internalList).GetEnumerator();
    }

    #endregion
}