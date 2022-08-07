using Imagenic.Core.Maths.Transformations;
using System;

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
            /*
            if (value is null)
            {
                throw new MessageBuilder<ParameterCannotBeNullException>()
                    .AddParameters(nameof(value))
                    .BuildIntoException<ParameterCannotBeNullException>();
            }
            */
            worldOrientation = value;
            RegenerateRotationMatrix();
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    protected OrientatedEntity(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin)
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