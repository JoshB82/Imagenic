using _3D_Engine.Constants;
using _3D_Engine.Maths;
using Imagenic.Core.Maths.Transformations;
using Imagenic.Core.Utilities;

namespace Imagenic.Core.Entities;

public abstract class OrientatedEntity : PositionedEntity
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