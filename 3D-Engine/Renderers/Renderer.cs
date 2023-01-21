﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Renderer<TImage>, representing a renderer that outputs an Image.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities;
using Imagenic.Core.Renderers.Animations;
using Imagenic.Core.Images;
using Imagenic.Core.Images.ImageOptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Imagenic.Core.Utilities.Node;
using _3D_Engine.Entities.SceneObjects.RenderingObjects;
using Imagenic.Core.Maths.Transformations;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Renderers;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TImage"></typeparam>
public abstract class Renderer<TImage> where TImage : Image
{
    #region Fields and Properties

    internal static readonly ClippingPlane[] ScreenClippingPlanes = new ClippingPlane[]
    {
        new(-Vector3D.One, Vector3D.UnitX), // Left
        new(-Vector3D.One, Vector3D.UnitY), // Bottom
        new(-Vector3D.One, Vector3D.UnitZ), // Near
        new(Vector3D.One, Vector3D.UnitNegativeX), // Right
        new(Vector3D.One, Vector3D.UnitNegativeY), // Top
        new(Vector3D.One, Vector3D.UnitNegativeZ) // Far
    };

    internal Buffer2D<float> zBuffer;

    public Matrix4x4 ScreenToWindow { get; private set; }
    protected static readonly Matrix4x4 windowTranslate = Transform.Translate(new Vector3D(1, 1, 0));

    public IImageOptions<TImage> ImageOptions { get; set; }

    private RenderingOptions renderingOptions;
    [DisallowNull]
    public RenderingOptions RenderingOptions
    {
        get => renderingOptions;
        set
        {
            ThrowIfNull(value);
            renderingOptions.RenderSizeChanged -= RenderSizeChangedUpdate;
            renderingOptions = value;
            renderingOptions.RenderSizeChanged += RenderSizeChangedUpdate;
        }
    }

    internal void RenderSizeChangedUpdate(int newRenderWidth, int newRenderHeight)
    {
        zBuffer = new(newRenderWidth, newRenderHeight);
        ScreenToWindow = Transform.Scale(0.5f * (newRenderWidth - 1), 0.5f * (newRenderHeight - 1), 1) * windowTranslate;
    }




    internal bool NewRenderNeeded { get; set; }
    private readonly Action newRenderDelegate;

    private Camera renderCamera;
    public virtual Camera RenderCamera
    {
        get => renderCamera;
        set
        {
            renderCamera = value;
            NewRenderNeeded = true;
        }
    }

    internal List<Triangle> TriangleBuffer { get; set; }

    private SceneEntity sceneObjectsToRender;
    public virtual SceneEntity SceneObjectsToRender
    {
        get => sceneObjectsToRender;
        set
        {
            if (value == sceneObjectsToRender) return;
            if (value is null)
            {
                throw new MessageBuilder<ParameterCannotBeNullException>()
                    .AddParameters(nameof(value))
                    .BuildIntoException<ParameterCannotBeNullException>();
            }

            UpdateSubscribers(sceneObjectsToRender, false);

            sceneObjectsToRender = value;

            UpdateSubscribers(sceneObjectsToRender, true);

            foreach (Triangle triangle in TriangleBuffer)
            {
                triangle.RenderAlteringPropertyChanged -= newRenderDelegate;
            }

            TriangleBuffer.Clear();

            foreach (Mesh child in sceneObjectsToRender.GetAllChildrenAndSelf<Mesh>(x => x.Visible))
            {
                foreach (Face face in child.Structure.Faces)
                {
                    TriangleBuffer.AddRange(face.Triangles);
                    foreach (Triangle triangle in face.Triangles)
                    {
                        triangle.RenderAlteringPropertyChanged += newRenderDelegate;
                    }
                }
            }
        }
    }
    

    #endregion

    #region Constructors

    public Renderer(SceneEntity toRender, Camera renderCamera, RenderingOptions renderingOptions) : this(toRender, renderCamera, renderingOptions, null) { }

    public Renderer(SceneEntity toRender, Camera renderCamera, RenderingOptions renderingOptions, IImageOptions<TImage> imageOptions)
    {
        ThrowIfParameterIsNull(toRender, nameof(toRender));
        ThrowIfParameterIsNull(renderCamera, nameof(renderCamera));
        ThrowIfParameterIsNull(renderingOptions, nameof(renderingOptions));
        ThrowIfParameterIsNull(imageOptions, nameof(imageOptions));

        SceneObjectsToRender = toRender;
        RenderCamera = renderCamera;
        RenderingOptions = renderingOptions;
        ImageOptions = imageOptions;

        newRenderDelegate = () => NewRenderNeeded = true;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Renders a <see cref="PhysicalEntity"/> as a sequence of frames.
    /// </summary>
    /// <param name="physicalEntity">The physical entity being rendered.</param>
    /// <param name="token">A <see cref="CancellationToken"/> that notifies the renderer to cease rendering.</param>
    /// <returns>An <see cref="IAsyncEnumerable{TImage}"/> for all frames.</returns>
    public abstract IAsyncEnumerable<TImage> RenderAsync(PhysicalEntity physicalEntity, CancellationToken token);

    private void UpdateSubscribers(SceneEntity sceneObject, bool addSubscription)
    {
        Action<Entity> updater = addSubscription
            ? s => s.RenderAlteringPropertyChanged += newRenderDelegate
            : s => s.RenderAlteringPropertyChanged -= newRenderDelegate;

        ApplyUpdater(updater, sceneObject);
    }

    protected void ApplyUpdater(Action<Entity> updater, SceneEntity sceneObject)
    {
        sceneObject.ForEach(s => updater(s));
        sceneObject.ForEach<Mesh>(m =>
        {
            updater(m.Structure.Vertices);
            updater(m.Structure.Edges);
            updater(m.Structure.Faces);

            foreach (Vertex vertex in m.Structure.Vertices)
            {
                updater(vertex);
            }
            foreach (Edge edge in m.Structure.Edges)
            {
                updater(edge);
            }
            foreach (Face face in m.Structure.Faces)
            {
                updater(face);
            }
        });
    }

    public abstract Task<TImage> RenderAsync(CancellationToken token);

    public abstract IAsyncEnumerable<TImage> RenderAsync(PhysicalEntity physicalEntity, Animation animation, CancellationToken token);
    public abstract IAsyncEnumerable<TImage> RenderAsync(IEnumerable<PhysicalEntity> physicalEntities, Animation animation, CancellationToken token);
    public abstract IAsyncEnumerable<TImage> RenderAsync(Node<PhysicalEntity> physicalEntityNode, Animation animation, CancellationToken token);
    public abstract IAsyncEnumerable<TImage> RenderAsync(IEnumerable<Node<PhysicalEntity>> physicalEntityNodes, Animation animation, CancellationToken token);

    #endregion
}