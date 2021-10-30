﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an arrow mesh.
 */

using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
{
    public sealed class Arrow : Mesh
    {
        #region Fields and Properties

        // Axes
        public static readonly Arrow ZAxis = (new Arrow(Vector3D.Zero, Vector3D.UnitZ, Vector3D.UnitY, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution)).ColourAllSolidFaces(Color.Blue);
        public static readonly Arrow YAxis = (new Arrow(Vector3D.Zero, Vector3D.UnitY, Vector3D.UnitNegativeZ, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution)).ColourAllSolidFaces(Color.Green);
        public static readonly Arrow XAxis = (new Arrow(Vector3D.Zero, Vector3D.UnitX, Vector3D.UnitY, Default.AxisArrowBodyLength, Default.AxisArrowTipLength, Default.AxisArrowBodyRadius, Default.AxisArrowTipRadius, Default.AxisArrowResolution)).ColourAllSolidFaces(Color.Red);

        public static readonly Group Axes = new(XAxis, YAxis, ZAxis);

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
                if (value == tipPosition) return;
                tipPosition = value;
                RequestNewRenders();

                length = (tipPosition - WorldOrigin).Magnitude();
                bodyLength = length - tipLength;
            }
        }

        public float Length
        {
            get => length;
            set
            {
                if (value == length) return;
                length = value;
                RequestNewRenders();

                tipPosition = WorldDirectionForward * length;
                bodyLength = length = tipLength;
            }
        }

        public float BodyLength
        {
            get => bodyLength;
            set
            {
                if (value == bodyLength) return;
                bodyLength = value;
                RequestNewRenders();

                length = bodyLength + tipLength;
                tipPosition = WorldOrigin + WorldDirectionForward * length;

                GenerateVertices();
            }
        }

        public float TipLength
        {
            get => tipLength;
            set
            {
                if (value == tipLength) return;
                tipLength = value;
                RequestNewRenders();

                length = bodyLength + tipLength;
                tipPosition = WorldOrigin + WorldDirectionForward * length;

                GenerateVertices();
            }
        }

        public float BodyRadius
        {
            get => bodyRadius;
            set
            {
                if (value == bodyRadius) return;
                bodyRadius = value;
                RequestNewRenders();

                GenerateVertices();
            }
        }

        public float TipRadius
        {
            get => tipRadius;
            set
            {
                if (value == tipRadius) return;
                tipRadius = value;
                RequestNewRenders();

                GenerateVertices();
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

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();
            }
        }

        #endregion

        #region Constructors

        internal Arrow(Vector3D worldOrigin,
                       Orientation worldOrientation,
                       float bodyLength,
                       float tipLength,
                       float bodyRadius,
                       float tipRadius,
                       int resolution,
                       bool hasDirectionArrows) : base(worldOrigin, worldOrientation, 3, hasDirectionArrows)
        {
            this.length = bodyLength + tipLength;
            this.tipPosition = worldOrigin + worldOrientation.DirectionForward * this.length;
            this.bodyLength = bodyLength;
            this.tipLength = tipLength;
            this.bodyRadius = bodyRadius;
            this.tipRadius = tipRadius;
            this.resolution = resolution;

            GenerateVertices();
            GenerateEdges();
            GenerateFaces();
        }

        public Arrow(Vector3D worldOrigin,
                     Orientation worldOrientation,
                     float bodyLength,
                     float tipLength,
                     float bodyRadius,
                     float tipRadius,
                     int resolution) : this(worldOrigin, worldOrientation, bodyLength, tipLength, bodyRadius, tipRadius, resolution, true) { }

        public static Arrow ArrowTipPosition(Vector3D worldOrigin,
                                             Vector3D tipPosition,
                                             Vector3D directionUp,
                                             float bodyLength,
                                             float tipLength,
                                             float bodyRadius,
                                             float tipRadius,
                                             int resolution) => new(worldOrigin, tipPosition - worldOrigin, directionUp, bodyLength, tipLength, bodyRadius, tipRadius, resolution, true);

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
            Triangles = new SolidTriangle[6 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Triangles[i] = new SolidTriangle(Vertices[i + 3], Vertices[0], Vertices[i + 4]);
                Triangles[i + resolution] = new SolidTriangle(Vertices[i + 3], Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Triangles[i + 2 * resolution] = new SolidTriangle(Vertices[i + 3], Vertices[i + resolution + 4], Vertices[i + 4]);
                Triangles[i + 3 * resolution] = new SolidTriangle(Vertices[i + resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[i + 2 * resolution + 3]);
                Triangles[i + 4 * resolution] = new SolidTriangle(Vertices[i + resolution + 3], Vertices[i + resolution + 4], Vertices[i + 2 * resolution + 4]);
                Triangles[i + 5 * resolution] = new SolidTriangle(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[2]);
            }
            Triangles[resolution - 1] = new SolidTriangle(Vertices[resolution + 2], Vertices[0], Vertices[3]);
            Triangles[2 * resolution - 1] = new SolidTriangle(Vertices[resolution + 2], Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Triangles[3 * resolution - 1] = new SolidTriangle(Vertices[resolution + 2], Vertices[resolution + 3], Vertices[3]);
            Triangles[4 * resolution - 1] = new SolidTriangle(Vertices[2 * resolution + 2], Vertices[2 * resolution + 3], Vertices[3 * resolution + 2]);
            Triangles[5 * resolution - 1] = new SolidTriangle(Vertices[2 * resolution + 2], Vertices[resolution + 3], Vertices[2 * resolution + 3]);
            Triangles[6 * resolution - 1] = new SolidTriangle(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3], Vertices[2]);
        }

        #endregion
    }
}