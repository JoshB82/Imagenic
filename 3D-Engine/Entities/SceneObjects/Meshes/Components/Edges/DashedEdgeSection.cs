/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a dashed edge section.
 */

using _3D_Engine.Entities;
using System.Drawing;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;

public class DashedEdgeSection : Entity
{
    #region Fields and Properties

    private bool isTransparent;
    public bool IsTransparent
    {
        get => isTransparent;
        set
        {
            if (value == isTransparent) return;
            isTransparent = value;
            if (isTransparent)
            {
                colour = null;
            }
            InvokeRenderingEvents(true, false);
        }
    }

    private Color? colour;
    public Color? Colour
    {
        get => colour;
        set
        {
            if (value == colour) return;
            colour = value;
            isTransparent = colour == null;
            InvokeRenderingEvents(true, false);
        }
    }

    private float percentage;
    public float Percentage
    {
        get => percentage;
        set
        {
            if (value == percentage) return;
            percentage = value;
            InvokeRenderingEvents(true, false);
        }
    }

    #endregion

    #region Constructors

    public DashedEdgeSection(float percentage, bool isTransparent = true, Color? colour = null)
    {
        IsTransparent = isTransparent;
        Colour = colour;
        Percentage = percentage;
    }

    #endregion
}