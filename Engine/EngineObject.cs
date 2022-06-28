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

            if (new_including_child.ObjectName == "player_damage")
                Console.WriteLine(new_including_child.Position.X + ";\t" + new_including_child.Position.Y);
            new_including_child.SetPosition(new Point(this.Position.X, -this.Position.Y));
            if (new_including_child.ObjectName == "player_damage") 
                Console.WriteLine(new_including_child.Position.X + ";\t" + new_including_child.Position.Y);

            new_including_child.ParentObject = this;

            this.children_list.Add(new_including_child);
        }

        public Engine.EngineObject? GetChildrenObject(string object_name)
            => this.children_list.Find((EngineObject target_object) => target_object.ObjectName == object_name);

        public List<EngineObject> GetChildrenObjects<TObject>() where TObject : EngineObject
            => this.children_list.FindAll((EngineObject target_object) => (target_object as TObject) != null);

        public void ConnectLinkToScene(Engine.IEngineScene scene_instance_link)
        {
            if (this.LinkedScene is not null) throw new Exception("Scene link already set");
            this.LinkedScene = scene_instance_link;
        }

        private object locker_object = new();
        public void SetPosition(System.Drawing.Point direction) 
        {
            lock(locker_object)
            {
                this.Position = new Point(Position.X + direction.X, Position.Y - direction.Y);
                children_list?.ForEach((Engine.EngineObject obj) => obj.SetPosition(direction));
            } 
        }

        public void SetPosition(int pos_x, int pos_y)
        {
            lock (locker_object)
            {
                this.Position = new Point(pos_x, pos_y);
                children_list?.ForEach((Engine.EngineObject obj) => obj.SetPosition(pos_x, pos_y));
            }
        }

        public abstract void PaintingOperation(System.Drawing.Graphics graphic);
        public virtual void InitialOperation(Engine.IEngineScene scene_instance) { return; }
        public virtual void UpdateOperation(Engine.IEngineScene scene_instance) { return; }

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

        public virtual void MouseMoveOperation(Win::Forms.MouseEventArgs mouse_arg) { return; }
        public virtual void MouseInputOperation(Win::Forms.MouseEventArgs mouse_arg) { return; }
        public virtual void MouseReleaseOperation(Win::Forms.MouseEventArgs mouse_arg) { return; }

        public virtual void KeyInputOperation(Win::Forms.KeyEventArgs key_arg) { return; }
        public virtual void KeyReleaseOperation(Win::Forms.KeyEventArgs key_arg) { return; }
    }
}
