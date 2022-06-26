using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Win = System.Windows;

namespace PracticeWork.Objects
{
    internal sealed class Enemy : Engine.EngineObject
    {
        public const int LifeValue = 3;
        private int current_life_value = Enemy.LifeValue;

        public enum LookDirection : System.Int16 { Right = 1, Left = -1 };

        private Engine.EngineAnimator? enemy_animator = null;
        private Engine.EngineInputController? player_instance = null;

        private System.Drawing.Point target_position = new();
        private System.Double target_update_speed = 1.0;
        private Enemy.LookDirection look_direction = default;

        [Engine.EngineObjectImportConfiguration("TargetUpdateSpeed")]
        public System.Double TargetUpdateSpeed
        { 
            private set { if (value <= 1.0 && value > 0) this.target_update_speed = value; }
            get { return this.target_update_speed; } 
        }

        [Engine.EngineObjectImportConfiguration("MovementSpeed")]
        public System.Double MovementSpeed { get; private set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public Enemy(string object_name) : base(object_name) => this.look_direction = LookDirection.Right;

        public void DamageRegistration()
        {
            if(--this.current_life_value == 0)
            {
                Console.WriteLine("Killed");
            }
            this.enemy_animator?.PlayAnimation("damage_animation", false);
        }

        public override void InitialOperation(EngineScene scene_instance)
        {
            try {
                this.player_instance = (EngineInputController?)(this.LinkedScene?.GetSceneObject("player")); ;
                this.enemy_animator = (EngineAnimator?)(this.GetChildrenObjects<EngineAnimator>()[0]);
            }
            catch(System.Exception error) { Console.WriteLine(error.Message); }

            this.enemy_animator?.PlayAnimation("run_animation");
            Win::Forms.Timer enemy_handling_timer = new() { Interval = (int)(60 / target_update_speed )};
            
            enemy_handling_timer.Tick += new EventHandler(delegate (object? sender, EventArgs args)
            {
                if(this.player_instance != null) this.target_position = this.player_instance.Position;
            });
            enemy_handling_timer.Start();
        }

        public override void UpdateOperation(Graphics graphic)
        {
            if (this.player_instance == null) return;

            if (this.enemy_animator.AnimationName != "damage_animation" && this.enemy_animator?.AnimationName != null &&
                this.enemy_animator.AnimationName != "run_animation" && this.enemy_animator.AnimationName != "death_animation")
                this.enemy_animator?.PlayAnimation("run_animation");

            if(this.current_life_value < 0 && this.player_instance != null)
            {
                this.enemy_animator?.PlayAnimation("death_animation", false);
                this.player_instance = null;
            }

            int target_x = this.target_position.X, target_y = this.target_position.Y;
            double angle = Math.Atan((double)(target_y - Position.Y) / (target_x - Position.X));

            if ((target_x - Position.X < 0 && this.look_direction == LookDirection.Right) ||
                (target_x - Position.X > 0 && this.look_direction == LookDirection.Left))
            {
                this.enemy_animator?.FlipAllAnimationFrame(ImageExtension.FlipImageDirection.FlipX);
                this.look_direction = (LookDirection)(-(int)look_direction);
            }

            int direction_x = (int)(MovementSpeed * Math.Cos(angle)) * (int)this.look_direction,
                direction_y = (int)(MovementSpeed * Math.Sin(angle)) * (int)this.look_direction;

            this.SetPosition(new Point(direction_x, -direction_y));
        }
    }
}
