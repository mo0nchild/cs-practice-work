using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Engine
{
    public sealed class EngineBoxCollision : Engine.EngineObject
    {
        [Engine.EngineObjectInportConfiguration("BorderDraw")]
        public System.Boolean BorderDraw { get; private set; } = false;

        [Engine.EngineObjectConstructorSelecter]
        public EngineBoxCollision(string object_name) : base(object_name)
        {

        }

        public override void UpdateOperation(Graphics graphic)
        {
            this.LinkedScene?.GetSceneObjects<EngineBoxCollision>().ForEach((target_object) =>
            {
                CollisionChecker((EngineBoxCollision)target_object);
            });
            graphic.DrawRectangle(Pens.Green, new Rectangle(this.Position, this.Geometry));
        }

        private bool CollisionChecker(EngineBoxCollision target_collision)
        {
            return true;
        }
    }
}
