using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;
using Imagenic.Core.Maths.Transformations;
using Imagenic.Core.Utilities.Node;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Imagenic.Core.Entities.PositionedEntities.OrientatedEntities;

public static class OrientatedEntityExtensions
{
    [return: NotNull]
    public static Node<T> AddExtras<T>(this T orientatedEntity, OrientatedEntityExtras extras) where T : OrientatedEntity
    {
        var returnNode = new Node<T>(orientatedEntity);
        
        if (extras.IncludeOrientationArrows)
        {
            returnNode.AddChildren(GenerateOrientationArrows(orientatedEntity));
        }
        if (extras.IncludeOrientationArcs)
        {
            returnNode.AddChildren(); //...
        }

        return returnNode;
    }

    private static IEnumerable<Arrow> GenerateOrientationArrows<T>(T orientatedEntity) where T : OrientatedEntity
    {
        return new Arrow[3]
        {
            new Arrow(orientatedEntity.WorldOrigin, orientatedEntity.WorldOrientation, Defaults.Default.DirectionArrowBodyLength, Defaults.Default.DirectionArrowTipLength, Defaults.Default.DirectionArrowBodyRadius, Defaults.Default.DirectionArrowTipRadius, Defaults.Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Blue),
            new Arrow(orientatedEntity.WorldOrigin, Orientation.CreateOrientationForwardUp(orientatedEntity.WorldOrientation.DirectionUp, -orientatedEntity.WorldOrientation.DirectionForward), Defaults.Default.DirectionArrowBodyLength, Defaults.Default.DirectionArrowTipLength, Defaults.Default.DirectionArrowBodyRadius, Defaults.Default.DirectionArrowTipRadius, Defaults.Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Green),
            new Arrow(orientatedEntity.WorldOrigin, Orientation.CreateOrientationForwardUp(Transform.CalculateDirectionRight(orientatedEntity.WorldOrientation.DirectionForward, orientatedEntity.WorldOrientation.DirectionUp), orientatedEntity.WorldOrientation.DirectionUp), Defaults.Default.DirectionArrowBodyLength, Defaults.Default.DirectionArrowTipLength, Defaults.Default.DirectionArrowBodyRadius, Defaults.Default.DirectionArrowTipRadius, Defaults.Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Red)
        };
    }

    
}

public class OrientatedEntityExtras
{
    public bool IncludeOrientationArcs { get; set; }
    public bool IncludeOrientationArrows { get; set; }
}