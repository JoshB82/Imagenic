using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public sealed class GradientStyle : FaceStyle
{
    #region Fields and Properties

    public Gradient DisplayGradient { get; set; }

    #endregion

    public GradientStyle([DisallowNull] Gradient gradient)
	{
        ThrowIfNull(gradient);
        DisplayGradient = gradient;
	}
}