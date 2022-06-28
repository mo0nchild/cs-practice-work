using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Win = System.Windows;

namespace PracticeWork.Objects
{
    public sealed class Enemy : Engine.EngineObject
    {
        public const int LifeValue = 3;
        public const double SpeedUpValue = 0.5;

        private int current_life_value = Enemy.LifeValue;

        public enum LookDirection : System.Int16 { Right = 1, Left = -1 };
        public System.Boolean IsAlive { get; set; } = false;
        public System.Boolean IsReadyToRefresh { get; set; } = false;

        private Engine.EngineAnimator? enemy_animator = null;
        private Engine.EngineInputController? player_instance = null;
        private Objects.EnemyHitRegistrator? hit_registrator = default;

        private System.Drawing.Point target_position = new();
        private System.Double target_update_speed = 1.0;
        private Enemy.LookDirection look_direction = default;
        private System.Double movement_speed = 2.0;

        [Engine.EngineObjectImportConfiguration("TargetUpdateSpeed")]
        public System.Double TargetUpdateSpeed
        { 
            private set { if (value <= 1.0 && value > 0) this.target_update_speed = value; }
            get { return this.target_update_speed; } 
        }

        [Engine.EngineObjectImportConfiguration("MovementSpeed")]
        public System.Double MaxMovementSpeed { get; private set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public Enemy(string object_name) : base(object_name) => this.look_direction = LookDirection.Right;

        public void Refresh() 
        {
            this.IsReadyToRefresh = !(this.IsAlive = true);
            this.current_life_value = Enemy.LifeValue;

            if (this.movement_speed + Enemy.SpeedUpValue <= this.MaxMovementSpeed) movement_speed += SpeedUpValue;
        }

        public void EnemyHitPlayer() 
        {
            if(this.IsAlive) this.enemy_animator?.PlayAnimation("attack_animation", false);
        }

        public void DamageRegistration()
        {
            if(--this.current_life_value <= 0)
            {
                this.enemy_animator?.PlayAnimation("death_animation", false);
            }
            else this.enemy_animator?.PlayAnimation("damage_animation", false);
        }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            try {
                this.player_instance = (EngineInputController?)(this.LinkedScene?.GetSceneObject("player")); ;
                this.enemy_animator = (EngineAnimator?)(this.GetChildrenObjects<EngineAnimator>()[0]);

                this.hit_registrator = (EnemyHitRegistrator)(this.GetChildrenObjects<Objects.DamageHolder>()[1]
                    .GetChildrenObjects<EnemyHitRegistrator>()[0]);

                this.hit_registrator.HitRegistration();
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

        public override void UpdateOperation(IEngineScene scene_instance)
        {
            if (this.player_instance == null) return;

            if (this.enemy_animator?.AnimationName != "attack_animation" && this.hit_registrator?.HitInstalled != true)
            {
                this.hit_registrator.HitRegistration();
            }

            //if (this.enemy_animator?.AnimationName != "damage_animation" && this.enemy_animator?.AnimationName != null &&
            //    this.enemy_animator?.AnimationName != "run_animation" && this.enemy_animator?.AnimationName != "death_animation"
            //    && this.enemy_animator.AnimationName != "attack_animation" && IsAlive)
            //    this.enemy_animator?.PlayAnimation("run_animation");
            if (this.enemy_animator?.AnimationName == "" && IsAlive) this.enemy_animator?.PlayAnimation("run_animation");

            //if (this.current_life_value <= 0 && this.IsAlive)
            //{
            //    this.enemy_animator?.PlayAnimation("death_animation", false);
            //    this.IsAlive = false;
            //}

            if (this.current_life_value <= 0 && this.enemy_animator.AnimationName != "death_animation") this.IsAlive = false;

            if(this.IsAlive == false && this.enemy_animator.AnimationName != "death_animation") this.IsReadyToRefresh = true;

            int target_x = this.target_position.X, target_y = this.target_position.Y;
            double angle = Math.Atan((double)(target_y - Position.Y) / (target_x - Position.X));

            if ((target_x - Position.X < 0 && this.look_direction == LookDirection.Right) ||
                (target_x - Position.X > 0 && this.look_direction == LookDirection.Left))
            {
                this.enemy_animator?.FlipAllAnimationFrame(ImageExtension.FlipImageDirection.FlipX);
                this.look_direction = (LookDirection)(-(int)look_direction);
            }

            int direction_x = (int)(movement_speed * Math.Cos(angle)) * (int)this.look_direction,
                direction_y = (int)(movement_speed * Math.Sin(angle)) * (int)this.look_direction;

            if(this.current_life_value > 0) this.SetPosition(new Point(direction_x, -direction_y));
        }

        public override void PaintingOperation(Graphics graphic) { return; }
    }

    public sealed class EnemyManager : Engine.EngineObject
    {
        public class EnemyConfiguration
        { 
            public Objects.Enemy EnemyInstance { get; set; }
            public System.Drawing.Point EnemyStartPosition { get; set; }
        }

        private List<EnemyManager.EnemyConfiguration> enemies = new();
        private int current_level = 1;

        [Engine.EngineObjectImportConfiguration("EnemyCount")]
        public int EnemyCount { get; private set; } = 0;

        [Engine.EngineObjectConstructorSelecter]
        public EnemyManager(string object_name) : base(object_name) { }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            if (EnemyCount > 10) throw new Exception("So many enemies :)");
            for (int i = 0; i < this.EnemyCount; i++)
            {
                var selected_enemy = (Objects.Enemy?)this.GetChildrenObject("enemy" + i);
                if (selected_enemy != null)
                {
                    this.enemies?.Add(new() { EnemyInstance = selected_enemy, EnemyStartPosition = new() });
                }
            }

            bool trigger_break = false;

            for(int row = 0; row < 5; row++)
            {
                for(int side = 0; side <= 1; side++)
                {
                    if (row * 2 + side >= this.enemies?.Count) { trigger_break = true; break; }

                    (this.enemies?[row * 2 + side])!.EnemyStartPosition = new(1024 * side, 150 * row);
                    (this.enemies?[row * 2 + side])!.EnemyInstance.SetPosition(1024 * side, 150 * row);
                }
                if (trigger_break) break;
            }
        }

        public override void UpdateOperation(IEngineScene scene_instance)
        {
            foreach (var item in this.enemies)
            {
                Objects.Enemy enemy = item.EnemyInstance;
                if (!enemy.IsAlive)
                {
                    enemy.SetPosition(item.EnemyStartPosition.X, item.EnemyStartPosition.Y);
                    enemy.Refresh();
                }
            }
        }

        public override void PaintingOperation(Graphics graphic) { return; }
    }

    public sealed class LevelWallContainer : Engine.EngineObject
    {
        [Engine.EngineObjectConstructorSelecter]
        public LevelWallContainer(string object_name) : base(object_name) { }

        public override void PaintingOperation(Graphics graphic) { return; }
    }
}


