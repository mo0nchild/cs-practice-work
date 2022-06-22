namespace PracticeWork
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeMainPanel(new Point(4, 4), 793, 434);

            Engine.EngineSceneBuilder scene_builder = new("Scene", this.main_panel!);

            scene_builder
                .RegisterSceneConfiguration<string>("main_sprite", "SpriteImagePath", @"C:\Users\Byter\Documents\Projects\C#Lang\Student\cs-practice-work\Assets\0001.png")
                .RegisterSceneConfiguration<Size>("object1", "ObjectGeometry", new Size(35, 50))
                .RegisterSceneConfiguration<Size>("test_colission", "ObjectGeometry", new(35, 50))
                .RegisterSceneConfiguration<string>("test_sprite", "SpriteImagePath", @"C:\Users\Byter\Documents\Projects\C#Lang\Student\cs-practice-work\Assets\0001.png");

            scene_builder
                .RegisterSceneChild<Engine.EngineSprite>("main_sprite", new())
                .RegisterSceneChild<Engine.EngineSprite>("test_sprite", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("main_colission", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("test_colission", new() { "test_sprite" })
                .RegisterSceneChild<Objects.MainPlayer>("object1", new() { "main_sprite", "main_colission" });

            var scene = scene_builder.BuildScene();
            scene.GetSceneObject("test_colission")?.SetPosition(new(200, -100));


            scene.RunSceneHandler(15);
        }


    }
}