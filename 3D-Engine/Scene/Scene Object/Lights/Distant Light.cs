using System.Drawing;

namespace _3D_Engine
{
    public sealed class Distant_Light// : Light
    {
        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double strength)// : base(origin, direction_forward, direction_up)
        {
        }

        //public Distant_Light(Vector3D origin, Mesh pointed_at, Color? colour, double intensity) : this(origin, pointed_at.World_Origin - origin, colour, intensity) { }
    }
}
