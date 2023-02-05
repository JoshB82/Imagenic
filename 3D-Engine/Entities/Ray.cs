using Imagenic.Core.Utilities.Node;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        : base(
        #if DEBUG
        MessageBuilder<RayCreatedMessage>.Instance()
        #endif
        )
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

    public bool DoesIntersect([DisallowNull] Triangle triangle, out float? intersectionDistance)
    {
        ThrowIfNull(triangle);
        Vector3D normal = Vector3D.NormalFromPlane(triangle.P1.WorldOrigin, triangle.P2.WorldOrigin, triangle.P3.WorldOrigin);
        if ((direction * normal).ApproxEquals(0))
        {
            intersectionDistance = null;
            return false;
        }
        intersectionDistance = (triangle.P1.WorldOrigin - StartPosition) * normal / (direction * normal);
        return true;
    }

    public bool DoesIntersect([DisallowNull] Triangle triangle, out Vector3D? intersection)
    {
        ThrowIfNull(triangle);
        bool doesIntersect = DoesIntersect(triangle, out float? intersectionDistance);
        intersection = intersectionDistance * direction + StartPosition;
        return doesIntersect;
    }

    private bool DoesIntersect(PlanePoints planePoints, out float? distance)
    {
        Vector3D normal = Vector3D.NormalFromPlane(planePoints.P1, planePoints.P2, planePoints.P3);
        if ((direction * normal).ApproxEquals(0))
        {
            distance = null;
            return false;
        }
        distance = (planePoints.P1 - StartPosition) * normal / (direction * normal);
        return true;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh and, if so, returns the distance between the start position and the first point of intersection.
    /// </summary>
    /// <param name="mesh">The mesh being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersectionDistance">The distance between the start position and the first point of intersection. If no intersection exists, its value is set to <see langword="null"/>.</param>
    /// <returns>Whether or not the ray intersects with a mesh.</returns>
    /// <exception cref="ArgumentNullException">The mesh cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] Mesh mesh, out float? intersectionDistance)
    {
        ThrowIfNull(mesh);
        mesh.CalculateModelToWorldMatrix();
        intersectionDistance = mesh.Structure?.Faces?.SelectMany(face => face.Triangles.Select(triangle =>
        {
            Vector3D p1 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.P1.WorldOrigin, 1));
            Vector3D p2 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.P2.WorldOrigin, 1));
            Vector3D p3 = (Vector3D)(mesh.ModelToWorld * new Vector4D(triangle.P3.WorldOrigin, 1));
            PlanePoints planePoints = new(p1, p2, p3);

            DoesIntersect(planePoints, out float? distance);
            return distance;
        })).Min();
        return intersectionDistance is not null;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh and, if so, returns the first point of intersection.
    /// </summary>
    /// <param name="mesh">The mesh being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersection">The first point of intersection between the ray and the mesh. If no intersection exists, its value is set to <see langword="null"/>.</param>
    /// <returns>Whether or not the ray intersects with a mesh.</returns>
    /// <exception cref="ArgumentNullException">The mesh cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] Mesh mesh, out Vector3D? intersection)
    {
        ThrowIfNull(mesh);
        bool doesIntersect = DoesIntersect(mesh, out float? intersectionDistance);
        intersection = intersectionDistance * direction + StartPosition;
        return doesIntersect;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh node (the mesh content of the node and the contents of child nodes) and, if so, returns the distance between the start position and the first point of intersection.
    /// </summary>
    /// <param name="meshNode">The mesh node being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersectionDistance">The distance between the start position and the first point of intersection. If no intersection exists, its value is set to <see langword="null"/>.</param>
    /// <returns>Whether or not the ray intersects with a mesh.</returns>
    /// <exception cref="ArgumentNullException">The mesh node cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] Node<Mesh> meshNode, out float? intersectionDistance)
    {
        ThrowIfNull(meshNode);
        intersectionDistance = meshNode.GetDescendantsAndSelfOfType<Mesh>().GetAllContents<Mesh>().Select(mesh =>
        {
            DoesIntersect(mesh, out float? intersectionDistance);
            return intersectionDistance;
        }).Min();
        return intersectionDistance is not null;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh node (the mesh content of the node and the contents of child nodes) and, if so, returns the first point of intersection.
    /// </summary>
    /// <param name="meshNode">The mesh node being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersection">The first point of intersection between the ray and the mesh. If no intersection exists, its value is set to <see langword="null"/>.</param>
    /// <returns>Whether or not the ray intersects with a mesh.</returns>
    /// <exception cref="ArgumentNullException">The mesh node cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] Node<Mesh> meshNode, out Vector3D? intersection)
    {
        ThrowIfNull(meshNode);
        bool doesIntersect = DoesIntersect(meshNode, out float? intersectionDistance);
        intersection = intersectionDistance * direction + StartPosition;
        return doesIntersect;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh node sequence (the mesh content of the nodes and the contents of child nodes) and, if so, returns the distance between the start position and the first point of intersection.
    /// </summary>
    /// <param name="meshNodes">The mesh node sequence being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersectionDistance"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">The mesh node sequence cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] IEnumerable<Node<Mesh>> meshNodes, out float? intersectionDistance)
    {
        ThrowIfNull(meshNodes);
        intersectionDistance = meshNodes.Select(meshNode =>
        {
            DoesIntersect(meshNode, out float? intersectionDistance);
            return intersectionDistance;
        }).Min();
        return intersectionDistance is not null;
    }

    /// <summary>
    /// Determines if the ray intersects a mesh node sequence (the mesh content of the nodes and the contents of child nodes) and, if so, returns the first point of intersection.
    /// </summary>
    /// <param name="meshNodes">The mesh node sequence being tested. This cannot be <see langword="null"/>.</param>
    /// <param name="intersection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">The mesh node sequence cannot be null.</exception>
    public bool DoesIntersect([DisallowNull] IEnumerable<Node<Mesh>> meshNodes, out Vector3D? intersection)
    {
        ThrowIfNull(meshNodes);
        bool doesIntersect = DoesIntersect(meshNodes, out float? intersectionDistance);
        intersection = intersectionDistance * direction + StartPosition;
        return doesIntersect;
    }

    /*
    public bool DoesIntersectWith<T>(T physicalEntity, out Vector3D? intersection) where T : PhysicalEntity
    {
        switch (physicalEntity)
        {
            case Mesh mesh:
                
            case Node<Mesh> meshNode:
                
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
    //}


    #endregion
}

public struct IntersectionInfo
{
    public Vector3D Point { get; }
}