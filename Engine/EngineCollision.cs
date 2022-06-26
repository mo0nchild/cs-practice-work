using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Engine
{
    public abstract class EngineBoxTrigerCollision : Engine.EngineObject
    {
        public enum CollideSide : System.UInt16 { None = 0, Left, Top, Right = 4, Bottom = 8 };

        [Engine.EngineObjectImportConfiguration("BorderDraw")]
        public System.Boolean BorderDraw { get; protected set; } = false;

        [Engine.EngineObjectImportConfiguration("TypesForCollide")]
        public List<System.Type>? CollisionTargets { get; protected set; } = null;

        public EngineBoxTrigerCollision(string object_name) : base(object_name) { }

        protected abstract void OnTriggerDetectCollision(Engine.EngineObject target_object, CollideSide side);

        public override void UpdateOperation(Graphics graphic)
        {
            this.LinkedScene?.GetSceneObjects<EngineBoxTrigerCollision>().ForEach((target_object) =>
            {
                if (this.CollisionTargets != null)
                {
                    if (this.CollisionTargets?.Find((System.Type searching) =>
                    {
                        return searching == target_object.ParentObject!.GetType();
                    }) == null) return;
                }

                if (this.CollisionChecker((Engine.EngineBoxTrigerCollision)target_object, out var side))
                    this.OnTriggerDetectCollision(target_object, side);
            });
            if (this.BorderDraw) graphic.DrawRectangle(Pens.Red, new Rectangle(this.Position, this.Geometry));   
        }

        private System.Boolean CollisionChecker(EngineBoxTrigerCollision target_collision, out CollideSide side)
        {
            if (target_collision.ObjectName == this.ObjectName) { side = default; return false; }

            bool check_x = (this.Position.X + this.Geometry.Width >= target_collision.Position.X) &&
                (this.Position.X <= target_collision.Position.X + target_collision.Geometry.Width);

            bool check_y = (this.Position.Y + this.Geometry.Height >= target_collision.Position.Y) &&
                (this.Position.Y <= target_collision.Position.Y + target_collision.Geometry.Height);

            side = CollideSide.None;

            int target_delta_y = target_collision.Position.Y + target_collision.Geometry.Height,
                target_delta_x = target_collision.Position.X + target_collision.Geometry.Width;

            int body_delta_y = Position.Y + Geometry.Height, body_delta_x = Position.X + Geometry.Width;

            bool left_topbottom = ((body_delta_y - target_collision.Position.Y < target_delta_x - Position.X)
                || (target_delta_y - Position.Y < target_delta_x - Position.X));

            bool right_topbottom = ((target_delta_y - Position.Y < body_delta_x - target_collision.Position.X)
                || (body_delta_y - target_collision.Position.Y < body_delta_x - target_collision.Position.X));

            int corner_delta_x = Math.Min(target_collision.Position.X + target_collision.Geometry.Width - this.Position.X,
                this.Position.Y + this.Geometry.Height - target_collision.Position.Y);

            int corner_delta_y = Math.Min(target_collision.Position.Y + target_collision.Geometry.Height - this.Position.Y,
                this.Position.Y + this.Geometry.Height - target_collision.Position.Y);

            if (check_x == true && !(left_topbottom && right_topbottom))
            {
                if (target_collision.Position.X + target_collision.Geometry.Width - this.Position.X < 
                   this.Position.X + this.Geometry.Width - target_collision.Position.X) side = CollideSide.Left;
                else side |= CollideSide.Right;
            }

            if(check_y == true && (left_topbottom && right_topbottom))
            {
                if (target_collision.Position.Y + target_collision.Geometry.Height - this.Position.Y <
                   this.Position.Y + this.Geometry.Height - target_collision.Position.Y) side |= CollideSide.Top;
                else side |= CollideSide.Bottom;
            }

            return (check_x && check_y);
        }
    }

    public sealed class EngineBoxStaticCollision : Engine.EngineBoxTrigerCollision
    {
        private System.Drawing.Point previous_position = new Point(0, 0);

        [Engine.EngineObjectConstructorSelecter]
        public EngineBoxStaticCollision(string object_name) : base(object_name) { }

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {
            int moving_direction_x = this.Position.X - this.previous_position.X, 
                moving_direction_y = this.previous_position.Y - this.Position.Y;

            if ((side & CollideSide.Left) != CollideSide.None)
            {
                if (moving_direction_x > 0) moving_direction_x = 0; else moving_direction_x = -moving_direction_x;
            }
            if ((side & CollideSide.Right) != CollideSide.None)
            {
                if (moving_direction_x < 0) moving_direction_x = 0; else moving_direction_x = -moving_direction_x;
            }
            if ((side & CollideSide.Top) != CollideSide.None)
            {
                if (moving_direction_y < 0) moving_direction_y = 0; else moving_direction_y = -moving_direction_y;
            }
            if ((side & CollideSide.Bottom) != CollideSide.None)
            {
                if (moving_direction_y > 0) moving_direction_y = 0; else moving_direction_y = -moving_direction_y;
            }
            this.ParentObject?.SetPosition(new(moving_direction_x, moving_direction_y));
        }

        public override void UpdateOperation(Graphics graphic)
        {
            base.UpdateOperation(graphic);
            this.previous_position = new(this.Position.X, this.Position.Y);
        }
    }
}
