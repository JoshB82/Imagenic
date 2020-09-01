﻿using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Face"/>.
    /// </summary>
    public sealed class Face
    {
        #region Fields and Properties

        // Positions
        internal Vertex Model_P1 { get; set; }
        internal Vertex Model_P2 { get; set; }
        internal Vertex Model_P3 { get; set; }

        /*
        public Vector3D World_P1 { get; internal set; }
        public Vector3D World_P2 { get; internal set; }
        public Vector3D World_P3 { get; internal set; }
        */

        internal Vector4D P1 { get; set; }
        internal Vector4D P2 { get; set; }
        internal Vector4D P3 { get; set; }
        
        public Vector3D T1 { get; internal set; }
        public Vector3D T2 { get; internal set; }
        public Vector3D T3 { get; internal set; }

        public bool Has_Texture { get; private set; } = false;

        // Appearance
        public Color Colour { get; set; } = Properties.Settings.Default.Face_Colour;
        public Texture Texture_Object { get; set; }
        public bool Draw_Outline { get; set; } = false;
        public bool Visible { get; set; } = true;

        #endregion

        internal void Reset_Vertices()
        {
            P1 = Model_P1.Point;
            P2 = Model_P2.Point;
            P3 = Model_P3.Point;
        }
        internal void Apply_Matrix(Matrix4x4 matrix)
        {
            P1 = matrix * P1;
            P2 = matrix * P2;
            P3 = matrix * P3;
        }

        #region Constructors

        internal Face(Vector4D p1, Vector4D p2, Vector4D p3)
        {
            P1 = p1; P2 = p2; P3 = p3;
        }

        internal Face(Vector4D p1, Vector4D p2, Vector4D p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object)
        {
            P1 = p1; P2 = p2; P3 = p3;
            T1 = t1; T2 = t2; T3 = t3;
            Texture_Object = texture_object;
        }

        public Face(Vertex model_p1, Vertex model_p2, Vertex model_p3)
        {
            Model_P1 = model_p1; Model_P2 = model_p2; Model_P3 = model_p3;
        }

        public Face(Vertex model_p1, Vertex model_p2, Vertex model_p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object)
        {
            Model_P1 = model_p1; Model_P2 = model_p2; Model_P3 = model_p3;
            T1 = t1; T2 = t2; T3 = t3;
            Has_Texture = true; Texture_Object = texture_object;
        }

        #endregion
    }
}