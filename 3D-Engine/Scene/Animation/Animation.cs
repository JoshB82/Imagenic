using _3D_Engine.Entities.SceneObjects;
using System.Collections.Generic;
using System.Reflection;

namespace _3D_Engine
{
    class Animation
    {
        public Queue<Change<object>> Changes { get; set; }

        public Animation(Queue<Change<object>> changes)
        {
            Changes = changes;
        }

        public void Run()
        {
            foreach (Change<object> change in Changes)
            {
                SceneObject sceneObject = change.SceneObject;
                PropertyInfo test = sceneObject.GetType().GetProperty(change.Property);
                test.SetValue(sceneObject, change.Start);
            }
        }
    }
}