using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordUnblocker
{
    public partial class Logs : Form
    {
        Logger l = null;

        public Logs(Logger l_)
        {
            l = l_;
            InitializeComponent();
            this.Visible = false;
            l.AddLogs(this.flowLayoutPanel1);
            l.CurrentPanel = flowLayoutPanel1;
            this.Visible = true;
        }

        private void CloseLogger_Click(object sender, EventArgs e)
        {
            this.Hide();
            l.CurrentPanel = null;
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
    }
}
