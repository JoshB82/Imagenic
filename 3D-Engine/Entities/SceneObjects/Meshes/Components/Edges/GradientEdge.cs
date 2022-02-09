/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a GradientEdge, representing a edge with a gradient colour.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;

public class GradientEdge : Edge
{
    #region Fields and Properties

    #endregion

    #region Constructors

    public GradientEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2) { }

    #endregion
}