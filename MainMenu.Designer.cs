namespace PracticeWork
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.player_picturebox = new System.Windows.Forms.PictureBox();
            this.enemy_picturebox = new System.Windows.Forms.PictureBox();
            this.main_logo = new System.Windows.Forms.Label();
            this.enemycount_numeric = new System.Windows.Forms.NumericUpDown();
            this.playerlife_numeric = new System.Windows.Forms.NumericUpDown();
            this.enemyspeed_numeric = new System.Windows.Forms.NumericUpDown();
            this.playerspeed_numeric = new System.Windows.Forms.NumericUpDown();
            this.debug_checkbox = new System.Windows.Forms.CheckBox();
            this.enemycount_label = new System.Windows.Forms.Label();
            this.playerlife_label = new System.Windows.Forms.Label();
            this.playerspeed_label = new System.Windows.Forms.Label();
            this.enemyspeed_label = new System.Windows.Forms.Label();
            this.play_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.player_picturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemy_picturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemycount_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerlife_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyspeed_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerspeed_numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // player_picturebox
            // 
            this.player_picturebox.BackColor = System.Drawing.Color.Transparent;
            this.player_picturebox.Location = new System.Drawing.Point(37, 12);
            this.player_picturebox.Name = "player_picturebox";
            this.player_picturebox.Size = new System.Drawing.Size(84, 80);
            this.player_picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.player_picturebox.TabIndex = 0;
            this.player_picturebox.TabStop = false;
            // 
            // enemy_picturebox
            // 
            this.enemy_picturebox.BackColor = System.Drawing.Color.Transparent;
            this.enemy_picturebox.Location = new System.Drawing.Point(359, 11);
            this.enemy_picturebox.Name = "enemy_picturebox";
            this.enemy_picturebox.Size = new System.Drawing.Size(84, 80);
            this.enemy_picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.enemy_picturebox.TabIndex = 1;
            this.enemy_picturebox.TabStop = false;
            // 
            // main_logo
            // 
            this.main_logo.AutoSize = true;
            this.main_logo.BackColor = System.Drawing.Color.Transparent;
            this.main_logo.Cursor = System.Windows.Forms.Cursors.No;
            this.main_logo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.main_logo.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.main_logo.ForeColor = System.Drawing.Color.DarkRed;
            this.main_logo.Location = new System.Drawing.Point(110, 13);
            this.main_logo.Name = "main_logo";
            this.main_logo.Size = new System.Drawing.Size(263, 75);
            this.main_logo.TabIndex = 2;
            this.main_logo.Text = "Zombiu";
            // 
            // enemycount_numeric
            // 
            this.enemycount_numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.enemycount_numeric.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemycount_numeric.Location = new System.Drawing.Point(63, 242);
            this.enemycount_numeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.enemycount_numeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.enemycount_numeric.Name = "enemycount_numeric";
            this.enemycount_numeric.Size = new System.Drawing.Size(84, 25);
            this.enemycount_numeric.TabIndex = 3;
            this.enemycount_numeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.enemycount_numeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // playerlife_numeric
            // 
            this.playerlife_numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playerlife_numeric.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playerlife_numeric.Location = new System.Drawing.Point(63, 309);
            this.playerlife_numeric.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.playerlife_numeric.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.playerlife_numeric.Name = "playerlife_numeric";
            this.playerlife_numeric.Size = new System.Drawing.Size(84, 25);
            this.playerlife_numeric.TabIndex = 4;
            this.playerlife_numeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.playerlife_numeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // enemyspeed_numeric
            // 
            this.enemyspeed_numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.enemyspeed_numeric.DecimalPlaces = 2;
            this.enemyspeed_numeric.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemyspeed_numeric.Location = new System.Drawing.Point(203, 242);
            this.enemyspeed_numeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.enemyspeed_numeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.enemyspeed_numeric.Name = "enemyspeed_numeric";
            this.enemyspeed_numeric.Size = new System.Drawing.Size(96, 25);
            this.enemyspeed_numeric.TabIndex = 5;
            this.enemyspeed_numeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.enemyspeed_numeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // playerspeed_numeric
            // 
            this.playerspeed_numeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playerspeed_numeric.DecimalPlaces = 2;
            this.playerspeed_numeric.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playerspeed_numeric.Location = new System.Drawing.Point(203, 309);
            this.playerspeed_numeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.playerspeed_numeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.playerspeed_numeric.Name = "playerspeed_numeric";
            this.playerspeed_numeric.Size = new System.Drawing.Size(96, 25);
            this.playerspeed_numeric.TabIndex = 6;
            this.playerspeed_numeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.playerspeed_numeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // debug_checkbox
            // 
            this.debug_checkbox.AutoSize = true;
            this.debug_checkbox.BackColor = System.Drawing.Color.Transparent;
            this.debug_checkbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debug_checkbox.ForeColor = System.Drawing.SystemColors.Control;
            this.debug_checkbox.Location = new System.Drawing.Point(347, 246);
            this.debug_checkbox.Name = "debug_checkbox";
            this.debug_checkbox.Size = new System.Drawing.Size(75, 25);
            this.debug_checkbox.TabIndex = 7;
            this.debug_checkbox.Text = "Debug";
            this.debug_checkbox.UseVisualStyleBackColor = false;
            // 
            // enemycount_label
            // 
            this.enemycount_label.AutoSize = true;
            this.enemycount_label.BackColor = System.Drawing.Color.Transparent;
            this.enemycount_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemycount_label.ForeColor = System.Drawing.Color.White;
            this.enemycount_label.Location = new System.Drawing.Point(63, 213);
            this.enemycount_label.Name = "enemycount_label";
            this.enemycount_label.Size = new System.Drawing.Size(96, 19);
            this.enemycount_label.TabIndex = 8;
            this.enemycount_label.Text = "Enemy count: ";
            // 
            // playerlife_label
            // 
            this.playerlife_label.AutoSize = true;
            this.playerlife_label.BackColor = System.Drawing.Color.Transparent;
            this.playerlife_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playerlife_label.ForeColor = System.Drawing.Color.White;
            this.playerlife_label.Location = new System.Drawing.Point(63, 281);
            this.playerlife_label.Name = "playerlife_label";
            this.playerlife_label.Size = new System.Drawing.Size(74, 19);
            this.playerlife_label.TabIndex = 9;
            this.playerlife_label.Text = "Player life: ";
            // 
            // playerspeed_label
            // 
            this.playerspeed_label.AutoSize = true;
            this.playerspeed_label.BackColor = System.Drawing.Color.Transparent;
            this.playerspeed_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playerspeed_label.ForeColor = System.Drawing.Color.White;
            this.playerspeed_label.Location = new System.Drawing.Point(203, 281);
            this.playerspeed_label.Name = "playerspeed_label";
            this.playerspeed_label.Size = new System.Drawing.Size(93, 19);
            this.playerspeed_label.TabIndex = 11;
            this.playerspeed_label.Text = "Player speed: ";
            // 
            // enemyspeed_label
            // 
            this.enemyspeed_label.AutoSize = true;
            this.enemyspeed_label.BackColor = System.Drawing.Color.Transparent;
            this.enemyspeed_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemyspeed_label.ForeColor = System.Drawing.Color.White;
            this.enemyspeed_label.Location = new System.Drawing.Point(203, 213);
            this.enemyspeed_label.Name = "enemyspeed_label";
            this.enemyspeed_label.Size = new System.Drawing.Size(97, 19);
            this.enemyspeed_label.TabIndex = 10;
            this.enemyspeed_label.Text = "Enemy speed: ";
            // 
            // play_button
            // 
            this.play_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.play_button.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.play_button.Location = new System.Drawing.Point(63, 137);
            this.play_button.Name = "play_button";
            this.play_button.Size = new System.Drawing.Size(359, 50);
            this.play_button.TabIndex = 12;
            this.play_button.Text = "Play game";
            this.play_button.UseVisualStyleBackColor = true;
            this.play_button.Click += new System.EventHandler(this.play_button_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.play_button);
            this.Controls.Add(this.playerspeed_label);
            this.Controls.Add(this.enemyspeed_label);
            this.Controls.Add(this.playerlife_label);
            this.Controls.Add(this.enemycount_label);
            this.Controls.Add(this.debug_checkbox);
            this.Controls.Add(this.playerspeed_numeric);
            this.Controls.Add(this.enemyspeed_numeric);
            this.Controls.Add(this.playerlife_numeric);
            this.Controls.Add(this.enemycount_numeric);
            this.Controls.Add(this.main_logo);
            this.Controls.Add(this.enemy_picturebox);
            this.Controls.Add(this.player_picturebox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainMenu";
            this.Text = "Zombiu";
            ((System.ComponentModel.ISupportInitialize)(this.player_picturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemy_picturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemycount_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerlife_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyspeed_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerspeed_numeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox player_picturebox;
        private PictureBox enemy_picturebox;
        private Label main_logo;
        private NumericUpDown enemycount_numeric;
        private NumericUpDown playerlife_numeric;
        private NumericUpDown enemyspeed_numeric;
        private NumericUpDown playerspeed_numeric;
        private CheckBox debug_checkbox;
        private Label enemycount_label;
        private Label playerlife_label;
        private Label playerspeed_label;
        private Label enemyspeed_label;
        private Button play_button;
    }
}