using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Configuration
{
    internal static class UIConfiguration
    {
        public static void AddUserInterface(this Engine.EngineSceneBuilder scene_builder, int font_size)
        {
            scene_builder
                .RegisterSceneConfiguration<string>("ui_top", "SpriteImagePath", @"..\..\..\Assets\Scene\ui-container.png")
                .RegisterSceneConfiguration<string>("ui_bottom", "SpriteImagePath", @"..\..\..\Assets\Scene\ui-container.png")
                .RegisterSceneConfiguration<Point>("ui_bottom", "ObjectPosition", new Point(0, 690))
                .RegisterSceneConfiguration<Size>("ui_top", "ObjectGeometry", new Size(1024, 40))
                .RegisterSceneConfiguration<Size>("ui_bottom", "ObjectGeometry", new Size(1024, 40))
                .RegisterSceneConfiguration<int>("interface", "UIFontSize", font_size);

            scene_builder
                .RegisterSceneChild<Engine.EngineSprite>("ui_top", new())
                .RegisterSceneChild<Engine.EngineSprite>("ui_bottom", new())
                .RegisterSceneChild<Objects.UInterface>("interface", new() { "ui_top", "ui_bottom" });
        }
    }
}
