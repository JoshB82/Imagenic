using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities;

internal static class EnumerableExtensions
{
    internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action(item);
        }
    }
}