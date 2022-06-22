using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Objects
{
    internal class MainPlayer : PracticeWork.Engine.EngineInputController
    {
        private int dir_x = 0, dir_y = 0;

        private Point cursor_position = new(0, 0);

        private const int speed = 5;

        [Engine.EngineObjectConstructorSelecter]
        public MainPlayer(string object_name) : base(object_name)
        {
            this.SetPosition(new Point { X = 20, Y = -20 });
        }

        public override void UpdateOperation(Graphics graphic)
        {
            this.SetPosition(new Point(speed * dir_x, speed * dir_y));
            graphic.DrawRectangle(Pens.Black, Position.X, Position.Y, Geometry.Width, Geometry.Height);
            graphic.DrawEllipse(Pens.Black, cursor_position.X, cursor_position.Y, 10, 10);
        }

        public override void KeyInputOperation(KeyEventArgs key_arg)
        {
            if (key_arg.KeyCode == Keys.Up) dir_y = 1;
            if (key_arg.KeyCode == Keys.Down) dir_y = -1;
            if (key_arg.KeyCode == Keys.Left) dir_x = -1;
            if (key_arg.KeyCode == Keys.Right) dir_x = 1;
        }

        public override void KeyReleaseOperation(KeyEventArgs key_arg)
        {
            if (key_arg.KeyCode == Keys.Up || key_arg.KeyCode == Keys.Down) dir_y = 0;
            if (key_arg.KeyCode == Keys.Left || key_arg.KeyCode == Keys.Right) dir_x = 0;
        }

        public override void MouseMoveOperation(MouseEventArgs mouse_arg)
        {
            cursor_position = new Point(mouse_arg.X, mouse_arg.Y);
        }

        public override void MouseClickOperation(MouseEventArgs mouse_arg)
        {

        }
    }
}
