using Imagenic.Core.Utilities;
using Imagenic.Core.Utilities.Messages;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

[Serializable]
public abstract class Entity
{
    #region Methods

    public virtual Entity ShallowCopy() => (Entity)MemberwiseClone();
    public virtual Entity DeepCopy()
    {
        var entity = ShallowCopy();
        entity.RenderAlteringPropertyChanged = RenderAlteringPropertyChanged;
        return entity;
    }

    #endregion


    public Entity()
    {
        MessageBuilder<EntityCreatedMessage>.Instance()
            .AddParameter(nextId)
            .Build()
            .DisplayInConsole();
    }

    //public EventList<Transition> Transitions { get; set; }

    // Id
    private static int nextId;
    /// <summary>
    /// The identification number.
    /// </summary>
    public int Id { get; } = nextId++;

    // Rendering events
    internal event Action<RenderUpdateArgs> RenderAlteringPropertyChanged;

    //internal event Action RenderAlteringPropertyChanged;
    //internal event Action ShadowMapAlteringPropertyChanged;

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