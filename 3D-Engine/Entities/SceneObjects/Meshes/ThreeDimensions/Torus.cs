/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a torus mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Torus"/> mesh.
    /// </summary>
    public sealed class Torus : Mesh
    {
        #region Fields and Properties

        private float innerRadius, outerRadius;
        private int innerResolution, outerResolution;

        public float InnerRadius
        {
            get => innerRadius;
            set
            {
                if (value == innerRadius) return;
                innerRadius = value;
                RequestNewRenders();

                GenerateVertices();
            }
        }

        public float OuterRadius
        {
            get => outerRadius;
            set
            {
                if (value == outerRadius) return;
                outerRadius = value;
                RequestNewRenders();

                GenerateVertices();
            }
        }

        public int InnerResolution
        {
            get => innerResolution;
            set
            {
                if (value == innerResolution) return;
                innerResolution = value;
                RequestNewRenders();

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();
            }
        }

        public int OuterResolution
        {
            get => outerResolution;
            set
            {
                if (value == outerResolution) return;
                outerResolution = value;
                RequestNewRenders();

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();
            }
        }

        #endregion

        #region Constructors

        public Torus(Vector3D worldOrigin,
                     Orientation worldOrientation,
                     float innerRadius,
                     float outerRadius,
                     int innerResolution,
                     int outerResolution) : base(worldOrigin, worldOrientation, 3)
        {
            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.innerResolution = innerResolution;
            this.outerResolution = outerResolution;

            GenerateVertices();
            GenerateEdges();
            GenerateFaces();
        }

        #endregion

        #region Methods

        private void GenerateVertices()
        {
            Content.Vertices = new Vertex[innerResolution * outerResolution];
            Content.Vertices[0] = new Vertex(Vector4D.Zero);

            float interiorRadius = (outerRadius - innerRadius) / 2;
            float exteriorRadius = innerRadius + interiorRadius;
            float innerAngle = 2 * PI / innerResolution;
            float outerAngle = 2 * PI / outerResolution;
            for (int i = 0; i < innerResolution; i++)
            {
                Content.Vertices[i + 1] = new Vertex(new Vector4D(Cos(innerAngle * i) * interiorRadius, Sin(innerAngle * i) * interiorRadius, 0, 1));
            }
            for (int i = 0; i < outerResolution; i++)
            {
                Content.Vertices[i + 1].Point = new Vector4D(Cos(outerAngle * i) * exteriorRadius * Content.Vertices[i + 1].Point.x, Content.Vertices[i + 1].Point.y, Sin(outerAngle * i) * exteriorRadius, 1);
            }
        }

        private void GenerateEdges()
        {

        }

        private void GenerateFaces()
        {

        }

        #endregion
    }
}