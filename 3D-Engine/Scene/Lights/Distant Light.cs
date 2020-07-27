using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Distant_Light : Light
    {
        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double intensity) : base(origin, direction_forward, direction_up)
        {
            Translation = origin;
            World_Origin = origin;

            //Intensity = intensity;

            Debug.WriteLine($"Distant light created at {origin}");
        }

        //public Distant_Light(Vector3D origin, Mesh pointed_at, Color? colour, double intensity) : this(origin, pointed_at.World_Origin - origin, colour, intensity) { }
    }
}
