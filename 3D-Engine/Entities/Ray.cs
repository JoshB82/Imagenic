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

    public bool DoesIntersect(Triangle triangle)
    {

    }

    #endregion
}