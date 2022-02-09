/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a SolidEdge, representing a single-coloured edge with no pattern.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using System.Drawing;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;

public class SolidEdge : Edge
{
    #region Fields and Properties

    private Color colour = Properties.Settings.Default.EdgeColour;
    public Color Colour
    {
        get => colour;
        set
        {
            if (value == colour) return;
            colour = value;
            InvokeRenderingEvents(true, false);
        }
    }

    #endregion

    #region Constructors

    public SolidEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2) { }

    public SolidEdge(Vertex modelP1, Vertex modelP2, Color colour) : base(modelP1, modelP2)
    {
        Colour = colour;
    }

    #endregion
}