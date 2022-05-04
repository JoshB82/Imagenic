using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Maths.Transformations;

public static partial class Transform
{
    public static Matrix4x4 ReflectAboutPlane(Vector3D axis)
    {
        if (axis.ApproxEquals(Vector3D.Zero))
        {
            // throw exception
        }

        //return new();
    }
}