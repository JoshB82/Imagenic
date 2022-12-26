using System;

namespace Imagenic.Core.Attributes;

/// <summary>
/// Specifies that a method should have a null check added via source generation (and based on static analysis). This class cannot be inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ThrowIfNullAttribute : Attribute
{
    #region Fields and Properties

    //public int[] ParameterIndices { get; set; }

    #endregion

    #region Constructors

    /*public ThrowIfNullAttribute(params int[] parameterIndices)
    {
        //ParameterIndices = parameterIndices;
    }*/

    #endregion
}