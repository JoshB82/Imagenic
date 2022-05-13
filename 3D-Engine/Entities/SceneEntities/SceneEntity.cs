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

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities;
using Imagenic.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.Entities.SceneObjects;

/// <summary>
/// An abstract base class that defines objects of type <see cref="SceneEntity"/>.
/// </summary>
public abstract partial class SceneEntity : Entity
{
    #region Fields and Properties

    // Appearance
    

    private bool displayDirectionArrows;
    /// <summary>
    /// Determines whether the <see cref="SceneEntity"/> direction arrows are shown or not.
    /// </summary>
    public bool DisplayDirectionArrows
    {
        get => displayDirectionArrows;
        set
        {
            if (value == displayDirectionArrows) return;
            displayDirectionArrows = value;
            InvokeRenderingEvents();
        }
    }
    internal bool HasDirectionArrows { get; set; }

    // Matrices
    

    

    

    // Render Camera
    internal List<RendererBase> Renderers { get; set; } = new();
    internal virtual void RequestNewRenders()
    {
        foreach (RendererBase renderer in Renderers)
        {
            renderer.NewRenderNeeded = true;
        }
    }

    public void Insert(int index, SceneEntity item)
    {
        Children.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Children.RemoveAt(index);
    }
    
    public bool Remove(SceneEntity item)
    {
        return Children.Remove(item);
    }

    #endregion

    #region Constructors

    protected SceneEntity(Vector3D worldOrigin,
                          Orientation worldOrientation,
                          bool hasDirectionArrows = true)
    {
        if (HasDirectionArrows = hasDirectionArrows)
        {
            this.AddChildren(
                new Arrow(worldOrigin, worldOrientation, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Blue),
                new Arrow(worldOrigin, Orientation.CreateOrientationForwardUp(worldOrientation.DirectionUp, -worldOrientation.DirectionForward), Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Green),
                new Arrow(worldOrigin, Orientation.CreateOrientationForwardUp(Transform.CalculateDirectionRight(worldOrientation.DirectionForward, worldOrientation.DirectionUp), worldOrientation.DirectionUp), Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Red)
            );
        }

        this.SetOrientation(worldOrientation);
        WorldOrigin = worldOrigin;

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

    public bool Contains(SceneEntity item)
    {
        return Children.Contains(item);
    }

    public void CopyTo(SceneEntity[] array, int arrayIndex)
    {
        Children.CopyTo(array, arrayIndex);
    }

    public int IndexOf(SceneEntity item)
    {
        return Children.IndexOf(item);
    }

    public IEnumerator<SceneEntity> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public SceneEntity ShallowCopy()
    {
        return this.MemberwiseClone() as SceneEntity;
    }

    public SceneEntity DeepCopy()
    {
        SceneEntity copy = ShallowCopy();
        copy.Renderers = new List<Camera>();
        copy.Renderers.AddRange(this.Renderers);
        return copy;
    }

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