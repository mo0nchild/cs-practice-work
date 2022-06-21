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
                .RegisterSceneConfiguration<Size>("main_sprite", "ObjectGeometry", new Size(35, 50));

            scene_builder
                .RegisterSceneChild<Engine.EngineSprite>("main_sprite", new())
                .RegisterSceneChild<Objects.MainPlayer>("object1", new() { "main_sprite" });

            var scene = scene_builder.BuildScene();
            scene.RunSceneHandler(15);
        }


    }
}