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

        public static readonly Arrow XAxis = new(Vector3D.Zero, new Vector3D(Default.AxisArrowLength, 0, 0), Vector3D.UnitY, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Red };
        public static readonly Arrow YAxis = new(Vector3D.Zero, new Vector3D(0, Default.AxisArrowLength, 0), Vector3D.UnitNegativeZ, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Green };
        public static readonly Arrow ZAxis = new(Vector3D.Zero, new Vector3D(0, 0, Default.AxisArrowLength), Vector3D.UnitY, Default.AxisArrowBodyRadius, Default.AxisArrowTipLength, Default.AxisArrowTipRadius, Default.AxisArrowResolution) { FaceColour = Color.Blue };

        public static readonly Group Axes = new(new List<SceneObject>() { XAxis, YAxis, ZAxis });

        private Vector3D start_position, end_position, unit_vector;

        public Vector3D Start_Position
        {
            get => start_position;
            set
            {
                start_position = value;
                WorldOrigin = start_position;
            }
        }
        public Vector3D End_Position
        {
            get => end_position;
            set
            {
                end_position = value;
                Vector3D line_vector = end_position - start_position;
                unit_vector = line_vector.Normalise();
                length = (line_vector).Magnitude();
                body_length = length - tip_length;
            }   
        }
        public Vector3D Unit_Vector
        {
            get => unit_vector;
            set
            {
                unit_vector = value.Normalise();
                end_position = unit_vector * (body_length + tip_length) + start_position;
            }
        }

        private float body_length, tip_length, length;
        public float Body_Length {
            get => body_length;
            set
            {
                body_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public float Tip_Length
        {
            get => tip_length;
            set
            {
                tip_length = value;
                end_position = unit_vector * (body_length + tip_length) + start_position;
                length = body_length + tip_length;
            }
        }
        public float Length
        {
            get => length;
            set
            {
                length = value;
                end_position = unit_vector * length + start_position;
                body_length = length - tip_length;
            }
        }

        public float Body_Radius { get; set; }
        public float Tip_Radius { get; set; }

        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Arrow(Vector3D startPosition, Vector3D endPosition, Vector3D directionUp, float bodyRadius, float tipLength, float tipRadius, int resolution) : base(startPosition, endPosition - startPosition, directionUp)
        {
            Dimension = 3;

            Start_Position = startPosition;
            Body_Length = (endPosition - startPosition).Magnitude() - tipLength;
            Tip_Length = tipLength;
            End_Position = endPosition;
            Body_Radius = bodyRadius;
            Tip_Radius = tipRadius;
            Resolution = resolution;

            // Vertices are defined in anti-clockwise order.
            Vertices = new Vertex[3 * resolution + 3];
            Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
            Vertices[1] = new Vertex(new Vector4D(Vector3D.UnitZ * body_length, 1));
            Vertices[2] = new Vertex(new Vector4D(Vector3D.UnitZ * (body_length + tipLength), 1));

            float angle = 2 * PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                float sin = Sin(angle * i), cos = Cos(angle * i);
                Vertices[i + 3] = new Vertex(new Vector4D(cos * bodyRadius, sin * bodyRadius, 0, 1));
                Vertices[i + resolution + 3] = new Vertex(new Vector4D(cos * bodyRadius, sin * bodyRadius, body_length, 1));
                Vertices[i + 2 * resolution + 3] = new Vertex(new Vector4D(cos * tipRadius, sin * tipRadius, body_length, 1));
            }

            Edges = new Edge[5 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Edges[i] = new Edge(Vertices[i + 3], Vertices[i + 4]);
                Edges[i + resolution] = new Edge(Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Edges[i + 2 * resolution] = new Edge(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4]);
            }
            Edges[resolution - 1] = new Edge(Vertices[resolution + 2], Vertices[3]);
            Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Edges[3 * resolution - 1] = new Edge(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3]);

            for (int i = 0; i < resolution; i++)
            {
                Edges[i + 3 * resolution] = new Edge(Vertices[i + 3], Vertices[i + resolution + 3]);
                Edges[i + 4 * resolution] = new Edge(Vertices[i + 2 * resolution + 3], Vertices[2]);
            }

            Draw_Edges = false;

            Faces = new Face[6 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new Face(Vertices[i + 3], Vertices[0], Vertices[i + 4]);
                Faces[i + resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 3], Vertices[i + resolution + 4]);
                Faces[i + 2 * resolution] = new Face(Vertices[i + 3], Vertices[i + resolution + 4], Vertices[i + 4]);
                Faces[i + 3 * resolution] = new Face(Vertices[i + resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[i + 2 * resolution + 3]);
                Faces[i + 4 * resolution] = new Face(Vertices[i + resolution + 3], Vertices[i + resolution + 4], Vertices[i + 2 * resolution + 4]);
                Faces[i + 5 * resolution] = new Face(Vertices[i + 2 * resolution + 3], Vertices[i + 2 * resolution + 4], Vertices[2]);
            }
            Faces[resolution - 1] = new Face(Vertices[resolution + 2], Vertices[0], Vertices[3]);
            Faces[2 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[2 * resolution + 2], Vertices[resolution + 3]);
            Faces[3 * resolution - 1] = new Face(Vertices[resolution + 2], Vertices[resolution + 3], Vertices[3]);
            Faces[4 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[2 * resolution + 3], Vertices[3 * resolution + 2]);
            Faces[5 * resolution - 1] = new Face(Vertices[2 * resolution + 2], Vertices[resolution + 3], Vertices[2 * resolution + 3]);
            Faces[6 * resolution - 1] = new Face(Vertices[3 * resolution + 2], Vertices[2 * resolution + 3], Vertices[2]);
        }

        public Arrow(Vector3D startPosition, Vector3D unitVector, Vector3D directionUp, float bodyLength, float bodyRadius, float tipLength, float tipRadius, int resolution) : this(startPosition, unitVector * (bodyLength + tipLength) + startPosition, directionUp, bodyRadius, tipLength, tipRadius, resolution) { }

        #endregion
    }
}