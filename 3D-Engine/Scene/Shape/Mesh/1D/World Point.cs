using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class World_Point : Mesh
    {
        public World_Point(Vector3D origin)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Vertices = new Vector4D[1]
            {
                new Vector4D(0, 0, 0) // 0
            };

            Spots = new Spot[1]
            {
                new Spot(Vertices[0])
            };

            Draw_Edges = false;
            
            Draw_Faces = false;

            Spot_Colour = Color.Blue;

            Debug.WriteLine($"World point created at {origin}");
        }
    }
}