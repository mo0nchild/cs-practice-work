namespace PracticeWork
{
    public class MyPanel : Panel { public MyPanel() : base() => this.DoubleBuffered = true; }
    partial class MainScene
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private MyPanel main_panel  = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeMainPanel(Point position, int width, int height)
        {
            this.main_panel = new MyPanel();

            this.main_panel.Location = new System.Drawing.Point(position.X, position.Y);
            this.main_panel.Name = "MainPanel";
            this.main_panel.Size = new System.Drawing.Size(width, height);
            this.main_panel.TabIndex = 0;

            this.Controls.Add(main_panel);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "MainScene";
            this.Text = "Vampire";
            this.ResumeLayout(false);

        }

        #endregion
    }
}