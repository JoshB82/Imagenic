using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

[Serializable]
public abstract class Entity
{
    public EventList<Transition> Transitions { get; set; }

    // Id
    private static int nextId;
    /// <summary>
    /// The identification number.
    /// </summary>
    public int Id { get; } = nextId++;

    // Rendering events
    internal event Action RenderAlteringPropertyChanged;
    internal event Action ShadowMapAlteringPropertyChanged;

    internal void InvokeRenderingEvents(bool renderEvent = true, bool shadowMapEvent = true)
    {
        if (renderEvent)
        {
            RenderAlteringPropertyChanged?.Invoke();
        }
        if (shadowMapEvent)
        {
            ShadowMapAlteringPropertyChanged?.Invoke();
        }
    }
}

public static class TransformationBuilder
{
    public static TEntity StartTransformation<TEntity>(this TEntity entity) where TEntity : Entity
    {

    }

    public static Matrix4x4 EndTransformation<TEntity>(this TEntity entity) where TEntity : Entity
    {

    }
}