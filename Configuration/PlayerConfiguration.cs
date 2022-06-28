using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Configuration
{
    internal static class PlayerConfiguration
    {
        public static void AddPlayer(this Engine.EngineSceneBuilder scene_builder, bool debug, int life, double speed)
        {
            scene_builder
                .RegisterSceneConfiguration<bool>("player_damage", "BorderDraw", debug)
                .RegisterSceneConfiguration<bool>("player_collision", "BorderDraw", debug)
                .RegisterSceneConfiguration<bool>("player_hit", "BorderDraw", debug)

                .RegisterSceneConfiguration<double>("player", "MovementSpeed", speed)
                .RegisterSceneConfiguration<Int32>("player", "LifeCount", life)
                .RegisterSceneConfiguration<Size>("player", "ObjectGeometry", new Size(50, 50))
                .RegisterSceneConfiguration<Point>("player", "ObjectPosition", new Point(512, 380))
                .RegisterSceneConfiguration<Size>("player_collision", "ObjectGeometry", new Size(40, 50))
                .RegisterSceneConfiguration<Point>("player_collision", "ObjectPosition", new Point(0, 5))
                .RegisterSceneConfiguration<double>("player_attack", "MaxAttackRadius", 80)
                .RegisterSceneConfiguration<List<Type>>("player_collision", "TypesForCollide", new() 
                { 
                    typeof(Objects.Enemy), typeof(Objects.LevelWallContainer) 
                })
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
        }
    }
}
