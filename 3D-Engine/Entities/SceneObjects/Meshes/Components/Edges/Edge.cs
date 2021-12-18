/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an edge.
 */

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;

/// <summary>
/// Encapsulates creation of an <see cref="Edge"/>.
/// </summary>
public abstract class Edge : Entity
{
    #region Fields and Properties

    // Appearance
    public bool Visible { get; set; } = true;

    // Vertices
    internal Vertex P1 { get; set; }
    internal Vertex P2 { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an <see cref="Edge"/>.
    /// </summary>
    /// <param name="modelP1">The position of the first point on the <see cref="Edge"/>.</param>
    /// <param name="modelP2">The position of the second point on the <see cref="Edge"/>.</param>
    public Edge(Vertex modelP1, Vertex modelP2)
    {
        P1 = modelP1; P2 = modelP2;
    }

    #endregion
}