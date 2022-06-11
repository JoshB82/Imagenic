using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities;
using System;

namespace Imagenic.Core.Properties;

public static class ProcessedResources
{
    public static Custom CameraIcon
    {
        get
        {
            var objData = Resources.Camera.Split(Environment.NewLine);
            return new Custom(TranslatableEntity.ModelOrigin, Orientation.ModelOrientation, objData);
        }
    }
}