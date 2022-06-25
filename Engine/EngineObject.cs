using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Win = System.Windows;

namespace PracticeWork.Engine
{
    [System.AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class EngineObjectConstructorSelecter : System.Attribute { }

    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EngineObjectImportConfiguration : System.Attribute 
    {
        public string PropertyName { get; private init; }
        public EngineObjectImportConfiguration(string property_name) => (this.PropertyName) = (property_name); 
    }

    public abstract class EngineObject : System.Object
    {
        protected  Engine.IEngineScene? LinkedScene { get; private set; } = null;
        private List<Engine.EngineObject> children_list = new List<Engine.EngineObject>(0);

        [Engine.EngineObjectImportConfiguration("ObjectPosition")]
        public System.Drawing.Point Position { get; protected set; } = new Point(0, 0);

        [Engine.EngineObjectImportConfiguration("ObjectGeometry")]
        public System.Drawing.Size Geometry { get; set; } = new Size(10, 10);

        public Engine.EngineObject? ParentObject { get; set; } = null;
        public System.String ObjectName { get; init; }
        protected System.Guid ObjectGuid { get; init; } = Guid.NewGuid();

        protected EngineObject(string object_name) : base() => this.ObjectName = object_name;

        public void ConnectLinkToChildren(Engine.EngineObject new_including_child) 
        {
            if (children_list.Find((target_object) => target_object.Equals(new_including_child)) != null)
                throw new Exception("Children allready contains in object");

            if(new_including_child.Geometry.Equals(new Size(10, 10))) 
                new_including_child.Geometry = this.Geometry;

            new_including_child.SetPosition(new Point(this.Position.X, -this.Position.Y));
            new_including_child.ParentObject = this;

            this.children_list.Add(new_including_child);
        }

        public void ConnectLinkToScene(Engine.IEngineScene scene_instance_link)
        {
            if (this.LinkedScene is not null) throw new Exception("Scene link already set");
            this.LinkedScene = scene_instance_link;
        }

        public void SetPosition(System.Drawing.Point direction) 
        {
            this.Position = new Point(Position.X + direction.X, Position.Y - direction.Y);
            children_list?.ForEach((Engine.EngineObject obj) => obj.SetPosition(direction));
        }

        public abstract void UpdateOperation(System.Drawing.Graphics graphic);
        public virtual void InitialOperation(EngineScene scene_instance) { return; }

        public override bool Equals(object? target_object)
        {
            if(target_object is EngineObject typed) return this.ObjectName == typed.ObjectName;
            throw new Exception("Object equals: different type for argument");
        }

        public override int GetHashCode() => ObjectGuid.GetHashCode();
    }

    public abstract class EngineInputController : EngineObject
    {
        protected EngineInputController(string object_name) : base(object_name) { }

        public abstract void MouseMoveOperation(Win::Forms.MouseEventArgs mouse_arg);
        public abstract void MouseClickOperation(Win::Forms.MouseEventArgs mouse_arg);

        public abstract void KeyInputOperation(Win::Forms.KeyEventArgs key_arg);
        public abstract void KeyReleaseOperation(Win::Forms.KeyEventArgs key_arg);
    }
}
