using PracticeWork.Objects;
using PracticeWork.Engine;
using PracticeWork.Configuration;

namespace PracticeWork
{
    public partial class MainScene : Form
    {
        public System.Int32 EnemyCount { get; set; } = 5;
        public System.Int32 PlayerLife { get; set; } = 3;
        public System.Int32 ChestSpawn { get; set; } = 30;
        public System.Double MaxEnemySpeed { get; set; } = 4.0;
        public System.Double PlayerSpeed { get; set; } = 4.0;
        public System.Boolean DebugMode { get; set; } = false;

        public MainScene()
        {
            InitializeComponent();
            InitializeMainPanel(new Point(4, 4), 1024, 768);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.main_panel!.BackgroundImage = Image.FromFile(@"..\..\..\Assets\Scene\background2.png");
            this.Load += SceneOnLoadAction;
        }

        private void SceneOnLoadAction(object? sender, EventArgs e)
        {
            Engine.EngineSceneBuilder scene_builder = new("Scene", this.main_panel!);
            scene_builder.AddChestSupport(ChestSpawn, DebugMode);
            scene_builder.AddPlayer(DebugMode, PlayerLife, PlayerSpeed);

            List<string> eneme_manager_children = new();
            scene_builder.RegisterSceneConfiguration<int>("enemy_manager", "EnemyCount", EnemyCount);

            for (int i = 0; i < EnemyCount; i++)
            {
                scene_builder.AddEnemy(DebugMode, i, MaxEnemySpeed);
                eneme_manager_children.Add("enemy" + i);
            }

            scene_builder.RegisterSceneChild<Objects.EnemyManager>("enemy_manager", eneme_manager_children);
            scene_builder.AddSceneBorder(@"..\..\..\Assets\Scene\border2.png");
            scene_builder.AddUserInterface(16);

            scene_builder.BuildScene().RunSceneHandler(15, () => { MessageBox.Show("You Lose"); this.Close(); });
        }
    }
}