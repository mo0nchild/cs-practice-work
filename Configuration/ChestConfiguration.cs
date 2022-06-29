using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Configuration
{
    internal static class ChestConfiguration
    {
        public static void AddChestSupport(this Engine.EngineSceneBuilder scene_builder, int spawn_time, bool debug)
        {
            scene_builder
                .RegisterSceneConfiguration<Point>("chest", "SpawnMinPosition", new Point(192, 192))
                .RegisterSceneConfiguration<Point>("chest", "SpawnMaxPosition", new Point(832, 576))
                .RegisterSceneConfiguration<Size>("chest", "ObjectGeometry", new Size(28, 28))
                .RegisterSceneConfiguration<int>("chest", "ChestSpawnTime", spawn_time)
                .RegisterSceneConfiguration<List<Type>>("chest_trigger", "TypesForCollide", new() { typeof(Objects.Player) })
                .RegisterSceneConfiguration<Dictionary<string, string>>("chest_animator", "AnimationsContainer", new()
                {
                    { "open_animation", @"..\..\..\Assets\Chest\Open" },
                    { "idle_animation", @"..\..\..\Assets\Chest\Idle" },
                })
                .RegisterSceneConfiguration<double>("chest_animator", "AnimationSpeed", 0.5)
                .RegisterSceneConfiguration<bool>("chest_trigger", "BorderDraw", debug);

            scene_builder
                .RegisterSceneChild<Engine.EngineAnimator>("chest_animator", new())
                .RegisterSceneChild<Objects.ChestTrigger>("chest_trigger", new())
                .RegisterSceneChild<Objects.ChestSupport>("chest", new() { "chest_animator", "chest_trigger" });
        }
    }
}
