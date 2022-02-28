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

    

    public IEnumerable<SceneObject> GetAllParents(Predicate<SceneObject> predicate = null)
    {
        List<SceneObject> parents = new();
        if (Parent is not null && ((predicate is not null && predicate(Parent)) || predicate is null))
        {
            parents.Add(Parent);
            parents.AddRange(Parent.GetAllParents(predicate));
        }
        return parents;
    }

    public IEnumerable<T> GetAllParents<T>(Predicate<T> predicate = null) where T : SceneObject
    {
        return this.GetAllParents(x => x is T t && predicate(t)) as IEnumerable<T>;
    }

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

    public IEnumerable<SceneObject> GetAllChildren(Predicate<SceneObject> predicate = null)
    {
        List<SceneObject> children = new();
        foreach (SceneObject child in Children)
        {
            if (predicate is not null && !predicate(child))
            {
                continue;
            }

            children.Add(child);
            children.AddRange(child.GetAllChildren(predicate));
        }
        return children;
    }

    public IEnumerable<T> GetAllChildren<T>(Predicate<T> predicate = null) where T : SceneObject
    {
        return this.GetAllChildren(x => x is T t && predicate(t)) as IEnumerable<T>;
    }

    public IEnumerable<SceneObject> GetAllChildrenAndSelf(Predicate<SceneObject> predicate = null)
    {
        List<SceneObject> sceneObjects = this.GetAllChildren(predicate).ToList();
        if ((predicate is not null && predicate(this)) || predicate is null)
        {
            sceneObjects.Add(this);
        }
        return sceneObjects;
    }

    /// <summary>
    /// Gets all children and this that are of type <typeparamref name="T"/> and an optional predicate.
    /// </summary>
    /// <typeparam name="T">The type of all the returned <see cref="SceneObject">SceneObjects</see>.</typeparam>
    /// <param name="predicate">A <see cref="Predicate{T}"/> that all returned <see cref="SceneObject">SceneObjects</see> must satisfy.</param>
    /// <returns></returns>
    public IEnumerable<T> GetAllChildrenAndSelf<T>(Predicate<T> predicate = null) where T : SceneObject
    {
        return this.GetAllChildrenAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
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