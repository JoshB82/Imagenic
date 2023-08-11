using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Enums;
using Imagenic.Core.Utilities.Node;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.SceneEntities.RenderingObjects.Cameras;

public struct RenderVolumeParams
{
    #region Fields and Properties

    public float ViewWidth { get; set; } = Defaults.Default.CameraWidth;
    public float ViewHeight { get; set; } = Defaults.Default.CameraHeight;
    public float ZNear { get; set; } = Defaults.Default.CameraZNear;
    public float ZFar { get; set; } = Defaults.Default.CameraZFar;

    #endregion

    #region Constructors

    public RenderVolumeParams() { }

    public RenderVolumeParams(float viewWidth, float viewHeight, float zNear, float zFar)
    {
        ViewWidth = viewWidth;
        ViewHeight = viewHeight;
        ZNear = zNear;
        ZFar = zFar;
    }

    #endregion
}

public static class OrthogonalCameraFactory
{
    private static IEnumerable<Node> GenerateRenderVolumeOutline(RenderVolumeParams volumeParams, VolumeOutline volumeStyle)
    {
        float semiViewWidth = volumeParams.ViewWidth / 2, semiViewHeight = volumeParams.ViewHeight / 2;
        List<DashedEdge> volumeEdges = new();

        if (volumeStyle != VolumeOutline.None)
        {
            Vertex zeroPoint = new(Vector3D.Zero);
            Vertex nearTopLeftPoint = new(new Vector3D(-semiViewWidth, semiViewHeight, volumeParams.ZNear));
            Vertex nearTopRightPoint = new(new Vector3D(semiViewWidth, semiViewHeight, volumeParams.ZNear));
            Vertex nearBottomLeftPoint = new(new Vector3D(-semiViewWidth, -semiViewHeight, volumeParams.ZNear));
            Vertex nearBottomRightPoint = new(new Vector3D(semiViewWidth, -semiViewHeight, volumeParams.ZNear));

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
                Vertex farTopLeftPoint = new(new Vector3D(-semiViewWidth, semiViewHeight, volumeParams.ZFar));
                Vertex farTopRightPoint = new(new Vector3D(semiViewWidth, semiViewHeight, volumeParams.ZFar));
                Vertex farBottomLeftPoint = new(new Vector3D(-semiViewWidth, -semiViewHeight, volumeParams.ZFar));
                Vertex farBottomRightPoint = new(new Vector3D(semiViewWidth, -semiViewHeight, volumeParams.ZFar));

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
    public static Node<OrthogonalCamera> Create(Vector3D worldOrigin,
                                                [DisallowNull] Orientation worldOrientation,
                                                VolumeOutline volumeStyle = VolumeOutline.Far)
    {
        return OrthogonalCameraFactory.Create(worldOrigin, worldOrientation, new(), volumeStyle);
    }

    [return: NotNull]
    public static Node<OrthogonalCamera> Create(Vector3D worldOrigin,
                                                [DisallowNull] Orientation worldOrientation,
                                                RenderVolumeParams volumeParams,
                                                VolumeOutline volumeStyle = VolumeOutline.Far)
    {
        var returnNode = new Node<OrthogonalCamera>(new(worldOrigin, worldOrientation));
        var volumeOutlineNodes = GenerateRenderVolumeOutline(volumeParams, volumeStyle);

        returnNode.AddChildren(volumeOutlineNodes);
        returnNode.Add(new Node<Custom>(ProcessedResources.CameraIcon));

        return returnNode;
    }
}

public static class PerspectiveCameraFactory
{
    private static IEnumerable<Node> GenerateRenderVolumeOutline(RenderVolumeParams volumeParams, VolumeOutline volumeStyle)
    {
        float semiViewWidth = volumeParams.ViewWidth / 2, semiViewHeight = volumeParams.ViewHeight / 2;
        List<DashedEdge> volumeEdges = new();

        Vertex zeroPoint = new(Vector3D.Zero);
        Vertex nearTopLeftPoint = new(new Vector3D(-semiViewWidth, semiViewHeight, volumeParams.ZNear));
        Vertex nearTopRightPoint = new(new Vector3D(semiViewWidth, semiViewHeight, volumeParams.ZNear));
        Vertex nearBottomLeftPoint = new(new Vector3D(-semiViewWidth, -semiViewHeight, volumeParams.ZNear));
        Vertex nearBottomRightPoint = new(new Vector3D(semiViewWidth, -semiViewHeight, volumeParams.ZNear));

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
            //float ratio = (this is OrthogonalCamera or DistantLight) ? 1 : ZFar / ZNear;
            float ratio = volumeParams.ZFar / volumeParams.ZNear;
            float semiViewWidthRatio = semiViewWidth * ratio, semiViewHeightRatio = semiViewHeight * ratio;

            Vertex farTopLeftPoint = new(new Vector3D(-semiViewWidthRatio, semiViewHeightRatio, volumeParams.ZFar));
            Vertex farTopRightPoint = new(new Vector3D(semiViewWidthRatio, semiViewHeightRatio, volumeParams.ZFar));
            Vertex farBottomLeftPoint = new(new Vector3D(-semiViewWidthRatio, -semiViewHeightRatio, volumeParams.ZFar));
            Vertex farBottomRightPoint = new(new Vector3D(semiViewWidthRatio, -semiViewHeightRatio, volumeParams.ZFar));

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

        return volumeEdges.ToNodes();
    }

    [return: NotNull]
    public static Node<PerspectiveCamera> Create(Vector3D worldOrigin,
                                                 [DisallowNull] Orientation worldOrientation,
                                                 VolumeOutline volumeStyle = VolumeOutline.Far)
    {
        return PerspectiveCameraFactory.Create(worldOrigin, worldOrientation, new(), volumeStyle);
    }

    [return: NotNull]
    public static Node<PerspectiveCamera> Create(Vector3D worldOrigin,
                                                [DisallowNull] Orientation worldOrientation,
                                                RenderVolumeParams volumeParams,
                                                VolumeOutline volumeStyle = VolumeOutline.Far)
    {
        var returnNode = new Node<PerspectiveCamera>(new(worldOrigin, worldOrientation));
        var volumeOutlineNodes = GenerateRenderVolumeOutline(volumeParams, volumeStyle);
        
        returnNode.AddChildren(volumeOutlineNodes);
        returnNode.Add(new Node<Custom>(ProcessedResources.CameraIcon));

        return returnNode;
    }
}