using Imagenic.Core.Enums;
using Imagenic.Core.Maths.Transformations;
using Imagenic.Core.Utilities.Messages;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// A <see cref="TranslatableEntity"/> that can be orientated.
/// </summary>
[Serializable]
public abstract class OrientatedEntity : TranslatableEntity
{
    #region Fields and Properties

    private Matrix4x4 rotationMatrix;

    // Orientation
    private Orientation worldOrientation;
    public virtual Orientation WorldOrientation
    {
        get => worldOrientation;
        set
        {
            if (value == worldOrientation) return;
            ThrowIfNull(value);
            worldOrientation = value;
            RegenerateRotationMatrix();
            InvokeRenderEvent(RenderUpdate.NewRender | RenderUpdate.NewShadowMap);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<OrientatedEntityCreatedMessage>? MessageBuilder => (IMessageBuilder<OrientatedEntityCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    #if DEBUG

    private protected OrientatedEntity(Vector3D worldOrigin, Orientation worldOrientation, IMessageBuilder<OrientatedEntityCreatedMessage> mb) : base(worldOrigin, mb)
    {
        MessageBuilder!.AddParameter(worldOrientation);
        NonDebugConstructorBody(worldOrientation);
    }

    #else

    protected OrientatedEntity(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin)
    {
        NonDebugConstructorBody(worldOrientation);
    }

    #endif

    public void NonDebugConstructorBody(Orientation worldOrientation)
    {
        this.worldOrientation = worldOrientation;
    }

    #endregion

    #region Methods

    public override OrientatedEntity ShallowCopy() => (OrientatedEntity)MemberwiseClone();
    public override OrientatedEntity DeepCopy()
    {
        var orientatedEntity = (OrientatedEntity)base.DeepCopy();
        orientatedEntity.rotationMatrix = rotationMatrix;
        orientatedEntity.worldOrientation = worldOrientation;
        return orientatedEntity;
    }

    private void RegenerateRotationMatrix()
    {
        Matrix4x4 directionForwardRotation = Transform.RotateBetweenVectors(Orientation.ModelDirectionForward, worldOrientation.DirectionForward);
        Matrix4x4 directionUpRotation = Transform.RotateBetweenVectors((Vector3D)(directionForwardRotation * Orientation.ModelDirectionUp), worldOrientation.DirectionUp);

        rotationMatrix = directionUpRotation * directionForwardRotation;

        RegenerateModelToWorldMatrix();
    }

    protected override void RegenerateModelToWorldMatrix()
    {
        base.RegenerateModelToWorldMatrix();

        ModelToWorld *= rotationMatrix;
    }

    #endregion
}