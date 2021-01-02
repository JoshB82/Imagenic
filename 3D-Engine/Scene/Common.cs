/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for generating an origin and axes.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes.OneDimension;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Scene.CreateOrigin']/*"/>
        public void CreateOrigin() => Add(new WorldPoint(Vector3D.Zero));

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Scene.CreateAxes']/*"/>
        public void CreateAxes()
        {
            // settings??? and elsewhere
            const int resolution = 50, body_radius = 10, tip_radius = 15, tip_length = 50, length = 150;

            Arrow x_axis = new Arrow(Vector3D.Zero, new Vector3D(length, 0, 0), Vector3D.UnitY, body_radius, tip_length, tip_radius, resolution) { FaceColour = Color.Red };
            Arrow y_axis = new Arrow(Vector3D.Zero, new Vector3D(0, length, 0), Vector3D.UnitNegativeZ, body_radius, tip_length, tip_radius, resolution) { FaceColour = Color.Green };
            Arrow z_axis = new Arrow(Vector3D.Zero, new Vector3D(0, 0, length), Vector3D.UnitY, body_radius, tip_length, tip_radius, resolution) { FaceColour = Color.Blue };

            Add(x_axis); Add(y_axis); Add(z_axis);
        }
    }
}