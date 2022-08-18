using System;

namespace NullCheckerSourceGenerator
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ThrowIfNullAttribute : Attribute { }
}