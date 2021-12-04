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

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class Line : Mesh
    {
        #region Fields and Properties

        private Vector3D startPosition, endPosition;

        public Vector3D StartPosition
        {
            get => startPosition;
            set
            {
                startPosition = value;
                Vector3D line_vector = endPosition - startPosition;
                Scaling = new Vector3D(line_vector.x, line_vector.y, line_vector.z);
            }
        }
        public Vector3D EndPosition
        {
            get => endPosition;
            set
            {
                endPosition = value;
                Vector3D line_vector = endPosition - startPosition;
                Scaling = new Vector3D(line_vector.x, line_vector.y, line_vector.z);
            }
        }

        public float Length { get; set; } //ss
        public Vector3D Unit_Vector { get; set; } //s

        #endregion

        #region Constructors

        public Line(Vector3D worldOrigin, Orientation worldOrientation, float length) : base(worldOrigin, worldOrientation, GenerateStructure())
        {

        }

        

        

        public Line(Vector3D start_position, Vector3D end_position) : base(start_position, Vector3D.UnitZ, Vector3D.UnitY)
        {
            Dimension = 2;

            StartPosition = start_position;
            EndPosition = end_position;

            Vertices = new Vertex[2]
            {
                new Vertex(new Vector4D(0, 0, 0, 1)), // 0
                new Vertex(new Vector4D(1, 1, 1, 1)) // 1
            };

            Edges = new Edge[1] { new Edge(Vertices[0], Vertices[1]) };

            DrawFaces = false;
        }

        public Line(Vector3D start_position, Vector3D unit_vector, float length) : this(start_position, start_position + unit_vector * length)
        {
            Length = length;
            Unit_Vector = unit_vector;
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
        }

        private static IList<Edge> GenerateEdges()
        {

        }

        #endregion
    }
}