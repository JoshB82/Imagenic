using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using System;

namespace Imagenic.Core.Properties;

public static class ProcessedResources
{
    public static Custom CameraIcon
    {
        get
        {
            var objData = Resources.Camera.Split(Environment.NewLine);
            return new Custom(PositionedEntity.ModelOrigin, Orientation.ModelOrientation, objData);
        }
    }
}