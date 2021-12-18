using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Entities;

public class Ray : Entity
{
    #region Fields and Properties

    public Vector3D StartPosition { get; set; }

    public Vector3D Direction { get; set; }

    #endregion

    #region Constructors

    public Ray(Vector3D startPosition, Vector3D direction)
    {
        StartPosition = startPosition;
        Direction = direction;
    }

    public Ray(Vector3D point1, Vector3D point2)
    {

    }

    #endregion

    #region Methods

    public bool DoesIntersect(Triangle triangle, out Vector3D? intersection)
    {
        Vector3D normal = Vector3D.NormalFromPlane(triangle.P1, triangle.P2, triangle.P3);
        if ((Direction * normal).ApproxEquals(0))
        {
            intersection = null;
            return false;
        }
        float d = ((triangle.P1 - StartPosition) * normal) / (Direction * normal);
        intersection = d * Direction + StartPosition;
        return true;
    }

    #endregion
}