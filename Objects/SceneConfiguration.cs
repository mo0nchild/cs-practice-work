using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    internal static class SceneConfiguration
    {
        public static void AddSceneBorder(this Engine.EngineSceneBuilder scene_builder, string sprite_path)
        {
            scene_builder
                .RegisterSceneConfiguration<string>("border_sprite", "SpriteImagePath", sprite_path)
                .RegisterSceneConfiguration<Size>("border_sprite", "ObjectGeometry", new Size(1024, 768))
                .RegisterSceneConfiguration<Size>("border_wall1", "ObjectGeometry", new Size(96, 1024))
                .RegisterSceneConfiguration<Size>("border_wall2", "ObjectGeometry", new Size(1024, 96))
                .RegisterSceneConfiguration<Size>("border_wall3", "ObjectGeometry", new Size(96, 1024))
                .RegisterSceneConfiguration<Point>("border_wall3", "ObjectPosition", new Point(928, 0))
                .RegisterSceneConfiguration<Size>("border_wall4", "ObjectGeometry", new Size(1024, 96))
                .RegisterSceneConfiguration<Point>("border_wall4", "ObjectPosition", new Point(0, 672));

            scene_builder
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("border_wall1", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("border_wall2", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("border_wall3", new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("border_wall4", new())
                .RegisterSceneChild<Engine.EngineSprite>("border_sprite", new())
                .RegisterSceneChild<Objects.LevelWallContainer>("border", new() { 
                    "border_sprite", "border_wall1", "border_wall2", "border_wall3", "border_wall4"
                });
        }
    }
}
