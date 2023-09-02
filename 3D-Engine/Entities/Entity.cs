using Imagenic.Core.Enums;
using Imagenic.Core.Utilities.Messages2;
using System;

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

    #if DEBUG

    //private protected virtual IMessageBuilder<EntityCreatedMessage>? MessageBuilder { get; }

    private protected MessageBuilder2<EntityCreatedMessage2> mb = new();

    #endif

    #endregion

    #region Constructors

    /*
    private protected Entity(
        #if DEBUG
        IMessageBuilder<EntityCreatedMessage> mb
        #endif
        )
    {
        #if DEBUG
        MessageBuilder = mb.AddParameter(nextId);
        #endif
    }

    /*protected Entity()
    {
        /*MessageBuilder = MessageBuilder<EntityCreatedMessage>.Instance()
                                                             .AddParameter(nextId)
                                                             .Build()
                                                             .DisplayInConsole();*/
    
    //}

    protected Entity()
    {
        #if DEBUG

        mb.AddParameter("ID", Id);

        #endif
    }

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
}