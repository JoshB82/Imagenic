using _3D_Engine;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;

namespace Imagenic.Core.Entities;

public class Ray : Entity
{
    #region Fields and Properties

    public Vector3D StartPosition { get; set; }

    private Vector3D direction;
    public Vector3D Direction
    {
        get => direction;
        set
        {
            direction = value.Normalise();
        }
    }

    #endregion

    #region Constructors

    public Ray(Vector3D startPosition, Vector3D direction)
    {
        StartPosition = startPosition;
        Direction = direction;
    }

    public static Ray CreateRayFromPoints(Vector3D point1, Vector3D point2)
    {
        return new Ray(point1, point2 - point1);
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
        float d = (triangle.P1 - StartPosition) * normal / (Direction * normal);
        intersection = d * Direction + StartPosition;
        return true;
    }

    #endregion
}