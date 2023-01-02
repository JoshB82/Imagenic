using Imagenic.Core.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Imagenic.Core.Entities;

public sealed class SolidFace : Face
{
    

    #region Constructors

    public SolidFace(params Triangle[] triangles) : this(triangles)
    {

    }

    public SolidFace(IList<Triangle> triangles) : base(triangles)
    {

    }

    #endregion
}