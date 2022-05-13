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

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components;

/// <summary>
/// Encapsulates creation of an <see cref="Vertex"/>.
/// </summary>
public sealed class Vertex : PositionedEntity
{
    #region Fields and Properties

    public Vector3D? Normal { get; set; }

    private Vector3D point;
    public Vector3D Point
    {
        get => point;
        set
        {
            if (value == point) return;
            point = value;
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    public Vertex(Vector3D point, Vector3D? normal = null)
    {
        Point = point;
        Normal = normal;
    }

    public Vertex(Vector3D point, float w)
    {
        Point = point;
        this.w = w;
    }

    #endregion
}