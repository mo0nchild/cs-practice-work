using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Engine
{
    public abstract class EngineBoxTrigerCollision : Engine.EngineObject
    {
        [Engine.EngineObjectInportConfiguration("BorderDraw")]
        public System.Boolean BorderDraw { get; private set; } = false;

        public EngineBoxTrigerCollision(string object_name) : base(object_name) { }

        protected abstract void OnTriggerDetectCollision(Engine.EngineObject target_object);

        public override void UpdateOperation(Graphics graphic)
        {
            this.LinkedScene?.GetSceneObjects<EngineBoxTrigerCollision>().ForEach((target_object) =>
            {
                if (this.CollisionChecker((Engine.EngineBoxTrigerCollision)target_object))
                    this.OnTriggerDetectCollision(target_object);
            });
            if (this.BorderDraw) graphic.DrawRectangle(Pens.Green, new Rectangle(this.Position, this.Geometry));   
        }

        private System.Boolean CollisionChecker(EngineBoxTrigerCollision target_collision)
        {
            if (target_collision.ObjectName == this.ObjectName) return false;

            var check_x = (Position.X + Geometry.Width >= target_collision.Position.X) &&
                (Position.X <= target_collision.Position.X + target_collision.Geometry.Width);

            var check_y = (Position.Y + Geometry.Height >= target_collision.Position.Y) &&
                (Position.Y <= target_collision.Position.Y + target_collision.Geometry.Height);

            return check_x && check_y;
        }
    }

    public sealed class EngineBoxStaticCollision : Engine.EngineBoxTrigerCollision
    {
        private System.Drawing.Point previous_position = new Point(0, 0);

        [Engine.EngineObjectConstructorSelecter]
        public EngineBoxStaticCollision(string object_name) : base(object_name) { }

        protected override void OnTriggerDetectCollision(EngineObject target_object)
        {         
            var backup_direction = new Point(this.previous_position.X - this.Position.X,
                this.Position.Y - this.previous_position.Y);

            this.ParentObject?.SetPosition(backup_direction);
        }

        public override void UpdateOperation(Graphics graphic)
        {
            base.UpdateOperation(graphic);
            this.previous_position = new(this.Position.X, this.Position.Y);
        }
    }
}
