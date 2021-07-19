using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Maths.Vectors;
using System;
using System.Drawing;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces
{
    /// <summary>
    /// Encapsulates creation of a <see cref="SolidTriangle"/>.
    /// </summary>
    public sealed class SolidTriangle : Triangle
    {
        #region Fields and Properties

        // Vertices
        //public Vertex P1, P2, P3;
        //public Vector3D T1, T2, T3;

        // Appearance
        public Color Colour { get; set; } = Properties.Settings.Default.FaceColour;

        #endregion

        #region Constructors

        internal SolidTriangle(Vector4D p1, Vector4D p2, Vector4D p3) => (P1, P2, P3) = (p1, p2, p3);

        public SolidTriangle(Vertex modelP1, Vertex modelP2, Vertex modelP3) => (ModelP1, ModelP2, ModelP3) = (modelP1, modelP2, modelP3);

        #endregion

        #region Methods

        internal override void Interpolator(RenderingObject renderingObject, Action<object, int, int, float> bufferAction)
        {
            // Round the vertices
            int x1 = P1.x.RoundToInt();
            int y1 = P1.y.RoundToInt();
            float z1 = P1.z;
            int x2 = P2.x.RoundToInt();
            int y2 = P2.y.RoundToInt();
            float z2 = P2.z;
            int x3 = P3.x.RoundToInt();
            int y3 = P3.y.RoundToInt();
            float z3 = P3.z;

            // Sort the vertices by their y-co-ordinate
            NumericManipulation.SortByY
            (
                ref x1, ref y1, ref z1,
                ref x2, ref y2, ref z2,
                ref x3, ref y3, ref z3
            );

            // Interpolate each point in the triangle
            RenderingObject.InterpolateSolidTriangle
            (
                bufferAction, Colour,
                x1, y1, z1,
                x2, y2, z2,
                x3, y3, z3
            );
        }

        internal override void ResetVertices() => (P1, P2, P3) = (ModelP1.Point, ModelP2.Point, ModelP3.Point);

        #endregion
    }
}