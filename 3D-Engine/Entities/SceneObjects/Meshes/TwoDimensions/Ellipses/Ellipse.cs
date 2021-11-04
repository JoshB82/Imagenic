/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an ellipse mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    public sealed class Ellipse : Mesh
    {
        #region Fields and Properties

        private float majorAxis, minorAxis;
        private int resolution;

        public float MajorAxis
        {
            get => majorAxis;
            set
            {
                majorAxis = value;
                Scaling = new Vector3D(majorAxis, 1, minorAxis);
            }
        }

        public float MinorAxis
        {
            get => MinorAxis;
            set
            {
                minorAxis = value;
                Scaling = new Vector3D(majorAxis, 1, minorAxis);
            }
        }

        public int Resolution
        {
            get => resolution;
            set
            {
                if (value == resolution) return;
                resolution = value;
                RequestNewRenders();
            }
        }

        #endregion

        #region Constructors

        public Ellipse(Vector3D worldOrigin,
                       Orientation worldOrientation,
                       float majorAxis,
                       float minorAxis,
                       int resolution) : base(worldOrigin, worldOrientation, 2)
        {
            MajorAxis = majorAxis;
            MinorAxis = minorAxis;
            Resolution = resolution;

            Vertices = new Vertex[resolution + 1];
            Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
        }

        #endregion

        #region Methods

        private void GenerateVertices()
        {

        }

        private void GenerateEdges()
        {

        }

        private void GenerateFaces()
        {

        }

        #endregion

        #region Casting



        #endregion
    }
}