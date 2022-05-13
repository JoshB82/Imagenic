using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Enums;
using Imagenic.Core.Utilities.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities.SceneEntities.RenderingObjects.Cameras;

public struct RenderVolumeParams
{
    public float ViewWidth { get; set; } = Defaults.Default.CameraWidth;
    public float ViewHeight { get; set; } = Defaults.Default.CameraHeight;
    public float ZNear { get; set; } = Defaults.Default.CameraZNear;
    public float ZFar { get; set; } = Defaults.Default.CameraZFar;

    public RenderVolumeParams() { }
}

public struct CameraExtras
{
    public bool IncludeRenderVolumeOutline { get; set; }
    public bool IncludeOrientationArcs { get; set; }
    public bool IncludeOrientationArrows { get; set; }
}

public static class OrthogonalCameraFactory
{
    private static IEnumerable<Node> GenerateRenderVolumeOutline(float viewWidth, float viewHeight, float zNear, float zFar, VolumeOutline volumeStyle)
    {
        float semiViewWidth = viewWidth / 2, semiViewHeight = viewHeight / 2;
        List<DashedEdge> volumeEdges = new();

        if (volumeStyle != VolumeOutline.None)
        {
            Vertex zeroPoint = new(Vector3D.Zero);
            Vertex nearTopLeftPoint = new(new Vector3D(-semiViewWidth, semiViewHeight, zNear));
            Vertex nearTopRightPoint = new(new Vector3D(semiViewWidth, semiViewHeight, zNear));
            Vertex nearBottomLeftPoint = new(new Vector3D(-semiViewWidth, -semiViewHeight, zNear));
            Vertex nearBottomRightPoint = new(new Vector3D(semiViewWidth, -semiViewHeight, zNear));

            if ((volumeStyle & VolumeOutline.Near) == VolumeOutline.Near)
            {
                volumeEdges.AddRange(new DashedEdge[]
                {
                    new DashedEdge(zeroPoint, nearTopLeftPoint), // Near top left
                    new DashedEdge(zeroPoint, nearTopRightPoint), // Near top right
                    new DashedEdge(zeroPoint, nearBottomLeftPoint), // Near bottom left
                    new DashedEdge(zeroPoint, nearBottomRightPoint), // Near bottom right
                    new DashedEdge(nearTopLeftPoint, nearTopRightPoint), // Near top
                    new DashedEdge(nearBottomLeftPoint, nearBottomRightPoint), // Near bottom
                    new DashedEdge(nearTopLeftPoint, nearBottomLeftPoint), // Near left
                    new DashedEdge(nearTopRightPoint, nearBottomRightPoint) // Near right
                });
            }

            if ((volumeStyle & VolumeOutline.Far) == VolumeOutline.Far)
            {
                Vertex farTopLeftPoint = new(new Vector3D(-semiViewWidth, semiViewHeight, zFar));
                Vertex farTopRightPoint = new(new Vector3D(semiViewWidth, semiViewHeight, zFar));
                Vertex farBottomLeftPoint = new(new Vector3D(-semiViewWidth, -semiViewHeight, zFar));
                Vertex farBottomRightPoint = new(new Vector3D(semiViewWidth, -semiViewHeight, zFar));

                volumeEdges.AddRange(new DashedEdge[]
                {
                    new DashedEdge(nearTopLeftPoint, farTopLeftPoint), // Far top left
                    new DashedEdge(nearTopRightPoint, farTopRightPoint), // Far top right
                    new DashedEdge(nearBottomLeftPoint, farBottomLeftPoint), // Far bottom left
                    new DashedEdge(nearBottomRightPoint, farBottomRightPoint), // Far bottom right
                    new DashedEdge(farTopLeftPoint, farTopRightPoint), // Far top
                    new DashedEdge(farBottomLeftPoint, farBottomRightPoint), // Far bottom
                    new DashedEdge(farTopLeftPoint, farBottomLeftPoint), // Far left
                    new DashedEdge(farTopRightPoint, farBottomRightPoint) // Far right
                });
            }
        }

        return volumeEdges.ToNodes();
    }

    [return: NotNull]
    public static Node<OrthogonalCamera> Create(Vector3D worldOrigin, Orientation worldOrientation, VolumeOutline volumeStyle)
    {
        var returnNode = new Node<OrthogonalCamera>(new(worldOrigin, worldOrientation));
        returnNode.Add(new Node<Custom>(ProcessedResources.CameraIcon));
        var volumeOutlineNodes = GenerateRenderVolumeOutline(Defaults.Default.CameraWidth, Defaults.Default.CameraHeight, Defaults.Default.CameraZNear, Defaults.Default.CameraZFar, volumeStyle);
        returnNode.AddChildren(volumeOutlineNodes);

        return returnNode;
    }

    [return: NotNull]
    public static Node<OrthogonalCamera> Create(Vector3D worldOrigin,
                                                Orientation worldOrientation,
                                                float viewWidth,
                                                float viewHeight,
                                                float zNear,
                                                float zFar)
    {
        
        
            new Node<OrthogonalCamera>(new OrthogonalCamera(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar)),
            
        
    }
}