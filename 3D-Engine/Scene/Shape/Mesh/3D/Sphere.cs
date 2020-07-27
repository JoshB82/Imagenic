using System.Diagnostics;

namespace _3D_Engine
{
    public sealed class Sphere : Mesh
    {
        public Sphere(Vector3D origin, Vector3D direction, Vector3D direction_up, double radius, int resolution)
        {
            World_Origin = origin;
            Set_Direction_1(direction, direction_up);

            Debug.WriteLine($"Sphere created at {origin}");
        }
    }
}