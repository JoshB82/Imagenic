namespace _3D_Engine
{
    class State<T>
    {
        
        public T Value { get; set; }

        public State()
        {
            
        }
    }

    class Change<T>
    {
        public SceneObject SceneObject { get; set; }
        public string Property { get; set; }

        public T Start { get; set; }
        public T Finish { get; set; }

        public float Duration { get; set; }

        public Change(SceneObject scene_object, string property, T start_value, T finish_value, float duration)
        {
            SceneObject = scene_object;
            Property = property;
            Start = start_value;
            Finish = finish_value;
            Duration = duration;
        }
    }
}