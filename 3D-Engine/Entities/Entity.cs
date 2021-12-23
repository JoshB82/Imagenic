using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine.Entities;

public abstract class Entity
{
    // Id
    private static int nextId;
    /// <summary>
    /// The identification number.
    /// </summary>
    public int Id { get; } = nextId++;

    // Rendering events
    internal event Action RenderAlteringPropertyChanged;
    internal event Action ShadowMapAlteringPropertyChanged;

    internal void InvokeRenderingEvents(bool renderEvent = true, bool shadowMapEvent = true)
    {
        if (renderEvent)
        {
            RenderAlteringPropertyChanged?.Invoke();
        }
        if (shadowMapEvent)
        {
            ShadowMapAlteringPropertyChanged?.Invoke();
        }
    }
}