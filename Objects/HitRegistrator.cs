using PracticeWork.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    class DamageHolder : Engine.EngineObject
    {
        [Engine.EngineObjectConstructorSelecter]
        public DamageHolder(string object_name) : base(object_name)
        {
            this.Geometry = new(50, 50);
        }

        public override void UpdateOperation(Graphics graphic)
        {
            return;
        }
    }

    internal sealed class PlayerHitRegistrator : Engine.EngineBoxTrigerCollision
    {
        public System.Boolean HitInstalled { get; private set; } = default;

        [Engine.EngineObjectConstructorSelecter]
        public PlayerHitRegistrator(string object_name) : base(object_name) { }

        public void HitRegistration(bool value = true) => this.HitInstalled = value;

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {
            if(target_object is EnemyDamageRegistrator registrator && this.HitInstalled)
            {
                registrator.DamageRegistration();
                this.HitInstalled = false;
            }
        }
    }

    internal sealed class EnemyDamageRegistrator : Engine.EngineBoxTrigerCollision
    {
        [Engine.EngineObjectConstructorSelecter]
        public EnemyDamageRegistrator(string object_name) : base(object_name) { }

        public void DamageRegistration()
        {
            (this.ParentObject?.ParentObject as Objects.Enemy)?.DamageRegistration();
        }

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {
            return;
        }
    }

    internal sealed class EnemyHitRegistrator : Engine.EngineBoxTrigerCollision
    {
        private System.Boolean hit_installed = default;

        [Engine.EngineObjectConstructorSelecter]
        public EnemyHitRegistrator(string object_name) : base(object_name) { }

        public void HitRegistration() => this.hit_installed = true;

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {
            if (target_object is PlayerDamageRegistrator registrator && this.hit_installed)
            {
                registrator.DamageRegistration();
                this.hit_installed = false;
            }
        }
    }

    internal sealed class PlayerDamageRegistrator : Engine.EngineBoxTrigerCollision
    {
        [Engine.EngineObjectConstructorSelecter]
        public PlayerDamageRegistrator(string object_name) : base(object_name) { }

        public void DamageRegistration() => (this.ParentObject as Objects.Player)?.DamageRegistration();

        protected override void OnTriggerDetectCollision(EngineObject target_object, CollideSide side)
        {

        }
    }
}
