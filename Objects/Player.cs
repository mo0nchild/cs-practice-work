using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    internal sealed class Player : PracticeWork.Engine.EngineInputController
    {
        public enum MoveDirection : System.Int16 { Positive = 1, Negative = -1, Idle = 0 };

        private System.Drawing.Point cursor_position = new Point(0, 0);
        private Player.MoveDirection move_direction_x = default, move_direction_y = default,
            look_direction = Player.MoveDirection.Positive;

        private Engine.EngineAnimator? player_animator = null;
        private Engine.EngineBoxStaticCollision? player_collision = null;

        [Engine.EngineObjectImportConfiguration("MovementSpeed")]
        public System.Double MovementSpeed { get; protected set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public Player(string object_name) : base(object_name) 
            => this.move_direction_x = this.move_direction_y = Player.MoveDirection.Idle;

        public override void InitialOperation(EngineScene scene_instance)
        {
            this.player_collision = (EngineBoxStaticCollision)(this.LinkedScene?.GetSceneObject("player_colission"))!;
            this.player_animator = (EngineAnimator)(this.LinkedScene?.GetSceneObject("player_animator"))!;
        }

        public override void UpdateOperation(Graphics graphic)
        {
            this.SetPosition(new Point((int)(MovementSpeed * (int)this.move_direction_x),
                (int)(MovementSpeed * (int)this.move_direction_y)));

            if(!Regex.IsMatch(this.player_animator?.AnimationName!, "attack_animation[1-3]{1}") &&
                this.player_animator?.AnimationName != null) 
            {
                if (Math.Abs((int)this.move_direction_x) + Math.Abs((int)this.move_direction_y) > 0)
                {
                    if (this.player_animator!.AnimationName != "run_animation") 
                        this.player_animator?.PlayAnimation("run_animation");
                }
                else
                {
                    if (this.player_animator!.AnimationName != "idle_animation")
                        this.player_animator?.PlayAnimation("idle_animation");
                }
            }
            if ((look_direction == MoveDirection.Positive && this.cursor_position.X < this.Position.X) ||
               (look_direction == MoveDirection.Negative && this.cursor_position.X > this.Position.X))
            {
                this.player_animator?.FlipAllAnimationFrame(ImageExtension.FlipImageDirection.FlipX);
                this.look_direction = (MoveDirection)(-(int)look_direction);
            }
            graphic.DrawEllipse(Pens.Black, cursor_position.X, cursor_position.Y, 10, 10);
        }

        public override void KeyInputOperation(KeyEventArgs key_arg)
        {
            if (key_arg.KeyCode == Keys.Up) this.move_direction_y = MoveDirection.Positive;
            if (key_arg.KeyCode == Keys.Down) this.move_direction_y = MoveDirection.Negative;

            if (key_arg.KeyCode == Keys.Right) this.move_direction_x = MoveDirection.Positive;
            if (key_arg.KeyCode == Keys.Left) this.move_direction_x = MoveDirection.Negative;
        }

        public override void KeyReleaseOperation(KeyEventArgs key_arg)
        {
            if (key_arg.KeyCode == Keys.Up || key_arg.KeyCode == Keys.Down) move_direction_y = MoveDirection.Idle;
            if (key_arg.KeyCode == Keys.Left || key_arg.KeyCode == Keys.Right) move_direction_x = MoveDirection.Idle;
        }

        public override void MouseMoveOperation(MouseEventArgs mouse_arg)
        {
            this.cursor_position = new Point(mouse_arg.X, mouse_arg.Y);
        }

        public override void MouseClickOperation(MouseEventArgs mouse_arg)
        {
            if(!Regex.IsMatch(this.player_animator?.AnimationName!, "attack_animation[1-3]{1}") &&
                this.player_animator?.AnimationName != null)
            {
                this.player_animator!.PlayAnimation("attack_animation" + new Random().Next(1, 4), false);     
            }    
        }
    }
}
