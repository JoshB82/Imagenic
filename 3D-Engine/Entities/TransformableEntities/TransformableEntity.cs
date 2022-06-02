using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System;

namespace Imagenic.Core.Entities.TransformableEntities;

public abstract class TransformableEntity : Entity
{
    public EventList<Transition> Transitions { get; set; } = new();
    public List<Delegate> Transformations { get; set; } = new();

    
    

    
}