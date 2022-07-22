/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * An abstract base class that defines objects of type SceneObject.
 */

using Imagenic.Core.Entities;
using Imagenic.Core.Utilities;

namespace _3D_Engine.Entities.SceneObjects;

/// <summary>
/// An abstract base class that defines objects of type <see cref="SceneEntity"/>.
/// </summary>
public abstract partial class SceneEntity : Entity
{
    #region Constructors

    protected SceneEntity(Vector3D worldOrigin,
                          Orientation worldOrientation,
                          bool hasDirectionArrows = true)
    {
        

        
        

        #if DEBUG

        new MessageBuilder<EntityCreatedMessage>()
            .AddType(this.GetType())
            .AddParameters(worldOrigin.ToString())
            .Build()
            .DisplayInConsole();

        #endif
    }

    #endregion

    #region Methods

    protected virtual void CalculateModelToWorldMatrix()
    {
        Matrix4x4 directionForwardRotation = Transform.RotateBetweenVectors(Orientation.ModelDirectionForward, worldOrientation.DirectionForward);
        Matrix4x4 directionUpRotation = Transform.RotateBetweenVectors((Vector3D)(directionForwardRotation * Orientation.ModelDirectionUp), worldOrientation.DirectionUp);
        Matrix4x4 translation = Transform.Translate(WorldOrigin);

        // String the transformations together in the following order:
        // 1) Rotation around direction forward vector
        // 2) Rotation around direction up vector
        // 3) Translation to final position in world space
        ModelToWorld = translation * directionUpRotation * directionForwardRotation;
    }

    #endregion
}