using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Imagenic.Core.Maths;

public class Gradient
{
    #region Fields and Properties

    public GradientType Type { get; set; }
    public List<ColourSection> Sections { get; set; }

    public Vector3D Direction { get; set; }

    public static Gradient LinearBlueGreen = new Gradient(GradientType.Linear, new List<ColourSection>
    {
        new ColourSection(Color.Blue, 50),
        new ColourSection(Color.Green, 50)
    }, Vector3D.UnitX);

    #endregion

    #region Constructors

    public Gradient(GradientType type, List<ColourSection> sections, Vector3D direction)
    {
        Type = type;
        Sections = sections;
        if (direction == Vector3D.Zero)
        {
            throw new MessageBuilder<GradientDirectionCannotBeZeroMessage>()
                  .AddParameter(direction)
                  .BuildIntoException<ArgumentException>();
        }
        Direction = direction;
    }

    #endregion
}

public struct ColourSection
{
    #region Fields and Properties

    public Color Colour { get; set; }
    public float Size { get; set; }

    #endregion

    #region Constructors

    public ColourSection(Color colour, float size)
    {
        Colour = colour;
        Size = size;
    }

    #endregion
}