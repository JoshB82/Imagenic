﻿using System;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Spotlight : Light
    {
        private double angle, radius;
        public double Angle
        {
            get => angle;
            set
            {
                angle = value;
                radius = Math.Tan(value / 2) * Distance;
            }
        }
        public double Radius
        {
            get => radius;
            set
            {
                angle = Math.Atan2(value, Distance) * 2;
                radius = value;
            }
        }

        public double Distance { get; set; }

        public Spotlight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double intensity, double angle, double distance) : base(origin, direction_forward, direction_up)
        {
            Translation = origin;
            World_Origin = origin;

            //Intensity = intensity;
            Angle = angle;
            Distance = distance;

            Debug.WriteLine($"Spotlight light created at {origin}");
        }

        //public Spotlight(Vector3D origin, Vector3D direction, Color? colour, string ignore, double intensity, double radius, double distance) : this(origin, direction, colour, intensity, Math.Atan2(radius, distance) * 2, distance) { }
    }
}