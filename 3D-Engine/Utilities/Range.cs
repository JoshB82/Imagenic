using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities;

internal enum BoundaryType
{
    Inclusive,
    Exclusive,
    Infinite
}

internal struct Range
{
    private float start, end;
    internal float Start
    {
        get => start;
        set
        {
            if (start > end)
            {
                // throw exception
            }
            start = value;
        }
    }
    internal BoundaryType StartBoundaryType { get; set; } = BoundaryType.Inclusive;
    internal float End { get; set; }
    internal BoundaryType EndBoundaryType { get; set; } = BoundaryType.Inclusive;

    public Range(float start, float end)
    {
        Start = start;
        End = end;
    }
}