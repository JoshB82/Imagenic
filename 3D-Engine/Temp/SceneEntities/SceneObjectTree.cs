/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for implementing a tree structure in SceneObject.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.Entities.SceneObjects;

public abstract partial class SceneEntity
{
    #region Fields and Properties

    public SceneEntity this[int index]
    {
        get => children[index];
        set => children[index] = value;
    }

    #endregion

    #region Methods

    

    

    

    

    

    

    

    

    public void ForEach(Action<SceneEntity> action, Predicate<SceneEntity> predicate = null)
    {
        foreach (SceneEntity child in this.GetAllChildrenAndSelf(predicate))
        {
            action(child);
        }
    }

    public void ForEach<T>(Action<T> action, Predicate<T> predicate = null)
    {
        this.ForEach(action, x => x is T t && predicate(t));
    }

    #endregion
}