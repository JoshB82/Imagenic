using Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions;

namespace Imagenic.Core.Utilities.Messages;

internal class MeshCreatedMessage : PhysicalEntityCreatedMessage, IMessage<MeshCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<MeshCreatedMessage> BriefText => $"Mesh created";
    public static new MessageInterpolatedStringHandler<MeshCreatedMessage> DetailedText => $"Mesh created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<MeshCreatedMessage> AllText => $"Mesh created with parameters {{{0}}}.";
}

internal class ArrowCreatedMessage : MeshCreatedMessage, IMessage<ArrowCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<ArrowCreatedMessage> BriefText => $"Arrow created.";
    public static new MessageInterpolatedStringHandler<ArrowCreatedMessage> DetailedText => $"Arrow created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<ArrowCreatedMessage> AllText => $"Arrow created with parameters {{{0}}}.";
}

internal class ConeCreatedMessage : MeshCreatedMessage, IMessage<ConeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<ConeCreatedMessage> BriefText => $"Cone created.";
    public static new MessageInterpolatedStringHandler<ConeCreatedMessage> DetailedText => $"Cone created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<ConeCreatedMessage> AllText => $"Cone created with parameters {{{0}}}.";
}

internal class CubeCreatedMessage : MeshCreatedMessage, IMessage<CubeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<CubeCreatedMessage> BriefText => $"Cube created.";
    public static new MessageInterpolatedStringHandler<CubeCreatedMessage> DetailedText => $"Cube created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<CubeCreatedMessage> AllText => $"Cube created with parameters {{{0}}}.";
}

internal class CylinderCreatedMessage : MeshCreatedMessage, IMessage<CylinderCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<CylinderCreatedMessage> BriefText => $"Cylinder created.";
    public static new MessageInterpolatedStringHandler<CylinderCreatedMessage> DetailedText => $"Cylinder created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<CylinderCreatedMessage> AllText => $"Cylinder created with parameters {{{0}}}.";
}

internal class PathCreatedMessage : MeshCreatedMessage, IMessage<PathCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<PathCreatedMessage> BriefText => $"Path created.";
    public static new MessageInterpolatedStringHandler<PathCreatedMessage> DetailedText => $"Path created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<PathCreatedMessage> AllText => $"Path created with parameters {{{0}}}.";
}

internal class CuboidCreatedMessage : MeshCreatedMessage, IMessage<CuboidCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<CuboidCreatedMessage> BriefText => $"Cuboid created.";
    public static new MessageInterpolatedStringHandler<CuboidCreatedMessage> DetailedText => $"Cuboid created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<CuboidCreatedMessage> AllText => $"Cuboid created with parameters {{{0}}}.";
}

internal class SquareCreatedMessage : MeshCreatedMessage, IMessage<SquareCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<SquareCreatedMessage> BriefText => $"Square created.";
    public static new MessageInterpolatedStringHandler<SquareCreatedMessage> DetailedText => $"Square created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<SquareCreatedMessage> AllText => $"Square created with parameters {{{0}}}.";
}

internal class PlaneCreatedMessage : MeshCreatedMessage, IMessage<PlaneCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<PlaneCreatedMessage> BriefText => $"Plane created.";
    public static new MessageInterpolatedStringHandler<PlaneCreatedMessage> DetailedText => $"Plane created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<PlaneCreatedMessage> AllText => $"Plane created with parameters {{{0}}}.";
}

internal class Text2DCreatedMessage : MeshCreatedMessage, IMessage<Text2DCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<Text2DCreatedMessage> BriefText => $"Text2D created.";
    public static new MessageInterpolatedStringHandler<Text2DCreatedMessage> DetailedText => $"Text2D created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<Text2DCreatedMessage> AllText => $"Text2D created with parameters {{{0}}}.";
}

internal class Text3DCreatedMessage : MeshCreatedMessage, IMessage<Text3DCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<Text3DCreatedMessage> BriefText => $"Text3D created.";
    public static new MessageInterpolatedStringHandler<Text3DCreatedMessage> DetailedText => $"Text3D created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<Text3DCreatedMessage> AllText => $"Text3D created with parameters {{{0}}}.";
}

internal class RingCreatedMessage : MeshCreatedMessage, IMessage<RingCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<RingCreatedMessage> BriefText => $"Ring created.";
    public static new MessageInterpolatedStringHandler<RingCreatedMessage> DetailedText => $"Ring created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<RingCreatedMessage> AllText => $"Ring created with parameters {{{0}}}.";
}

internal class BézierCurveCreatedMessage : MeshCreatedMessage, IMessage<BézierCurveCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<BézierCurveCreatedMessage> BriefText => $"Ring created.";
    public static new MessageInterpolatedStringHandler<BézierCurveCreatedMessage> DetailedText => $"Ring created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<BézierCurveCreatedMessage> AllText => $"Ring created with parameters {{{0}}}.";
}

internal class CircleCreatedMessage : MeshCreatedMessage, IMessage<CircleCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<CircleCreatedMessage> BriefText => $"Circle created.";
    public static new MessageInterpolatedStringHandler<CircleCreatedMessage> DetailedText => $"Circle created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<CircleCreatedMessage> AllText => $"Circle created with parameters {{{0}}}.";
}

internal class EllipseCreatedMessage : MeshCreatedMessage, IMessage<EllipseCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<EllipseCreatedMessage> BriefText => $"Ellipse created.";
    public static new MessageInterpolatedStringHandler<EllipseCreatedMessage> DetailedText => $"Ellipse created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<EllipseCreatedMessage> AllText => $"Ellipse created with parameters {{{0}}}.";
}

internal class TorusCreatedMessage : MeshCreatedMessage, IMessage<TorusCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<TorusCreatedMessage> BriefText => $"Torus created.";
    public static new MessageInterpolatedStringHandler<TorusCreatedMessage> DetailedText => $"Torus created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<TorusCreatedMessage> AllText => $"Torus created with parameters {{{0}}}.";
}

internal class WorldPointCreatedMessage : MeshCreatedMessage, IMessage<WorldPointCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<WorldPointCreatedMessage> BriefText => $"WorldPoint created.";
    public static new MessageInterpolatedStringHandler<WorldPointCreatedMessage> DetailedText => $"WorldPoint created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<WorldPointCreatedMessage> AllText => $"WorldPoint created with parameters {{{0}}}.";
}

internal class LineCreatedMessage : MeshCreatedMessage, IMessage<LineCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<LineCreatedMessage> BriefText => $"Line created.";
    public static new MessageInterpolatedStringHandler<LineCreatedMessage> DetailedText => $"Line created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<LineCreatedMessage> AllText => $"Line created with parameters {{{0}}}.";
}