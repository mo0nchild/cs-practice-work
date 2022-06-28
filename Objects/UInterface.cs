using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    internal sealed class UInterface : Engine.EngineObject
    {
        [Engine.EngineObjectImportConfiguration("LifeCountOffset")]
        public int LifeCountOffset { get; private set; } = 900;

        [Engine.EngineObjectImportConfiguration("EnemyEliminatedOffset")]
        public int EnemyEliminatedOffset { get; private set; } = 50;

        [Engine.EngineObjectImportConfiguration("UIFontSize")]
        public int UIFontSize { get;  private set; } = 16;

        private Objects.EnemyManager? enemy_manager = null;
        private Objects.Player? player_instance = null;
        private System.Int32 session_time = default;

        [Engine.EngineObjectConstructorSelecter]
        public UInterface(string object_name) : base(object_name) { }

        public override void InitialOperation(IEngineScene scene_instance)
        {
            this.enemy_manager = (EnemyManager?)scene_instance.GetSceneObject("enemy_manager");
            this.player_instance = (Player?)scene_instance.GetSceneObject("player");

            System.Windows.Forms.Timer timer = new() { Interval = 1000 };
            timer.Tick += (object? sender, EventArgs e) => this.session_time++;
            timer.Start();
        }

        public override void PaintingOperation(Graphics graphic)
        {
            graphic.DrawString("Enemies eliminated: " + this.enemy_manager?.EnemyEliminated.ToString(), 
                new Font("Arial", this.UIFontSize), Brushes.White, this.EnemyEliminatedOffset, 10f);

            graphic.DrawString("Time: " + this.session_time.ToString(), new Font("Arial", this.UIFontSize), 
                Brushes.White, this.LinkedScene!.SceneSize.Width / 2, 10f);

            graphic.DrawString("Life: " + (this.player_instance?.LifeCount).ToString(), 
                new Font("Arial", this.UIFontSize), Brushes.White, this.LifeCountOffset, 10f);
        }
    }
}
