/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Vertex, representing a point in 3D space.
 */

using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components;

/// <summary>
/// Encapsulates creation of an <see cref="Vertex"/>.
/// </summary>
public sealed class Vertex : Entity
{
    #region Fields and Properties

    public Vector3D? Normal { get; set; }
    public Vector3D Point { get; set; }

    #endregion

    #region Constructors

    public Vertex(Vector3D point, Vector3D? normal = null)
    {
        Point = point;
        Normal = normal;
    }

    #endregion
}