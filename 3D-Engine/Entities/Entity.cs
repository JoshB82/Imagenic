using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

[Serializable]
public abstract class Entity
{
    #region Fields and Properties

    // Id
    private static int nextId;
    /// <summary>
    /// The identification number.
    /// </summary>
    public int Id { get; } = nextId++;

    // Rendering events
    internal event Action<RenderUpdate>? RenderAlteringPropertyChanged;

    //internal event Action RenderAlteringPropertyChanged;
    //internal event Action ShadowMapAlteringPropertyChanged;

/*internal void InvokeRenderingEvents(bool renderEvent = true, bool shadowMapEvent = true)
{
    if (renderEvent)
    {
        RenderAlteringPropertyChanged?.Invoke();
    }
    if (shadowMapEvent)
    {
        ShadowMapAlteringPropertyChanged?.Invoke();
    }
}*/

    #if DEBUG

    private protected virtual IMessageBuilder<EntityCreatedMessage>? MessageBuilder { get; }

    #endif

    #endregion

    #region Constructors

    #if DEBUG

    private protected Entity(IMessageBuilder<EntityCreatedMessage> mb)
    {
        MessageBuilder = mb.AddParameter(nextId);
    }

    #else

    protected Entity()
    {
        /*MessageBuilder = MessageBuilder<EntityCreatedMessage>.Instance()
                                                             .AddParameter(nextId)
                                                             .Build()
                                                             .DisplayInConsole();*/
    }

    #endif

    #endregion

    #region Methods

    internal void InvokeRenderEvent(RenderUpdate args)
    {
        RenderAlteringPropertyChanged?.Invoke(args);
    }

    public virtual Entity ShallowCopy() => (Entity)MemberwiseClone();
    public virtual Entity DeepCopy()
    {
        var entity = ShallowCopy();
        entity.RenderAlteringPropertyChanged = RenderAlteringPropertyChanged;
        return entity;
    }

    #endregion

    //public EventList<Transition> Transitions { get; set; }
}