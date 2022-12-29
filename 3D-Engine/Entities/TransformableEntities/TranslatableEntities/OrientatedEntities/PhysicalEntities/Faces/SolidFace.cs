using System.Collections;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

public sealed class SolidFace : Face
{
    #region Fields and Properties



    #endregion

    #region Constructors

    public SolidFace(params Triangle[] triangles) : this(triangles)
    {

    }

    public SolidFace(IList<Triangle> triangles) : base(triangles)
    {

    }

    #endregion
}