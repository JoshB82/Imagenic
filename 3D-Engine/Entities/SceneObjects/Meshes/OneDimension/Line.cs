/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a line mesh.
 */

using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.OneDimension
{
    public sealed class Line : Mesh
    {
        #region Fields and Properties

        public override Vector3D WorldOrigin
        {
            get => base.WorldOrigin;
            set
            {
                base.WorldOrigin = value;
                Scaling = endPosition - value;
            }
        }

        public override Orientation WorldOrientation
        {
            get => base.WorldOrientation;
            set
            {
                base.WorldOrientation = value;
                endPosition = value.DirectionForward * Length + WorldOrigin;
            }
        }

        private Vector3D endPosition;

        public Vector3D EndPosition
        {
            get => endPosition;
            set
            {
                endPosition = value;
                Scaling = endPosition - WorldOrigin;
                length = (endPosition - WorldOrigin).Magnitude();
            }
        }

        private float length;

        public float Length
        {
            get => length;
            set
            {
                length = value;
                endPosition = WorldOrientation.DirectionForward * length + WorldOrigin;
            }
        }

        #endregion

        #region Constructors

        public Line(Vector3D worldOrigin, Orientation worldOrientation, float length) : base(worldOrigin, worldOrientation, GenerateStructure())
        {
            Length = length;
        }

        public Line(Vector3D worldOrigin, Vector3D endPosition) : this(worldOrigin, Orientation.OrientationZY, (endPosition - worldOrigin).Magnitude())
        {
            DrawFaces = false;
        }

        #endregion

        #region Methods

        private static MeshStructure GenerateStructure()
        {
            IList<Vertex> vertices = GenerateVertices();
            IList<Edge> edges = GenerateEdges();

            return new MeshStructure(Dimension.One, vertices, edges, null);
        }

        private static IList<Vertex> GenerateVertices()
        {
            return HardcodedMeshData.LineVertices;
        }

        private static IList<Edge> GenerateEdges()
        {
            return HardcodedMeshData.LineEdges;
        }

        #endregion
    }
}