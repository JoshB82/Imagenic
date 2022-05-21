using _3D_Engine.Constants;
using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Maths.Transformations;
using Imagenic.Core.Renderers.Animations;
using Imagenic.Core.Utilities;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

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
            if (value is null)
            {
                throw new MessageBuilder<ParameterCannotBeNullException>()
                    .AddParameters(nameof(value))
                    .BuildIntoException<ParameterCannotBeNullException>();
            }
            worldOrientation = value;
            RegenerateRotationMatrix();
            InvokeRenderingEvents();
        }
    }

    public IEnumerable<Frame<Orientation>> WorldOrientationFrames
    {
        get;
        set;
    }

    #endregion

    #region Constructors

    protected OrientatedEntity(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin)
    {
        this.worldOrientation = worldOrientation;
    }

    #endregion

    #region Methods

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