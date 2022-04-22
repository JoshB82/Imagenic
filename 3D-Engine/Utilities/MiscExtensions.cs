using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities;

internal static class ListExtensions
{
    internal static void ForEach<T>(this IList<T> list, Action<T> action)
    {
        foreach (T item in list)
        {
            action(item);
        }
    }
}