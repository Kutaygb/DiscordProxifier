using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DiscordUnblocker
{
    public partial class Settings : Form
    {
        public Configuration Configuration { get; set; }
        public Configuration NConfig = new Configuration();

        public Settings()
        {
            InitializeComponent();
        }

        private void CloseSettings_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private Point mouseOffset;
        private bool isDragging = false;

        private void DragPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseOffset = new Point(e.X, e.Y);
            }
        }

        private void DragPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                this.Location = newLocation;
            }
        }

        private void DragPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void OpenAtStart_Box_CheckedChanged(object sender, EventArgs e)
        {
            NConfig.OpenAtStartup = OpenAtStart_Box.Checked;
        }

        private void AutoUpdateShortcust_Box_CheckedChanged(object sender, EventArgs e)
        {
            NConfig.AutoUpdateProxified = AutoUpdateShortcust_Box.Checked;
        }

        private void LoadConfig(Configuration configuration)
        {
            //NConfig = configuration;
            OpenAtStart_Box.Checked = configuration.OpenAtStartup || false;
            AutoUpdateShortcust_Box.Checked = configuration.AutoUpdateProxified || false;
            ModeBox.SelectedItem =
                configuration.Mode != null && configuration.Mode.Length > 0
                    ? configuration.Mode
                    : "Tor";
            MainPort_Box.Text =
                configuration.MainProxyPort != null
                && configuration.MainProxyPort.ToString().Length > 0
                    ? configuration.MainProxyPort.ToString()
                    : "42213";
            GostPort_box.Text =
                configuration.GostPort != null && configuration.GostPort.ToString().Length > 0
                    ? configuration.GostPort.ToString()
                    : "42212";
        }

        private void Save_Btn_Click(object sender, EventArgs e)
        {
            Configuration = NConfig;

            File.WriteAllText(
                Directory.GetCurrentDirectory() + "\\Config.json",
                JsonConvert.SerializeObject(Configuration)
            );
            this.Close();
        }

        private void Discard_Btn_Click(object sender, EventArgs e)
        {
            LoadConfig(Configuration);
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            Configuration configuration = new Configuration();
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Config.json"))
                configuration = JsonConvert.DeserializeObject<Configuration>(
                    File.ReadAllText(Directory.GetCurrentDirectory() + "\\Config.json")
                );

            LoadConfig(configuration);
            Configuration = configuration;
        }

        private void ModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            NConfig.Mode = ModeBox.SelectedItem.ToString();
        }

        private void MainPort_Box_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(MainPort_Box.Text, out int am))
            {
                if (am > 65535)
                {
                    MessageBox.Show("Main Port must be lower than 65535!");
                    return;
                }
                if (am < 1)
                {
                    MessageBox.Show("Main Port must be higher than 0!");
                    return;
                }
                if (GostPort_box.Text == MainPort_Box.Text)
                {
                    MessageBox.Show("Main Port and Gost Port cannot be same!");

                    return;
                }
                NConfig.MainProxyPort = MainPort_Box.Text;
            }
            else
            {
                MessageBox.Show("Main Port Must Be Valid Int!");

                return;
            }
        }

        private void GostPort_box_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(GostPort_box.Text, out int am))
            {
                if (am > 65535)
                {
                    MessageBox.Show("Gost Port must be lower than 65535!");
                    return;
                }
                if (am < 1)
                {
                    MessageBox.Show("Gost Port must be higher than 0!");
                    return;
                }
                if (GostPort_box.Text == MainPort_Box.Text)
                {
                    MessageBox.Show("Gost Port and Main Port cannot be same!");

                    return;
                }
                NConfig.GostPort = GostPort_box.Text;
            }
            else
            {
                MessageBox.Show("Gost Port Must Be Valid Int!");

                return;
            }
        }
    }

    public class Configuration
    {
        public bool OpenAtStartup { get; set; }
        public bool AutoUpdateProxified { get; set; }
        public string Mode { get; set; }
        public string MainProxyPort { get; set; }
        public string GostPort { get; set; }
    }
}
