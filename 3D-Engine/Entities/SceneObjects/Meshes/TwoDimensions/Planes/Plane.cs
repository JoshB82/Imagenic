/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a plane.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions.Planes
{
    /// <summary>
    /// A sealed class representing a two-dimensional plane mesh. It inherits from
    /// the abstract <see cref="Mesh"/> class.
    /// </summary>
    /// <remarks>
    /// Composition:<br/>
    /// <list type="bullet">
    /// <item><description><strong>4</strong> vertices</description></item>
    /// <item><description><strong>4</strong> edges</description></item>
    /// <item><description><strong>1</strong> face (made of <strong>2</strong> triangles)</description></item>
    /// </list>
    /// </remarks>
    public sealed class Plane : Mesh
    {
        #region Fields and Properties

        private float length, width;

        /// <summary>
        /// The length of the <see cref="Plane"/>.
        /// </summary>
        public float Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }
        /// <summary>
        /// The width of the <see cref="Plane"/>.
        /// </summary>
        public float Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Plane"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Plane"/>.</param>
        /// <param name="worldOrientation"></param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        public Plane(Vector3D worldOrigin,
                     Orientation worldOrientation,
                     float length,
                     float width) : base(worldOrigin, worldOrientation, 2)
        {
            Vertices = HardcodedMeshData.PlaneVertices;
            Edges = HardcodedMeshData.PlaneEdges;
            Faces = HardcodedMeshData.PlaneSolidFaces;

            Length = length;
            Width = width;
        }

        /// <summary>
        /// Creates a textured <see cref="Plane"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Plane"/>.</param>
        /// <param name="worldOrientation"></param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Plane"/>.</param>
        public Plane(Vector3D worldOrigin,
                     Orientation worldOrientation,
                     float length,
                     float width,
                     Texture texture) : base(worldOrigin, worldOrientation, 2)
        {
            Vertices = HardcodedMeshData.PlaneVertices;
            Edges = HardcodedMeshData.PlaneEdges;
            Faces = HardcodedMeshData.GeneratePlaneTextureFace(texture);

            Length = length;
            Width = width;
            Textures = new Texture[1] { texture };
        }

        #endregion

        #region Methods

        protected override IList<Vertex> GenerateVertices()
        {

        }

        protected override IList<Edge> GenerateEdges()
        {

        }

        protected override IList<Face> GenerateFaces()
        {

        }

        #endregion

        #region Casting



        #endregion
    }
}