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
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// A sealed class representing a three-dimensional torus mesh. It inherits from the abstract <see cref="Mesh"/> class.
    /// </summary>
    /// <remarks>
    /// Composition:<br/>
    /// <list type="bullet">
    /// <item><description>A number of vertices equal to the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>.</description></item>
    /// <item><description></description></item>
    /// <item><description></description></item>
    /// </list>
    /// </remarks>
    public sealed class Torus : Mesh
    {
        #region Fields and Properties

        private float innerRadius, outerRadius;
        private int innerResolution, outerResolution;

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
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

            float interiorRadius = (outerRadius - innerRadius) / 2, exteriorRadius = innerRadius + interiorRadius;
            float innerAngle = Tau / innerResolution, outerAngle = Tau / outerResolution;
            for (int i = 0; i < outerResolution; i++)
            {
                for (int j = 0; j < innerResolution; j++)
                {
                    Content.Vertices[innerResolution * i + j + 1] = new Vertex(new Vector4D(Cos(innerAngle * i) * interiorRadius * Cos(outerAngle * i) * exteriorRadius,
                                                                                            Sin(innerAngle * i) * interiorRadius,
                                                                                            Sin(outerAngle * i) * exteriorRadius,
                                                                                            1));
                }
            }
        }

        private void GenerateEdges()
        {
            Content.Edges = new Edge[innerResolution * outerResolution];
        }

        private void GenerateFaces()
        {

        }

        #endregion
    }
}