﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a light.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.Transformations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _3D_Engine.SceneObjects.Lights
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Light"/>.
    /// </summary>
    public abstract partial class Light : SceneObject
    {
        #region Fields and Properties

        // Appearance
        public Color Colour { get; set; } = Color.White;
        public float Strength { get; set; }

        public Mesh Icon { get; protected set; }
        /// <summary>
        /// Determines if the <see cref="Light"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool DrawIcon { get; set; } = false;

        public override Vector3D WorldOrigin
        {
            get => base.WorldOrigin; //??x2
            set
            {
                worldOrigin = value;
                ParentScene.
            }
        }

        // View Volume
        private VolumeOutline volumeStyle = VolumeOutline.None;

        public VolumeOutline Volume_Style
        {
            get => volumeStyle;
            set
            {
                volumeStyle = value;

                Volume_Edges.Clear();

                float semi_width = ShadowMapWidth / 2f, semi_height = ShadowMapHeight / 2f;

                Vertex zero_point = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertex near_top_left_point = new Vertex(new Vector4D(-semi_width, semi_height, ShadowMapZNear, 1));
                Vertex near_top_right_point = new Vertex(new Vector4D(semi_width, semi_height, ShadowMapZNear, 1));
                Vertex near_bottom_left_point = new Vertex(new Vector4D(-semi_width, -semi_height, ShadowMapZNear, 1));
                Vertex near_bottom_right_point = new Vertex(new Vector4D(semi_width, -semi_height, ShadowMapZNear, 1));

                if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
                {
                    Volume_Edges.AddRange(new[]
                    {
                        new Edge(zero_point, near_top_left_point), // Near top left
                        new Edge(zero_point, near_top_right_point), // Near top right
                        new Edge(zero_point, near_bottom_left_point), // Near bottom left
                        new Edge(zero_point, near_bottom_right_point), // Near bottom right
                        new Edge(near_top_left_point, near_top_right_point), // Near top
                        new Edge(near_bottom_left_point, near_bottom_right_point), // Near bottom
                        new Edge(near_top_left_point, near_bottom_left_point), // Near left
                        new Edge(near_top_right_point, near_bottom_right_point) // Near right
                    });
                }

                if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
                {
                    float ratio = (this is DistantLight) ? 1 : Shadow_Map_Z_Far / ShadowMapZNear;
                    float semi_width_ratio = semi_width * ratio, semi_height_ratio = semi_height * ratio;

                    Vertex far_top_left_point = new Vertex(new Vector4D(-semi_width_ratio, semi_height_ratio, Shadow_Map_Z_Far, 1));
                    Vertex far_top_right_point = new Vertex(new Vector4D(semi_width_ratio, semi_height_ratio, Shadow_Map_Z_Far, 1));
                    Vertex far_bottom_left_point = new Vertex(new Vector4D(-semi_width_ratio, -semi_height_ratio, Shadow_Map_Z_Far, 1));
                    Vertex far_bottom_right_point = new Vertex(new Vector4D(semi_width_ratio, -semi_height_ratio, Shadow_Map_Z_Far, 1));

                    Volume_Edges.AddRange(new[]
                    {
                        new Edge(near_top_left_point, far_top_left_point), // Far top left
                        new Edge(near_top_right_point, far_top_right_point), // Far top right
                        new Edge(near_bottom_left_point, far_bottom_left_point), // Far bottom left
                        new Edge(near_bottom_right_point, far_bottom_right_point), // Far bottom right
                        new Edge(far_top_left_point, far_top_right_point), // Far top
                        new Edge(far_bottom_left_point, far_bottom_right_point), // Far bottom
                        new Edge(far_top_left_point, far_bottom_left_point), // Far left
                        new Edge(far_top_right_point, far_bottom_right_point) // Far right
                    });
                }
            }
        }

        internal List<Edge> Volume_Edges = new();

        // Matrices
        internal Matrix4x4 WorldToLightView;
        internal Matrix4x4 LightViewToLightScreen;
        internal Matrix4x4 LightScreenToLightWindow;

        internal override void CalculateMatrices()
        {
            base.CalculateMatrices();

            WorldToLightView = ModelToWorld.Inverse();
        }

        // Clipping planes
        internal ClippingPlane[] LightViewClippingPlanes;

        // Shadow map volume
        internal Buffer2D<float> ShadowMap;
        public abstract int ShadowMapWidth { get; set; }
        public abstract int ShadowMapHeight { get; set; }
        public abstract float ShadowMapZNear { get; set; }
        public abstract float Shadow_Map_Z_Far { get; set; }

        private static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0));
        protected void SetShadowMap()
        {
            // Set shadow map
            ShadowMap = new(ShadowMapWidth, ShadowMapHeight);
            
            // Set light-screen-to-light-window matrix
            LightScreenToLightWindow = Transform.Scale(0.5f * (ShadowMapWidth - 1), 0.5f * (ShadowMapHeight - 1), 1) * windowTranslate;
        }

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true) : base(origin, direction_forward, direction_up, has_direction_arrows) { }

        #endregion

        #region Methods

        // Export
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Light.Export_Shadow_Map']/*"/>
        public void ExportShadowMap() => Export_Shadow_Map($"{Directory.GetCurrentDirectory()}\\Export\\{GetType().Name}_{Id}_Export_Map.bmp");

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Light.Export_Shadow_Map(System.String)']/*"/>
        public void Export_Shadow_Map(string file_path)
        {
            Trace.WriteLine($"Generating shadow map for {GetType().Name}...");

            string file_directory = Path.GetDirectoryName(file_path);
            if (!Directory.Exists(file_directory)) Directory.CreateDirectory(file_directory);

            using (Bitmap shadow_map_bitmap = new Bitmap(ShadowMapWidth, ShadowMapHeight))
            {
                for (int x = 0; x < ShadowMapWidth; x++)
                {
                    for (int y = 0; y < ShadowMapHeight; y++)
                    {
                        int value = (255 * ((ShadowMap.Values[x][y] + 1) / 2)).RoundToInt();

                        Color greyscale_colour = Color.FromArgb(255, value, value, value);
                        shadow_map_bitmap.SetPixel(x, y, greyscale_colour);
                    }
                }

                shadow_map_bitmap.Save(file_path, ImageFormat.Bmp);
            }

            Trace.WriteLine($"Successfully saved shadow map for {GetType().Name}");
        }

        #endregion
    }
}