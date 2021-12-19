namespace _3D_Engine.Entities.Groups
{
    

    
    
    
    /*
    public partial class Group : SceneObject, IList<SceneObject>
    {
        
        public Group(SceneObject sceneObject) => Add(sceneObject);
        public Group(IEnumerable<SceneObject> sceneObjects) => Add(sceneObjects);
        public Group(params SceneObject[] sceneObjects) => Add(sceneObjects);
        public Group(Group group) => Add(group);
        public Group(IEnumerable<Group> groups) => Add(groups);
        public Group(params Group[] groups) => Add(groups);

        

        #region Methods

        // Add
        public void Add(SceneObject sceneObject)
        {
            SceneObjects.Add(sceneObject);
            switch (sceneObject)
            {
                case Camera camera:
                    Cameras.Add(camera);
                    break;
                case Light light:
                    Lights.Add(light);
                    break;
                case Mesh mesh:
                    Meshes.Add(mesh);
                    break;
                case Group group:
                    Groups.Add(group);
                    break;
            }

            if (RenderCamera is not null)
            {
                RenderCamera.NewRenderNeeded = true;
                sceneObject.RenderCameras.Add(RenderCamera);
            }
        }
        public void Add(IEnumerable<SceneObject> sceneObjects)
        {
            foreach (SceneObject sceneObject in sceneObjects)
            {
                Add(sceneObject);
            }
        }
        public void Add(params SceneObject[] sceneObjects) => Add((IEnumerable<SceneObject>)sceneObjects);
        public void Add(Group group) => Add(group.SceneObjects);
        public void Add(IEnumerable<Group> groups)
        {
            foreach (Group group in groups)
            {
                Add(group);
            }
        }
        public void Add(params Group[] groups) => Add((IEnumerable<Group>)groups);

        // Remove
        //public void RemoveAll(Predicate<SceneObject> predicate) => ;

        
        public void Remove(int id)
        {
            SceneObjects.RemoveAll(x => x.Id == id);
            switch (SceneObjects.Find(x => x.Id == id))
            {
                case Camera camera:
                    Cameras.Remove(camera);
                    break;
                case Light light:
                    Lights.Remove(light);
                    break;
                case Mesh mesh:
                    Meshes.Remove(mesh);
                    break;
                case Group group:
                    Groups.Remove(group);
                    break;
            }
        }
        
        #endregion

    }
    */
}