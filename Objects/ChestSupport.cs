using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    internal sealed class ChestSupport : Engine.EngineObject
    {
        private Objects.ChestTrigger? chest_trigger = default;
        private Engine.EngineAnimator? chest_animator = default;
        private Objects.Player? player_instance = default;
        
        private System.Int32 timer_value = default;
        private System.Boolean chest_is_open = default;

        [Engine.EngineObjectImportConfiguration("ChestSpawnTime")]
        public System.Int32 ChestSpawnTime { get; private set; } = 5;

        [Engine.EngineObjectImportConfiguration("HealingValue")]
        public System.Int32 HealingValue { get; private set; } = 3;

        [Engine.EngineObjectImportConfiguration("SpawnMinPosition")]
        public System.Drawing.Point SpawnMinPosition { get; private set; } = new();

        [Engine.EngineObjectImportConfiguration("SpawnMaxPosition")]
        public System.Drawing.Point SpawnMaxPosition { get; private set; } = new();

        [Engine.EngineObjectConstructorSelecter]
        public ChestSupport(string object_name) : base(object_name) { }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            System.Windows.Forms.Timer chest_timer = new() { Interval = 1000 };
            chest_timer.Tick += delegate (object? sender, EventArgs e)  
            { 
                if (++timer_value >= ChestSpawnTime) { ChestSpawn(); timer_value = 0; } 
            };
            chest_timer.Start();

            this.chest_trigger = (ChestTrigger?)(this.GetChildrenObjects<ChestTrigger>()[0]);
            this.chest_animator = (EngineAnimator?)(this.GetChildrenObjects<EngineAnimator>()[0]);
            this.player_instance = (Player?)(this.LinkedScene?.GetSceneObject("player"));
        }

        public void ChestSpawn() 
        {
            if (!this.chest_is_open)
            {
                var random_position = new Random();
                this.SetPosition(random_position.Next(this.SpawnMinPosition.X, this.SpawnMaxPosition.X),
                    random_position.Next(this.SpawnMinPosition.Y, this.SpawnMaxPosition.Y));

                this.chest_trigger!.IsReady = true;
            }
        }

        public void ChestOpen()
        {
            this.player_instance?.HealingAction(this.HealingValue);
            this.chest_is_open = true;
            this.chest_animator?.PlayAnimation("open_animation", false);
        }

        public override void UpdateOperation(IEngineScene scene_instance)
        {
            if (this.chest_animator!.AnimationName != "open_animation" && this.chest_is_open == true)
            {
                this.chest_animator?.PlayAnimation("idle_animation");
                this.SetPosition(0, 0);
                this.chest_is_open = false;
            }
        }

        public override void PaintingOperation(Graphics graphic) { return; }
    }

    internal sealed class ChestTrigger : Engine.EngineBoxTrigerCollision
    {
        private Objects.ChestSupport? chest_instance = default;
        public System.Boolean IsReady { get; set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public ChestTrigger(string object_name) : base(object_name) { }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            base.InitialOperation(scene_instance);
            this.chest_instance = this.ParentObject as Objects.ChestSupport;
        }

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {
            if (this.IsReady)
            {
                this.chest_instance?.ChestOpen();
                this.IsReady = false;
            }
        }
    }
}
