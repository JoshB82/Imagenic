using System.Collections.Generic;

namespace _3D_Engine
{
    public partial class Group : Scene_Object
    {
        #region Fields and Properties

        public List<Scene_Object> Scene_Objects { get; set; }

        public override Vector3D World_Origin
        {
            get => base.World_Origin;
            set
            {
                Vector3D displacement = value - World_Origin;
                foreach (Scene_Object scene_object in Scene_Objects) scene_object.Translate(displacement);
                base.World_Origin = value;
            }
        }

        #endregion

        #region Constructors

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        public Group(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, List<Scene_Object> scene_objects) : base(origin, direction_forward, direction_up)
        {
            Scene_Objects = scene_objects;
        }

        #endregion
    }
}