using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Maths;
using Imagenic.Core.Utilities.Tree;
using Microsoft.VisualBasic;
using System.Linq;

namespace Imagenic.Core.Entities;

public class Ray : Entity
{
    #region Fields and Properties

    public Vector3D StartPosition { get; set; }

    private Vector3D direction;
    public Vector3D Direction
    {
        get => direction;
        set => direction = value.Normalise();
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

    public bool DoesIntersectWith<T>(T physicalEntity, out Vector3D? intersection) where T : PhysicalEntity
    {
        switch (physicalEntity)
        {
            case Mesh mesh:
                mesh.CalculateModelToWorldMatrix();
                intersection = mesh.Structure.Faces.SelectMany(face => face.Triangles.Select(triangle =>
                {
                    Vector3D p1 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.ModelP1.Point, 1));
                    Vector3D p2 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.ModelP2.Point, 1));
                    Vector3D p3 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.ModelP3.Point, 1));
                    PlanePoints planePoints = new(p1, p2, p3);

                    DoesIntersect(planePoints, out float? distance);
                    return distance;
                })).Min() * direction + StartPosition;
                return intersection is not null;
            case Node<Mesh> meshNode:
                meshNode.GetDescendantsAndSelfOfType<Mesh>().
            default:
                throw new System.Exception();

                
        }
        /*
        return physicalEntity switch
        {
            Triangle triangle => DoesIntersect(triangle, out intersection),
            Face face => face.Triangles.Min(triangle => DoesIntersect(triangle, out intersection)),
            _ => throw new System.Exception()
        };*/
    }

    private bool DoesIntersect(PlanePoints planePoints, out float? distance)
    {
        Vector3D normal = Vector3D.NormalFromPlane(planePoints);
        if ((direction * normal).ApproxEquals(0))
{
            distance = null;
            return false;
        }
        distance = (planePoints.P1 - StartPosition) * normal / (direction * normal);
        return true;
    }

    public bool DoesIntersect(Triangle triangle, out Vector3D? intersection)
    {
        Vector3D normal = Vector3D.NormalFromPlane(triangle.P1, triangle.P2, triangle.P3);
        if ((direction * normal).ApproxEquals(0))
        {
            intersection = null;
            return false;
        }
        float d = (triangle.P1 - StartPosition) * normal / (direction * normal);
        intersection = d * direction + StartPosition;
        return true;
    }

    #endregion
}

public struct IntersectionInfo
{
    public Vector3D Point { get; }
}