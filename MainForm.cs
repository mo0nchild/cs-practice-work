using PracticeWork.Objects;
using PracticeWork.Engine;
using PracticeWork.Configuration;

namespace PracticeWork
{
    public partial class MainForm : Form
    {
        public const int enemy_count = 5, player_life = 5;
        public const double max_enemy_speed = 4.0, player_speed = 4.0;
        public const bool debug_mode = true;

        public MainForm()
        {
            InitializeComponent();
            InitializeMainPanel(new Point(4, 4), 1024, 768);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.main_panel!.BackgroundImage = Image.FromFile(@"..\..\..\Assets\Scene\background2.png");
            Engine.EngineSceneBuilder scene_builder = new("Scene", this.main_panel!);

            scene_builder

                .RegisterSceneConfiguration<bool>("player_damage", "BorderDraw", debug_mode)
                .RegisterSceneConfiguration<bool>("player_collision", "BorderDraw", debug_mode)
                .RegisterSceneConfiguration<bool>("player_hit", "BorderDraw", debug_mode)

                .RegisterSceneConfiguration<double>("player", "MovementSpeed", player_speed)
                .RegisterSceneConfiguration<Int32>("player", "LifeCount", player_life)
                .RegisterSceneConfiguration<Size>("player", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<Point>("player", "ObjectPosition", new Point(512, 380))
                .RegisterSceneConfiguration<Size>("player_collision", "ObjectGeometry", new Size(40, 50))
                .RegisterSceneConfiguration<Point>("player_collision", "ObjectPosition", new Point(0, 5))
                .RegisterSceneConfiguration<double>("player_attack", "MaxAttackRadius", 80)
                .RegisterSceneConfiguration<List<Type>>("player_collision", "TypesForCollide", new() { typeof(Objects.Enemy), typeof(Objects.LevelWallContainer) })
                .RegisterSceneConfiguration<List<Type>>("player_hit", "TypesForCollide", new() { typeof(Objects.DamageHolder) })
                .RegisterSceneConfiguration<Point>("player_damage", "ObjectPosition", new Point(-10, 0))
                .RegisterSceneConfiguration<Size>("player_damage", "ObjectGeometry", new Size(60, 60))
                .RegisterSceneConfiguration<Dictionary<string, string>>("player_animator", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Player\Movement" },
                    { "attack_animation1", @"..\..\..\Assets\Player\Attack1" },
                    { "attack_animation2", @"..\..\..\Assets\Player\Attack2" },
                    { "attack_animation3", @"..\..\..\Assets\Player\Attack3" },
                    { "idle_animation", @"..\..\..\Assets\Player\Idle" },
                    { "damage_animation", @"..\..\..\Assets\Player\Damage" },
                    { "death_animation", @"..\..\..\Assets\Player\Death" },
                })
                .RegisterSceneConfiguration<double>("player_animator", "AnimationSpeed", 0.9);

            scene_builder
                .RegisterSceneChild<Engine.EngineAnimator>("player_animator", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("player_collision", new())

                .RegisterSceneChild<Objects.PlayerHitRegistrator>("player_hit", new())
                .RegisterSceneChild<Objects.PlayerAttackCursor>("player_attack", new() { "player_hit" })

                .RegisterSceneChild<Objects.PlayerDamageRegistrator>("player_damage", new())
                .RegisterSceneChild<Objects.DamageHolder>("damage_holder_player", new() { "player_damage" })

                .RegisterSceneChild<Objects.Player>("player", new() { "player_animator", "player_collision", 
                    "player_attack", "damage_holder_player" });

            List<string> eneme_manager_children = new();
            scene_builder.RegisterSceneConfiguration<int>("enemy_manager", "EnemyCount", enemy_count);

            for (int i = 0; i < enemy_count; i++) 
            {
                scene_builder
                    .RegisterSceneConfiguration<double>("enemy" + i, "MovementSpeed", max_enemy_speed)
                    .RegisterSceneConfiguration<Size>("enemy" + i, "ObjectGeometry", new Size(50, 50))
                    .RegisterSceneConfiguration<double>("enemy" + i, "TargetUpdateSpeed", 0.1)
                    .RegisterSceneConfiguration<Size>("enemy_collision" + i, "ObjectGeometry", new Size(50, 40))
                    .RegisterSceneConfiguration<Point>("enemy_collision" + i, "ObjectPosition", new Point(0, 10))
                    .RegisterSceneConfiguration<bool>("enemy_collision" + i, "BorderDraw", debug_mode)
                    .RegisterSceneConfiguration<List<Type>>("enemy_collision" + i, "TypesForCollide", new() { typeof(Player), typeof(Enemy) })
                    .RegisterSceneConfiguration<List<Type>>("enemy_hit" + i, "TypesForCollide", new() { typeof(Objects.DamageHolder) })
                    .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator" + i, "AnimationsContainer", new()
                    {
                        { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                        { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                        { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                        { "damage_animation", @"..\..\..\Assets\Enemy\Damage" },
                        { "death_animation", @"..\..\..\Assets\Enemy\Death" },
                    })
                    .RegisterSceneConfiguration<double>("enemy_animator" + i, "AnimationSpeed", 0.75);

                scene_builder
                    .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator" + i, new())
                    .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision" + i, new())

                    .RegisterSceneChild<Objects.EnemyDamageRegistrator>("enemy_damage" + i, new())
                    .RegisterSceneChild<Objects.DamageHolder>("damage_holder" + i, new() { "enemy_damage" + i })

                    .RegisterSceneChild<Objects.EnemyHitRegistrator>("enemy_hit" + i, new())
                    .RegisterSceneChild<Objects.DamageHolder>("hit_holder" + i, new() { "enemy_hit" + i })

                    .RegisterSceneChild<Objects.Enemy>("enemy" + i, new() { "enemy_animator" + i, "enemy_collision" + i, 
                        "damage_holder" + i, "hit_holder" + i});

                eneme_manager_children.Add("enemy" + i);
            }

            scene_builder.RegisterSceneChild<Objects.EnemyManager>("enemy_manager", eneme_manager_children);
            scene_builder.AddSceneBorder(@"..\..\..\Assets\Scene\border2.png");
            scene_builder.AddUserInterface(16);

            scene_builder.BuildScene().RunSceneHandler(15, () => { this.Close(); });
        }


    }
}