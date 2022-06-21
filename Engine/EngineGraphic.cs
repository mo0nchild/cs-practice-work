using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWork.Engine
{
    public sealed class EngineSprite : Engine.EngineObject
    {
        [Engine.EngineObjectInportConfiguration("SpriteImagePath")]
        public System.String? SpriteImagePath { get; private set; }
        public System.Drawing.Image? SpriteImage { get; private set; }

        [Engine.EngineObjectConstructorSelecter]
        public EngineSprite(string object_name) : base(object_name)
        {
            System.Drawing.Bitmap bitmap = new Bitmap(this.Geometry.Width, this.Geometry.Height);
            using (System.Drawing.Graphics image_graphic = Graphics.FromImage(bitmap))
            {
                image_graphic.DrawRectangle(Pens.Black, Position.X, Position.Y, Geometry.Width, Geometry.Height);
            }
            this.SpriteImage = (Image)bitmap.Clone();
        }

        public override void InitialOperation(EngineScene scene_instance)
        {
            if (this.SpriteImagePath is not null) 
            {
                using (System.IO.FileStream file_stream = new FileStream(this.SpriteImagePath, FileMode.Open))
                    this.SpriteImage = Image.FromStream(file_stream);
            }
        }

        public override void UpdateOperation(Graphics graphic)
        {
            graphic.DrawImage(this.SpriteImage!, new Rectangle(this.Position, this.Geometry));
        }
    }

    public static class ImageExtension
    {
        public enum FlipImageDirection : UInt16 { FlipX = 0, FlipY };

        public static Image RotateImage(this Image sourse_image, float rotation_angle)
        {
            System.Drawing.Bitmap buffer_image = new(sourse_image.Width, sourse_image.Height);
            using (System.Drawing.Graphics image_graphic = Graphics.FromImage(buffer_image))
            {
                image_graphic.TranslateTransform((float)buffer_image.Width / 2, (float)buffer_image.Height / 2);
                image_graphic.RotateTransform(rotation_angle);

                image_graphic.TranslateTransform(-(float)buffer_image.Width / 2, -(float)buffer_image.Height / 2);
                image_graphic.DrawImage(sourse_image, new Point(0, 0));
            }
            return buffer_image;
        }

        public static Image FlipImage(this Image sourse_image, FlipImageDirection flip_direction)
        {
            switch (flip_direction) 
            {
                case FlipImageDirection.FlipX: sourse_image.RotateFlip(RotateFlipType.RotateNoneFlipX); break;
                case FlipImageDirection.FlipY: sourse_image.RotateFlip(RotateFlipType.RotateNoneFlipY); break;
            }
            return sourse_image;
        }
    }
}
