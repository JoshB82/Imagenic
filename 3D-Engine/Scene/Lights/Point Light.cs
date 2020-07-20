using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Point_Light : Light
    {
        public Point_Light(Vector3D origin, Vector3D direction, Color? colour, double intensity)
        {
            Translation = origin;
            World_Origin = origin;
            World_Direction = direction;
            Colour = colour ?? Color.White;
            //Intensity = intensity;

            Debug.WriteLine($"Point light created at {origin}");
        }

        public Point_Light(Vector3D origin, Mesh pointed_at, Color? colour, double intensity) : this(origin, pointed_at.World_Origin - origin, colour, intensity) { }
    }
}
