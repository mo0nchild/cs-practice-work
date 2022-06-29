using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticeWork
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(@"..\..\..\Assets\Menu\background.png");
            this.player_picturebox.Image = Image.FromFile(@"..\..\..\Assets\Player\Idle\test_01.png");

            Image enemy_logo = Image.FromFile(@"..\..\..\Assets\Enemy\Idle\enemy_01.png");
            enemy_logo.RotateFlip(RotateFlipType.RotateNoneFlipX);

            this.enemy_picturebox.Image = enemy_logo;
        }

        private void play_button_Click(object sender, EventArgs e)
        {
            Form form = new MainScene()
            {
                PlayerLife = (int)this.playerlife_numeric.Value,
                PlayerSpeed = (double)this.playerspeed_numeric.Value,
                
                EnemyCount = (int)this.enemycount_numeric.Value,
                MaxEnemySpeed = (double)this.enemyspeed_numeric.Value,

                DebugMode = this.debug_checkbox.Checked,
                ChestSpawn = (int)this.chestspawn_numeric.Value
            };

            form.ShowDialog();
        }
    }
}
