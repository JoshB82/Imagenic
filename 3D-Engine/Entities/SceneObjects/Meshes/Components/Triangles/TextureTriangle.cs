using _3D_Engine.Maths.Vectors;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using System;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces
{
    public sealed class TextureTriangle : Triangle
    {
        #region Fields and Properties

        // Appearance
        public Texture TextureObject { get; set; }

        // Texture space values
        public Vector3D TextureT1 { get; set; }
        public Vector3D TextureT2 { get; set; }
        public Vector3D TextureT3 { get; set; }

        // Calculation values
        internal Vector3D T1 { get; set; }
        internal Vector3D T2 { get; set; }
        internal Vector3D T3 { get; set; }

        #endregion

        #region Constructors

        public TextureTriangle(Vector4D p1, Vector4D p2, Vector4D p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture textureObject)
        {
            (P1, P2, P3) = (p1, p2, p3);
            (T1, T2, T3) = (t1, t2, t3);
            TextureObject = textureObject;
        }

        public TextureTriangle(Vertex modelP1, Vertex modelP2, Vertex modelP3, Vector3D textureT1, Vector3D textureT2, Vector3D textureT3, Texture textureObject)
        {
            (ModelP1, ModelP2, ModelP3) = (modelP1, modelP2, modelP3);
            (TextureT1, TextureT2, TextureT3) = (textureT1, textureT2, textureT3);
            TextureObject = textureObject;
        }

        #endregion

        #region Methods

        internal override void ResetVertices()
        {
            (P1, P2, P3) = (ModelP1.Point, ModelP2.Point, ModelP3.Point);
            (T1, T2, T3) = (TextureT1, TextureT2, TextureT3);
        }

        internal override void Interpolator(RenderingObject renderingObject, Action<object, int, int, float> bufferAction)
        {
            // Round the vertices
            int x1 = P1.x.RoundToInt();
            int y1 = P1.y.RoundToInt();
            int x2 = P2.x.RoundToInt();
            int y2 = P2.y.RoundToInt();
            int x3 = P3.x.RoundToInt();
            int y3 = P3.y.RoundToInt();

            // Scale the texture co-ordinates
            int textureWidth = TextureObject.File.Width - 1;
            int textureHeight = TextureObject.File.Height - 1;

            float tx1 = T1.x * textureWidth;
            float ty1 = T1.y * textureHeight;
            float tz1 = T1.z;
            float tx2 = T2.x * textureWidth;
            float ty2 = T2.y * textureHeight;
            float tz2 = T2.z;
            float tx3 = T3.x * textureWidth;
            float ty3 = T3.y * textureHeight;
            float tz3 = T3.z;

            // Sort the vertices by their y-co-ordinate
            NumericManipulation.TexturedSortByY
            (
                ref x1, ref y1, ref tx1, ref ty1, ref tz1,
                ref x2, ref y2, ref tx2, ref ty2, ref tz2,
                ref x3, ref y3, ref tx3, ref ty3, ref tz3
            );

            // Generate z-buffer
            ((Camera)renderingObject).InterpolateTextureTriangle
            (
                TextureObject.File,
                x1, y1, tx1, ty1, tz1,
                x2, y2, tx2, ty2, tz2,
                x3, y3, tx3, ty3, tz3
            );
        }

        #endregion
    }
}