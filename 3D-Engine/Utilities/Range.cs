using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities;

public class Range
{
    private int minimum;
    private int? maximum;

    public int Minimum
    {
        get => minimum;
        set
        {
            if (value > maximum)
            {
                // throw exception
            }
            minimum = value;
        }
    }
    
    public int? Maximum
    {
        get => maximum;
        set
        {
            if (value < minimum)
            {
                // throw exception
            }
            maximum = value;
        }
    }

    public Range()
    {

    }

    public Range(int minimum, int maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }
}