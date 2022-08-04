using System;

namespace Imagenic.Core.Attributes;

/// <summary>
/// Specifies that a method should have a null check added via source generation (and based on static analysis). This class cannot be inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class NullCheckAttribute : Attribute
{
    #region Fields and Properties

    public int[] ParameterIndices { get; set; }

    #endregion

    #region Constructors

    public NullCheckAttribute(params int[] parameterIndices)
    {
        ParameterIndices = parameterIndices;
    }

    #endregion
}