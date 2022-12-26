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

using Imagenic.Core.Enums;
using System.Drawing;

namespace Imagenic.Core.Entities;

public sealed class SolidEdge : Edge
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
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #if DEBUG

    private protected override IMessageBuilder<SolidEdgeCreatedMessage>? MessageBuilder => (IMessageBuilder<SolidEdgeCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    #if DEBUG

    public SolidEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2, MessageBuilder<SolidEdgeCreatedMessage>.Instance())
    {

    }

    public SolidEdge(Vertex modelP1,
                     Vertex modelP2,
                     Color colour) : base(modelP1, modelP2, MessageBuilder<SolidEdgeCreatedMessage>.Instance())
    {
        NonDebugConstructorBody(colour);
    }

    #else

    public SolidEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2) { }

    public SolidEdge(Vertex modelP1,
                     Vertex modelP2,
                     Color colour) : base(modelP1, modelP2)
    {
        NonDebugConstructorBody(colour);
    }

    #endif

    private void NonDebugConstructorBody(Color colour)
    {
        Colour = colour;
    }

    #endregion
}