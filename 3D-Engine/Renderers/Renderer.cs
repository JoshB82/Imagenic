/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a Renderer<T>, representing a renderer that outputs an Image.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using _3D_Engine.Utilities;
using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public abstract class Renderer<T> where T : Image
{
    #region Fields and Properties

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

    private SceneObject sceneObjectsToRender;
    public virtual SceneObject SceneObjectsToRender
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
    public IImageOptions<T> ImageOptions { get; set; }
    public RenderingOptions RenderingOptions { get; set; }

    #endregion

    #region Constructors

    public Renderer(SceneObject toRender, Camera renderCamera, RenderingOptions renderingOptions) : this(toRender, renderCamera, renderingOptions, null) { }

    public Renderer(SceneObject toRender, Camera renderCamera, RenderingOptions renderingOptions, IImageOptions<T> imageOptions)
    {
        ExceptionHelper.ThrowIfParameterIsNull(toRender, nameof(toRender));
        ExceptionHelper.ThrowIfParameterIsNull(renderCamera, nameof(renderCamera));
        ExceptionHelper.ThrowIfParameterIsNull(renderingOptions, nameof(renderingOptions));
        ExceptionHelper.ThrowIfParameterIsNull(imageOptions, nameof(imageOptions));

        SceneObjectsToRender = toRender;
        RenderCamera = renderCamera;
        RenderingOptions = renderingOptions;
        ImageOptions = imageOptions;

        newRenderDelegate = () => NewRenderNeeded = true;
    }

    #endregion

    #region Methods

    private void UpdateSubscribers(SceneObject sceneObject, bool addSubscription)
    {
        Action<Entity> updater = addSubscription
            ? s => s.RenderAlteringPropertyChanged += newRenderDelegate
            : s => s.RenderAlteringPropertyChanged -= newRenderDelegate;

        ApplyUpdater(updater, sceneObject);
    }

    protected void ApplyUpdater(Action<Entity> updater, SceneObject sceneObject)
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

    public abstract Task<T> RenderAsync(CancellationToken token);

    #endregion
}