﻿using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Point_Light : Light
    {
        public Point_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double intensity) : base(origin, direction_forward, direction_up)
        {
            Translation = origin;

            Icon = null;

            //Intensity = intensity;

            Debug.WriteLine($"Point light created at {origin}");
        }

        //public Point_Light(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double intensity) : this(origin, pointed_at.World_Origin - origin, colour, intensity) { }
    }
}