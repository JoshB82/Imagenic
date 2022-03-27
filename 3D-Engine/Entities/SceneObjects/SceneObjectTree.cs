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

public abstract partial class SceneObject
{
    #region Fields and Properties

    public SceneObject this[int index]
    {
        get => children[index];
        set => children[index] = value;
    }

    #endregion

    #region Methods

    

    

    

    public IEnumerable<SceneObject> GetAllParentsAndSelf(Predicate<SceneObject> predicate = null)
    {
        List<SceneObject> sceneObjects = this.GetAllParents(predicate).ToList();
        if ((predicate is not null && predicate(this)) || predicate is null)
        {
            sceneObjects.Add(this);
        }
        return sceneObjects;
    }

    public IEnumerable<T> GetAllParentsAndSelf<T>(Predicate<T> predicate = null) where T : SceneObject
    {
        return this.GetAllParentsAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
    }

    

    

    

    

    public void ForEach(Action<SceneObject> action, Predicate<SceneObject> predicate = null)
    {
        foreach (SceneObject child in this.GetAllChildrenAndSelf(predicate))
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