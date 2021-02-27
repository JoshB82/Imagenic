/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of an arrow.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes.Components;
using System.Collections.Generic;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.Meshes.ThreeDimensions
{
    public sealed class Arrow : Mesh
    {
        #region Fields and Properties

        // Axes
        public static readonly Arrow XAxis = new(Vector3D.Zero, new Vector3D(Default.AxisArrowLength, 0, 0), Vector3D.UnitY, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Red };
        public static readonly Arrow YAxis = new(Vector3D.Zero, new Vector3D(0, Default.AxisArrowLength, 0), Vector3D.UnitNegativeZ, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Green };
        public static readonly Arrow ZAxis = new(Vector3D.Zero, new Vector3D(0, 0, Default.AxisArrowLength), Vector3D.UnitY, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Blue };

        public static readonly Group Axes = new(new List<SceneObject>() { XAxis, YAxis, ZAxis });

        private Vector3D tipPosition;
        private float length, bodyLength, tipLength, bodyRadius, tipRadius;
        private int resolution;

        public override Vector3D WorldDirectionForward
        {
            get => base.WorldDirectionForward;
            protected set
            {
                base.WorldDirectionForward = value;

                tipPosition = WorldDirectionForward * length;
            }
        }
        public Vector3D TipPosition
        {
            get => tipPosition;
            set
            {
                tipPosition = value;

                length = (tipPosition - WorldOrigin).Magnitude();
                bodyLength = length - tipLength;

                UpdateRenderCamera();
            }
        }
        public float Length
        {
            get => length;
            set
            {
                length = value;

                tipPosition = WorldDirectionForward * length;
                bodyLength = length = tipLength;

                UpdateRenderCamera();
            }
        }
        public float BodyLength
        {
            get => bodyLength;
            set
            {
                bodyLength = value;

                length = bodyLength + tipLength;
                tipPosition = WorldOrigin + WorldDirectionForward * length;

                GenerateVertices();

                UpdateRenderCamera();
            }
        }
        public float TipLength
        {
            get => tipLength;
            set
            {
                tipLength = value;

                length = bodyLength + tipLength;
                tipPosition = WorldOrigin + WorldDirectionForward * length;

                GenerateVertices();

                UpdateRenderCamera();
            }
        }
        public float BodyRadius
        {
            get => bodyRadius;
            set
            {
                bodyRadius = value;

                GenerateVertices();

                UpdateRenderCamera();
            }
        }
        public float TipRadius
        {
            get => tipRadius;
            set
            {
                tipRadius = value;

                GenerateVertices();

                UpdateRenderCamera();
            }
        }
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();

                UpdateRenderCamera();
            }
        }

        #endregion

        #region Constructors

        /*
        internal Arrow(Vector3D startPosition, Vector3D endPosition, Vector3D directionUp, float bodyRadius, float tipLength, float tipRadius, int resolution, bool hasDirectionArrows) : base(startPosition, endPosition - startPosition, directionUp, hasDirectionArrows) { }
        */

        internal Arrow(Vector3D worldOrigin, Vector3D directionForward, Vector3D directionUp, float bodyLength, float tipLength, float bodyRadius, float tipRadius, int resolution, bool hasDirectionArrows) : base(worldOrigin, directionForward, directionUp, hasDirectionArrows)
        {
            Dimension = 3;

            this.length = bodyLength + tipLength;
            this.tipPosition = worldOrigin + directionForward * this.length;
            this.bodyLength = bodyLength;
            this.tipLength = tipLength;
            this.bodyRadius = bodyRadius;
            this.tipRadius = tipRadius;
            this.resolution = resolution;

            GenerateVertices();
            GenerateEdges();
            GenerateFaces();
        }
        public Arrow(Vector3D worldOrigin, Vector3D directionForward, Vector3D directionUp, float bodyLength, float tipLength, float bodyRadius, float tipRadius, int resolution) : this(worldOrigin, directionForward, directionUp, bodyLength, tipLength, bodyRadius, tipRadius, resolution, true) { }

        //

        /*
        public Arrow(Vector3D startPosition, Vector3D endPosition, Vector3D directionUp, float bodyRadius, float tipLength, float tipRadius, int resolution) : this(startPosition, endPosition, directionUp, bodyRadius, tipLength, tipRadius, resolution, false) { }

        public Arrow(Vector3D startPosition, Vector3D unitVector, Vector3D directionUp, float bodyLength, float bodyRadius, float tipLength, float tipRadius, int resolution) : this(startPosition, unitVector * (bodyLength + tipLength) + startPosition, directionUp, bodyRadius, tipLength, tipRadius, resolution) { }
        */

        #endregion

        #region Methods

        private void GenerateVertices()
        {
            Vertices = new Vertex[3 * resolution + 3];
            Vertices[0] = new(new Vector4D(0, 0, 0, 1));
            Vertices[1] = new(new Vector4D(Vector3D.UnitZ * bodyLength, 1));
            Vertices[2] = new(new Vector4D(Vector3D.UnitZ * (bodyLength + tipLength), 1));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                float sin = Sin(angle * i), cos = Cos(angle * i);
                Vertices[i + 3] = new(new Vector4D(cos * bodyRadius, sin * bodyRadius, 0, 1));
                Vertices[i + resolution + 3] = new(new Vector4D(cos * bodyRadius, sin * bodyRadius, bodyLength, 1));
                Vertices[i + 2 * resolution + 3] = new(new Vector4D(cos * tipRadius, sin * tipRadius, bodyLength, 1));
            }
        }
        private void GenerateEdges()
        {
            Edges = new Edge[5 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Edges[i] = new(Vertices[i + 3], Vertices[i + 4]);
                Edges[i + resolution] = new(Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Edges[i + 2 * resolution] = new(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4]);
            }
            Edges[resolution - 1] = new(Vertices[resolution + 2], Vertices[3]);
            Edges[2 * resolution - 1] = new(Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Edges[3 * resolution - 1] = new(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3]);

            for (int i = 0; i < resolution; i++)
            {
                Edges[i + 3 * resolution] = new(Vertices[i + 3], Vertices[i + resolution + 3]);
                Edges[i + 4 * resolution] = new(Vertices[i + 2 * resolution + 3], Vertices[2]);
            }

            DrawEdges = false;
        }
        private void GenerateFaces()
        {
            Faces = new Face[6 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new(Vertices[i + 3], Vertices[0], Vertices[i + 4]);
                Faces[i + resolution] = new(Vertices[i + 3], Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Faces[i + 2 * resolution] = new(Vertices[i + 3], Vertices[i + resolution + 4], Vertices[i + 4]);
                Faces[i + 3 * resolution] = new(Vertices[i + resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[i + 2 * resolution + 3]);
                Faces[i + 4 * resolution] = new(Vertices[i + resolution + 3], Vertices[i + resolution + 4], Vertices[i + 2 * resolution + 4]);
                Faces[i + 5 * resolution] = new(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[2]);
            }
            Faces[resolution - 1] = new(Vertices[resolution + 2], Vertices[0], Vertices[3]);
            Faces[2 * resolution - 1] = new(Vertices[resolution + 2], Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Faces[3 * resolution - 1] = new(Vertices[resolution + 2], Vertices[resolution + 3], Vertices[3]);
            Faces[4 * resolution - 1] = new(Vertices[2 * resolution + 2], Vertices[2 * resolution + 3], Vertices[3 * resolution + 2]);
            Faces[5 * resolution - 1] = new(Vertices[2 * resolution + 2], Vertices[resolution + 3], Vertices[2 * resolution + 3]);
            Faces[6 * resolution - 1] = new(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3], Vertices[2]);
        }

        #endregion
    }
}