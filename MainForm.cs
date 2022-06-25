namespace PracticeWork
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeMainPanel(new Point(4, 4), 1024, 768);

            this.main_panel!.BackgroundImage = Image.FromFile(@"..\..\..\Assets\Scene\background.png");
            Engine.EngineSceneBuilder scene_builder = new("Scene", this.main_panel!);

            scene_builder
                .RegisterSceneConfiguration<double>("enemy", "MovementSpeed", 2.5)
                .RegisterSceneConfiguration<Size>("enemy", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<double>("enemy", "TargetUpdateSpeed", 0.1)
                .RegisterSceneConfiguration<Size>("enemy_collision", "ObjectGeometry", new Size(50, 40))
                .RegisterSceneConfiguration<Point>("enemy_collision", "ObjectPosition", new Point(0, 10))

                .RegisterSceneConfiguration<double>("enemy1", "MovementSpeed", 2.5)
                .RegisterSceneConfiguration<Size>("enemy1", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<double>("enemy1", "TargetUpdateSpeed", 0.1)
                .RegisterSceneConfiguration<Size>("enemy_collision1", "ObjectGeometry", new Size(50, 40))
                .RegisterSceneConfiguration<Point>("enemy_collision1", "ObjectPosition", new Point(0, 10))

                .RegisterSceneConfiguration<double>("enemy2", "MovementSpeed", 2.5)
                .RegisterSceneConfiguration<Size>("enemy2", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<double>("enemy2", "TargetUpdateSpeed", 0.1)
                .RegisterSceneConfiguration<Size>("enemy_collision2", "ObjectGeometry", new Size(50, 40))
                .RegisterSceneConfiguration<Point>("enemy_collision2", "ObjectPosition", new Point(0, 10))

                .RegisterSceneConfiguration<double>("player", "MovementSpeed", 3.5)
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
                .RegisterSceneConfiguration<double>("player_animator", "AnimationSpeed", 0.85)



                .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator1", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                    { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                    { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                })
                .RegisterSceneConfiguration<double>("enemy_animator1", "AnimationSpeed", 0.75)



                .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator2", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                    { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                    { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                })
                .RegisterSceneConfiguration<double>("enemy_animator2", "AnimationSpeed", 0.75)


                .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator", "AnimationsContainer", new()
                {
                    { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                    { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                    { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                })
                .RegisterSceneConfiguration<double>("enemy_animator", "AnimationSpeed", 0.75);

            scene_builder
                .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision", new())
                .RegisterSceneChild<Objects.Enemy>("enemy", new() { "enemy_animator", "enemy_collision" })
                .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator1", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision1", new())
                .RegisterSceneChild<Objects.Enemy>("enemy1", new() { "enemy_animator1", "enemy_collision1" })
                .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator2", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision2", new())
                .RegisterSceneChild<Objects.Enemy>("enemy2", new() { "enemy_animator2", "enemy_collision2" })
                .RegisterSceneChild<Engine.EngineAnimator>("player_animator", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("player_collision", new())
                .RegisterSceneChild<Objects.Player>("player", new() { "player_animator", "player_collision" });

            var scene = scene_builder.BuildScene();

            scene.GetSceneObject("enemy").SetPosition(new(100, -100));
            scene.GetSceneObject("enemy1").SetPosition(new(100, -400));
            scene.GetSceneObject("enemy2").SetPosition(new(100, -80));

            scene.RunSceneHandler(15);
        }


    }
}