using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Maths;

public static class Lerp
{
    public static IEnumerable<float> Between(float start, float end, int numSteps)
    {
        float stepSize = (end - start) / numSteps;
        for (int i = 1; i <= numSteps; i++)
        {
            yield return start + i * stepSize;
        }
    }

    public static IEnumerable<Vector3D> Between(Vector3D start, Vector3D end, int numSteps)
    {
        Vector3D stepSize = (end - start) / numSteps;
        for (int i = 1; i <= numSteps; i++)
        {
            yield return start + i * stepSize;
        }
    }
}