/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a dashed edge.
 */

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;

public class DashedEdge : Edge
{
    #region Fields and Properties

    private IEnumerable<DashedEdgeSection> sections;
    public IEnumerable<DashedEdgeSection> Sections
    {
        get => sections;
        set
        {
            if (!value.Select(x => x.Percentage).Sum().ApproxEquals(100))
            {
                // throw exception
            }
            sections = value;
        }
    }

    private float patternPercentage = 100;
    public float PatternPercentage
    {
        get => patternPercentage;
        set
        {
            if (value == patternPercentage) return;
            if (patternPercentage.ApproxLessThan(0) || patternPercentage.ApproxMoreThan(100))
            {
                // throw exception
            }
            patternPercentage = value;
            InvokeRenderingEvents(true, false);
        }
    }

    #endregion

    #region Constructors

    public DashedEdge(Vertex modelP1, Vertex modelP2) : base(modelP1, modelP2)
    {
        Sections = new DashedEdgeSection[]
        {
            new DashedEdgeSection(50, false, Color.Black),
            new DashedEdgeSection(50, true)
        };
    }

    public DashedEdge(Vertex modelP1,
                        Vertex modelP2,
                        IEnumerable<DashedEdgeSection> sections) : base(modelP1, modelP2)
    {
        Sections = sections;
    }

    #endregion
}