using System.Diagnostics;
using System.Drawing;

namespace _3D_Graphics
{
    public sealed class Line : Mesh
    {
        public Line(Vector3D start_position, Vector3D end_position)
        {
            World_Origin = start_position;
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Vertices = new Vector4D[2]
            {
                new Vector4D(0, 0, 0), // 0
                new Vector4D(1, 1, 1) // 1
            };

            Spots = new Spot[]
            {
                new Spot(Vertices[0]), // 0
                new Spot(Vertices[1]) // 1
            };

            Edges = new Edge[1]
            {
                new Edge(Vertices[0], Vertices[1]) // 0
            };

            Draw_Faces = false;

            Vector3D line_vector = end_position - start_position;
            Scaling = new Vector3D(line_vector.X, line_vector.Y, line_vector.Z);

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;

            Debug.WriteLine($"Line created at {start_position}");
        }
    }
}