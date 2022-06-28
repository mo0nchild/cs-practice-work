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
        private Objects.PlayerHitRegistrator? hit_registrator = default;

        [Engine.EngineObjectImportConfiguration("LifeCount")]
        public System.Int32 LifeCount { get; private set; } = 3;
        public System.Boolean IsAlive { get; private set; } = true;
        public System.Boolean IsReadyToAttack { get; set; } = true;

        [Engine.EngineObjectImportConfiguration("MovementSpeed")]
        public System.Double MovementSpeed { get; protected set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public Player(string object_name) : base(object_name) 
            => this.move_direction_x = this.move_direction_y = Player.MoveDirection.Idle;

        public override void InitialOperation(IEngineScene scene_instance)
        {
            this.player_collision = (EngineBoxStaticCollision)(this.LinkedScene?.GetSceneObject("player_colission"))!;
            this.player_animator = (EngineAnimator)(this.LinkedScene?.GetSceneObject("player_animator"))!;
            this.hit_registrator = (PlayerHitRegistrator)(this.LinkedScene?.GetSceneObject("player_hit")!);
        }

        public void DamageRegistration()
        {
            if (this.IsAlive)
            {
                if (--this.LifeCount <= 0)
                {
                    this.player_animator?.PlayAnimation("death_animation", false);
                    this.IsAlive = false;
                }
                else this.player_animator?.PlayAnimation("damage_animation", false);
            }
            
        }

        public override void UpdateOperation(IEngineScene scene_instance)
        {
            double current_speed = this.MovementSpeed;
            if ((int)look_direction > 0 && this.move_direction_x < 0 || (int)look_direction < 0 && this.move_direction_x > 0)
                current_speed /= 1.5;

            if(this.IsAlive) this.SetPosition(new Point((int)(current_speed * (int)this.move_direction_x),
                (int)(current_speed * (int)this.move_direction_y)));

            if (!this.IsAlive && this.player_animator?.AnimationName != "death_animation")
            {
                this.LinkedScene?.ExitSceneHandler();
            }

            if (!Regex.IsMatch(this.player_animator?.AnimationName!, "attack_animation[1-3]{1}") )
            {
                if(hit_registrator.HitInstalled) this.hit_registrator.HitRegistration(false);
                this.IsReadyToAttack = true;
            }

            if (!Regex.IsMatch(this.player_animator?.AnimationName!, "attack_animation[1-3]{1}") &&
                this.player_animator?.AnimationName != "damage_animation" && this.IsAlive)
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
        }

        public override void PaintingOperation(Graphics graphic) { }

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

        public override void MouseReleaseOperation(MouseEventArgs mouse_arg)
        {
            if (!Regex.IsMatch(this.player_animator?.AnimationName!, "attack_animation[1-3]{1}") &&
                this.player_animator?.AnimationName != null)
            {
                this.player_animator!.PlayAnimation("attack_animation" + new Random().Next(1, 4), false);
                this.IsReadyToAttack = false;
            }
        }
    }

    internal sealed class PlayerAttackCursor : PracticeWork.Engine.EngineInputController
    {
        private System.Drawing.Image look_cursor_image, attack_cursor_image;
        private Objects.PlayerHitRegistrator? hit_registrator = default;
        private Objects.Player? player_instance = default;
        
        private System.Boolean is_attacking = default;
        private System.Drawing.Point cursor_position = new Point(0, 0);

        [Engine.EngineObjectImportConfiguration("MaxAttackRadius")]
        public System.Double MaxAttackRadius { get; private set; } = 100.0;

        [Engine.EngineObjectConstructorSelecter]
        public PlayerAttackCursor(string object_name) : base(object_name) 
        {
            this.look_cursor_image = Image.FromFile(@"..\..\..\Assets\Interface\look_cursor.png");
            this.attack_cursor_image = Image.FromFile(@"..\..\..\Assets\Interface\sword_cursor.png");
        }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            this.hit_registrator = (PlayerHitRegistrator?)this.GetChildrenObjects<PlayerHitRegistrator>()[0];
            this.player_instance = (Player?)this.LinkedScene?.GetSceneObject("player");
        }

        public override void MouseMoveOperation(MouseEventArgs mouse_arg)
        {
            this.cursor_position = new Point(mouse_arg.X, mouse_arg.Y);
        }

        public override void MouseInputOperation(MouseEventArgs mouse_arg) => this.is_attacking = true;

        public override void MouseReleaseOperation(MouseEventArgs mouse_arg)
        {
            if (this.player_instance!.IsReadyToAttack) this.hit_registrator?.HitRegistration();
            this.is_attacking = false;
        }

        private System.Drawing.Point center = new();
        private System.Double angle = default, direction = default;

        public override void UpdateOperation(IEngineScene scene_instance)
        {
            double delta_h = ((this.Position.Y + this.Geometry.Height / 2.0) - this.cursor_position.Y),
                delta_w = (this.cursor_position.X - (this.Position.X + this.Geometry.Width / 2.0));

            double R = (Math.Sqrt(Math.Pow(delta_w, 2) + Math.Pow(delta_h, 2)));
            R = (R < this.MaxAttackRadius) ? R : this.MaxAttackRadius;

            this.angle = (delta_w == 0) ? 1 : Math.Atan(delta_h / delta_w);
            this.center = new((int)(this.Position.X + this.Geometry.Width / 2.0), (int)(this.Position.Y + this.Geometry.Height / 2.0));

            this.direction = (this.cursor_position.X - center.X > 0) ? 1 : -1;

            this.hit_registrator?.SetPosition(center.X + (int)(MaxAttackRadius * Math.Cos(angle) * direction),
                center.Y + (int)(MaxAttackRadius * Math.Sin(angle) * -direction));
        }

        public override void PaintingOperation(Graphics graphic)
        {
            graphic.DrawImage(this.is_attacking ? this.attack_cursor_image : this.look_cursor_image, 
                new Rectangle(new(this.cursor_position.X + 15, this.cursor_position.Y + 15), new(30, 30)));

            if (this.is_attacking == true)
            {
                graphic.DrawEllipse(Pens.White, new Rectangle(new(center.X - (int)(MaxAttackRadius),
                    center.Y - (int)(MaxAttackRadius)), new((int)(MaxAttackRadius * 2), (int)(MaxAttackRadius * 2))));

                graphic.DrawLine(Pens.White, center, new(center.X + (int)(MaxAttackRadius * Math.Cos(angle + 30 * Math.PI / 180) 
                    * direction), center.Y + (int)(MaxAttackRadius * Math.Sin(angle + 30 * Math.PI / 180) * -direction)));

                graphic.DrawLine(Pens.White, center, new(center.X + (int)(MaxAttackRadius * Math.Cos(angle - 30 * Math.PI / 180) 
                    * direction), center.Y + (int)(MaxAttackRadius * Math.Sin(angle - 30 * Math.PI / 180) * -direction)));
            }
        }
    }
}
