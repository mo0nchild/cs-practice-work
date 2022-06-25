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

        public EngineBoxTrigerCollision(string object_name) : base(object_name) { }

        protected abstract void OnTriggerDetectCollision(Engine.EngineObject target_object, CollideSide side);

        public override void UpdateOperation(Graphics graphic)
        {
            this.LinkedScene?.GetSceneObjects<EngineBoxTrigerCollision>().ForEach((target_object) =>
            {
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

            bool is_top_or_bottom = ((Position.Y + Geometry.Height - target_collision.Position.Y < target_collision.Position.X + target_collision.Geometry.Width - Position.X)
                || (target_collision.Position.Y + target_collision.Geometry.Height - Position.Y < target_collision.Position.X + target_collision.Geometry.Width - Position.X))
                && ((target_collision.Position.Y + target_collision.Geometry.Height - Position.Y < Position.X + Geometry.Width - target_collision.Position.X)
                || (Position.Y + Geometry.Height - target_collision.Position.Y < Position.X + Geometry.Width - target_collision.Position.X));

            int corner_delta_x = Math.Min(target_collision.Position.X + target_collision.Geometry.Width - this.Position.X,
                this.Position.Y + this.Geometry.Height - target_collision.Position.Y);

            int corner_delta_y = Math.Min(target_collision.Position.Y + target_collision.Geometry.Height - this.Position.Y,
                this.Position.Y + this.Geometry.Height - target_collision.Position.Y);

            if (check_x == true && !is_top_or_bottom)
            {
                if (target_collision.Position.X + target_collision.Geometry.Width - this.Position.X < 
                   this.Position.X + this.Geometry.Width - target_collision.Position.X) side = CollideSide.Left;
                else side |= CollideSide.Right;
            }

            if(check_y == true && is_top_or_bottom)
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
            //var backup_direction = new Point(this.previous_position.X - this.Position.X,
            //    this.Position.Y - this.previous_position.Y);

            int moving_direction_x = this.Position.X - this.previous_position.X, moving_direction_y = this.previous_position.Y - this.Position.Y;

            if (this.ObjectName == "player_collision")
            {
                if ((side & CollideSide.Left) != CollideSide.None) Console.Write("Left;\t");
                if ((side & CollideSide.Right) != CollideSide.None) Console.Write("Right;\t");
                if ((side & CollideSide.Top) != CollideSide.None) Console.Write("Top;\t");
                if ((side & CollideSide.Bottom) != CollideSide.None) Console.Write("Bottom;\t");
                Console.WriteLine();
            }

            if ((side & CollideSide.Left) != CollideSide.None)
            {
                if (moving_direction_x > 0) moving_direction_x = 0;
                else moving_direction_x = -moving_direction_x;
            }
            if ((side & CollideSide.Right) != CollideSide.None)
            {
                if (moving_direction_x < 0) moving_direction_x = 0;
                else moving_direction_x = -moving_direction_x;
            }
            if ((side & CollideSide.Top) != CollideSide.None)
            {
                if (moving_direction_y < 0) moving_direction_y = 0;
                else moving_direction_y = -moving_direction_y;
            }
            if ((side & CollideSide.Bottom) != CollideSide.None)
            {
                if (moving_direction_y > 0) moving_direction_y = 0;
                else moving_direction_y = -moving_direction_y;
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
