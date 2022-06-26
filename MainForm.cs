namespace PracticeWork
{
    public partial class MainForm : Form
    {
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

                .RegisterSceneConfiguration<string>("border", "SpriteImagePath", @"..\..\..\Assets\Scene\border2.png")
                .RegisterSceneConfiguration<Size>("border", "ObjectGeometry", new Size(1024, 768))
                .RegisterSceneConfiguration<Point>("border", "ObjectPosition", new Point(0, 0))

                .RegisterSceneConfiguration<double>("enemy", "MovementSpeed", 2.5)
                .RegisterSceneConfiguration<Size>("enemy", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<Point>("enemy", "ObjectPosition", new Point(0, 100))
                .RegisterSceneConfiguration<double>("enemy", "TargetUpdateSpeed", 0.1)
                .RegisterSceneConfiguration<Size>("enemy_collision", "ObjectGeometry", new Size(50, 40))
                .RegisterSceneConfiguration<Point>("enemy_collision", "ObjectPosition", new Point(0, 10))
                .RegisterSceneConfiguration<List<Type>>("enemy_collision", "TypesForCollide", new() { typeof(Objects.Player), typeof(Objects.Enemy) })
                .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                    { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                    { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                    { "damage_animation", @"..\..\..\Assets\Enemy\Damage" },
                    { "death_animation", @"..\..\..\Assets\Enemy\Death" },
                })
                .RegisterSceneConfiguration<double>("enemy_animator", "AnimationSpeed", 0.75)

                    
                .RegisterSceneConfiguration<double>("player_attack", "MaxAttackRadius", 80)

                .RegisterSceneConfiguration<bool>("player_hit", "BorderDraw", true)
                .RegisterSceneConfiguration<bool>("enemy_damage", "BorderDraw", true)


                .RegisterSceneConfiguration<double>("player", "MovementSpeed", 4)
                .RegisterSceneConfiguration<Point>("player", "ObjectPosition", new Point(400, 300))
                .RegisterSceneConfiguration<Size>("player", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<Size>("player_collision", "ObjectGeometry", new Size(50, 40))
                .RegisterSceneConfiguration<Point>("player_collision", "ObjectPosition", new Point(0, 10))
                .RegisterSceneConfiguration<Dictionary<string, string>>("player_animator", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Player\Movement" },
                    { "attack_animation1", @"..\..\..\Assets\Player\Attack1" },
                    { "attack_animation2", @"..\..\..\Assets\Player\Attack2" },
                    { "attack_animation3", @"..\..\..\Assets\Player\Attack3" },
                    { "idle_animation", @"..\..\..\Assets\Player\Idle" },
                })
                .RegisterSceneConfiguration<double>("player_animator", "AnimationSpeed", 0.9);

            scene_builder

                .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision", new())
                .RegisterSceneChild<Objects.EnemyDamageRegistrator>("enemy_damage", new())
                .RegisterSceneChild<Objects.DamageHolder>("damage_holder", new() { "enemy_damage" })
                .RegisterSceneChild<Objects.Enemy>("enemy", new() { "enemy_animator", "enemy_collision", "damage_holder" })


                .RegisterSceneChild<Engine.EngineSprite>("border", new())


                .RegisterSceneChild<Engine.EngineAnimator>("player_animator", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("player_collision", new())
                .RegisterSceneChild<Objects.PlayerHitRegistrator>("player_hit", new())
                .RegisterSceneChild<Objects.PlayerAttackCursor>("player_attack", new() { "player_hit" })
                .RegisterSceneChild<Objects.Player>("player", new() { "player_animator", "player_collision", "player_attack" });

            var scene = scene_builder.BuildScene();

            scene.GetSceneObject("enemy").SetPosition(new(100, -100));

            scene.RunSceneHandler(15);
        }


    }
}