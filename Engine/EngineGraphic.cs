using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Win = System.Windows;

namespace PracticeWork.Engine
{
    public sealed class EngineSprite : Engine.EngineObject
    {
        [Engine.EngineObjectImportConfiguration("SpriteImagePath")]
        public System.String? SpriteImagePath { get; protected set; }
        public System.Drawing.Image? SpriteImage { get; private set; }

        [Engine.EngineObjectConstructorSelecter]
        public EngineSprite(string object_name) : base(object_name)
        {
            System.Drawing.Bitmap bitmap = new Bitmap(this.Geometry.Width, this.Geometry.Height);
            using (System.Drawing.Graphics image_graphic = Graphics.FromImage(bitmap))
            {
                image_graphic.DrawRectangle(Pens.Black, new Rectangle(this.Position, this.Geometry));
            }
            this.SpriteImage = (Image)bitmap.Clone();
        }

        public void SpriteFlip(ImageExtension.FlipImageDirection direction) => this.SpriteImage?.FlipImage(direction);
        public void SpriteRotate(System.Double rotation_angle) => this.SpriteImage?.RotateImage((float)rotation_angle);

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

    public sealed class EngineAnimator : Engine.EngineObject
    {
        public record AnimationRecord(List<System.Drawing.Image> AnimationFrames);
        public sealed record AnimationLinkedFrame(string AnimationName, int CurrentFrame, bool Repeat);

        private Dictionary<string, EngineAnimator.AnimationRecord> animation_list = new();
        private System.Double animation_playing_speed = 1.0; 
        private EngineAnimator.AnimationLinkedFrame? current_frame = null;

        public System.Boolean AnimationIsPlaying { get; private set; } = false;
        public System.String? AnimationName { get => this.current_frame?.AnimationName; }

        [Engine.EngineObjectImportConfiguration("AnimationsContainer")]
        public Dictionary<string, string> AnimationsContainer { get; protected set; }

        [Engine.EngineObjectImportConfiguration("AnimationSpeed")]
        public System.Double AnimationsSpeed 
        { 
            protected set { if (value <= 1.0 && value > 0) this.animation_playing_speed = value; } 
            get => this.animation_playing_speed; 
        }

        [Engine.EngineObjectConstructorSelecter]
        public EngineAnimator(string object_name) : base(object_name) => this.AnimationsContainer = new();

        public override void InitialOperation(EngineScene scene_instance)
        {
            foreach (KeyValuePair<string, string> target_item in this.AnimationsContainer)
            {
                List<string> filepath_list = Directory.EnumerateFiles(target_item.Value, "*.png").ToList();
                if (filepath_list.Count == 0) continue;

                this.animation_list.Add(target_item.Key, new AnimationRecord(new(0)));
                filepath_list.ForEach(delegate (string target_file)
                {
                    this.animation_list[target_item.Key].AnimationFrames.Add(Image.FromFile(target_file));
                });
                this.current_frame = new(target_item.Key, 0, false);
            }
            Win::Forms.Timer animation_handling_timer = new() { Interval = (int)(60 / this.AnimationsSpeed) };
            animation_handling_timer.Tick += new EventHandler(this.AnimationHandling);
            animation_handling_timer.Start();
        }

        private void AnimationHandling(object? sender, System.EventArgs event_arg)
        {
            if (this.AnimationIsPlaying == true && this.current_frame != null)
            {
                var max_frame = this.animation_list[current_frame?.AnimationName!].AnimationFrames.Count;
                if (this.current_frame?.CurrentFrame < max_frame - 1)
                {
                    this.current_frame = new(current_frame.AnimationName, current_frame.CurrentFrame + 1, 
                        current_frame.Repeat);
                }
                else
                {
                    this.current_frame = new(current_frame!.AnimationName, 0, current_frame.Repeat);
                    if (this.current_frame!.Repeat != true) this.StopCurrentAnimation();
                } 
            }
        }

        public void PlayAnimation(System.String animation_name, bool is_repeat = true) 
        {
            if (this.animation_list.ContainsKey(animation_name) == true) 
            {
                this.current_frame = new EngineAnimator.AnimationLinkedFrame(animation_name, 0, is_repeat);
                this.AnimationIsPlaying = true;
            }
        }

        public void StopCurrentAnimation() => (this.AnimationIsPlaying, this.current_frame) 
            = (false, new ("", 0, false));

        public void FlipAllAnimationFrame(ImageExtension.FlipImageDirection direction) 
        {
            foreach (KeyValuePair<string, AnimationRecord> target_item in this.animation_list) 
            {
                target_item.Value.AnimationFrames.ForEach((Image image) => image.FlipImage(direction));
            }
        }

        public override void UpdateOperation(Graphics graphic)
        {
            if (this.current_frame != null) 
            {
                (string animation_name, int frame_index, _) = this.current_frame;
                if (this.animation_list.ContainsKey(animation_name) != true) return;

                Image output_image = this.animation_list[animation_name].AnimationFrames[frame_index];
                try
                {
                    graphic.DrawImage(output_image, new Rectangle(this.Position, this.Geometry));
                }
                catch(System.Exception) 
                {
                    Console.WriteLine(ParentObject!.Position.X + "; " + ParentObject!.Position.Y);
                    this.ParentObject.SetPosition(0, 0);
                }
            }
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
