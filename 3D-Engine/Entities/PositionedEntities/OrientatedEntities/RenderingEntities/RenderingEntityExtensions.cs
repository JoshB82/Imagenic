using Imagenic.Core.Utilities.Node;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.RenderingEntities;

public static class RenderingEntityExtensions
{
    [return: NotNull]
    public static Node<T> AddExtras<T>(this T renderingEntity, RenderingEntityExtras extras) where T : RenderingEntity
    {
        var returnNode = OrientatedEntityExtensions.AddExtras(renderingEntity, extras);

        if (extras.IncludeRenderVolumeOutline)
        {
            returnNode.AddChildren(); //...
        }

        return returnNode;
    }
}

public class RenderingEntityExtras : OrientatedEntityExtras
{
    public bool IncludeRenderVolumeOutline { get; set; }
}