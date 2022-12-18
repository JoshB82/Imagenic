using Imagenic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Temp;

public static class TransformationBuilder
{
    public static TEntity StartTransformation<TEntity>(this TEntity entity) where TEntity : Entity
    {

    }

    public static Matrix4x4 EndTransformation<TEntity>(this TEntity entity) where TEntity : Entity
    {

    }
}
