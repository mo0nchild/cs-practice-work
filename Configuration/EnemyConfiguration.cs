using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticeWork.Objects;

namespace PracticeWork.Configuration
{

    internal static class EnemyConfiguration
    {
        public static void AddEnemy(this Engine.EngineSceneBuilder scene_builder, bool debug, int number, double speed)
        {
            scene_builder
                    .RegisterSceneConfiguration<double>("enemy" + number, "MovementSpeed", speed)
                    .RegisterSceneConfiguration<Size>("enemy" + number, "ObjectGeometry", new Size(50, 50))
                    .RegisterSceneConfiguration<double>("enemy" + number, "TargetUpdateSpeed", 0.1)
                    .RegisterSceneConfiguration<Size>("enemy_collision" + number, "ObjectGeometry", new Size(50, 40))
                    .RegisterSceneConfiguration<Point>("enemy_collision" + number, "ObjectPosition", new Point(0, 10))
                    .RegisterSceneConfiguration<bool>("enemy_collision" + number, "BorderDraw", debug)
                    .RegisterSceneConfiguration<List<Type>>("enemy_collision" + number, "TypesForCollide", new() { typeof(Player), typeof(Enemy) })
                    .RegisterSceneConfiguration<List<Type>>("enemy_hit" + number, "TypesForCollide", new() { typeof(Objects.DamageHolder) })
                    .RegisterSceneConfiguration<Dictionary<string, string>>("enemy_animator" + number, "AnimationsContainer", new()
                    {
                        { "run_animation", @"..\..\..\Assets\Enemy\Movement" },
                        { "attack_animation", @"..\..\..\Assets\Enemy\Attack" },
                        { "idle_animation", @"..\..\..\Assets\Enemy\Idle" },
                        { "damage_animation", @"..\..\..\Assets\Enemy\Damage" },
                        { "death_animation", @"..\..\..\Assets\Enemy\Death" },
                    })
                    .RegisterSceneConfiguration<double>("enemy_animator" + number, "AnimationSpeed", 0.75);

            scene_builder
                .RegisterSceneChild<Engine.EngineAnimator>("enemy_animator" + number, new())
                .RegisterSceneChild<Engine.EngineBoxStaticCollision>("enemy_collision" + number, new())

                .RegisterSceneChild<Objects.EnemyDamageRegistrator>("enemy_damage" + number, new())
                .RegisterSceneChild<Objects.DamageHolder>("damage_holder" + number, new() { "enemy_damage" + number })

                .RegisterSceneChild<Objects.EnemyHitRegistrator>("enemy_hit" + number, new())
                .RegisterSceneChild<Objects.DamageHolder>("hit_holder" + number, new() { "enemy_hit" + number })

                .RegisterSceneChild<Objects.Enemy>("enemy" + number, new() { "enemy_animator" + number, "enemy_collision" + number,
                        "damage_holder" + number, "hit_holder" + number});
        }
    }
}
