namespace DiscordUnblocker
    {
    partial class MAIN
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
            if (disposing && ( components != null ))
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAIN));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.proxified_label = new System.Windows.Forms.TextBox();
            this.name_label = new System.Windows.Forms.TextBox();
            this.proxify = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.startwproxy = new System.Windows.Forms.Button();
            this.settings_btn = new System.Windows.Forms.Button();
            this.ShowLogs = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.FStop = new System.Windows.Forms.Button();
            this.StartServer = new System.Windows.Forms.Button();
            this.ServerStatus = new System.Windows.Forms.Label();
            this.DragPanel = new System.Windows.Forms.Panel();
            this.Traybtn = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ShortcutUpdater = new System.Windows.Forms.Timer(this.components);
            this.TRAY = new System.Windows.Forms.NotifyIcon(this.components);
            this.TRAY_CONTEXT = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TRAY_SERVER_STATUS_C = new System.Windows.Forms.ToolStripMenuItem();
            this.TRAY_SERVER_TOGGLE_C = new System.Windows.Forms.ToolStripMenuItem();
            this.TRAY_SHOW_APP_C = new System.Windows.Forms.ToolStripMenuItem();
            this.TRAY_EXIT_C = new System.Windows.Forms.ToolStripMenuItem();
            this.Versionlbl = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.DragPanel.SuspendLayout();
            this.TRAY_CONTEXT.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 5);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(136, 156);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.panel1.Controls.Add(this.proxified_label);
            this.panel1.Controls.Add(this.name_label);
            this.panel1.Controls.Add(this.proxify);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.startwproxy);
            this.panel1.Location = new System.Drawing.Point(155, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 155);
            this.panel1.TabIndex = 2;
            // 
            // proxified_label
            // 
            this.proxified_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.proxified_label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.proxified_label.ForeColor = System.Drawing.Color.White;
            this.proxified_label.Location = new System.Drawing.Point(3, 41);
            this.proxified_label.Name = "proxified_label";
            this.proxified_label.ReadOnly = true;
            this.proxified_label.Size = new System.Drawing.Size(137, 13);
            this.proxified_label.TabIndex = 11;
            this.proxified_label.Text = "Is Proxified : No";
            // 
            // name_label
            // 
            this.name_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.name_label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.name_label.ForeColor = System.Drawing.Color.White;
            this.name_label.Location = new System.Drawing.Point(3, 3);
            this.name_label.Multiline = true;
            this.name_label.Name = "name_label";
            this.name_label.ReadOnly = true;
            this.name_label.Size = new System.Drawing.Size(134, 27);
            this.name_label.TabIndex = 10;
            this.name_label.Text = "Client Name : Please Sellect Client";
            // 
            // proxify
            // 
            this.proxify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.proxify.FlatAppearance.BorderSize = 0;
            this.proxify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.proxify.ForeColor = System.Drawing.Color.White;
            this.proxify.Location = new System.Drawing.Point(3, 95);
            this.proxify.Name = "proxify";
            this.proxify.Size = new System.Drawing.Size(137, 23);
            this.proxify.TabIndex = 9;
            this.proxify.Text = "Proxify The Shortcut";
            this.proxify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.proxify.UseVisualStyleBackColor = false;
            this.proxify.Click += new System.EventHandler(this.proxify_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(3, 124);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Custom Proxy Settings";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // startwproxy
            // 
            this.startwproxy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.startwproxy.FlatAppearance.BorderSize = 0;
            this.startwproxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startwproxy.ForeColor = System.Drawing.Color.White;
            this.startwproxy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startwproxy.Location = new System.Drawing.Point(3, 66);
            this.startwproxy.Name = "startwproxy";
            this.startwproxy.Size = new System.Drawing.Size(137, 23);
            this.startwproxy.TabIndex = 4;
            this.startwproxy.Text = "Start With Proxy";
            this.startwproxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startwproxy.UseVisualStyleBackColor = false;
            this.startwproxy.Click += new System.EventHandler(this.startwproxy_Click);
            // 
            // settings_btn
            // 
            this.settings_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.settings_btn.FlatAppearance.BorderSize = 0;
            this.settings_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settings_btn.ForeColor = System.Drawing.Color.White;
            this.settings_btn.Location = new System.Drawing.Point(244, 286);
            this.settings_btn.Name = "settings_btn";
            this.settings_btn.Size = new System.Drawing.Size(76, 23);
            this.settings_btn.TabIndex = 3;
            this.settings_btn.Text = "Settings";
            this.settings_btn.UseVisualStyleBackColor = false;
            this.settings_btn.Click += new System.EventHandler(this.settingsbtn_Click);
            // 
            // ShowLogs
            // 
            this.ShowLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShowLogs.FlatAppearance.BorderSize = 0;
            this.ShowLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowLogs.ForeColor = System.Drawing.Color.White;
            this.ShowLogs.Location = new System.Drawing.Point(244, 257);
            this.ShowLogs.Name = "ShowLogs";
            this.ShowLogs.Size = new System.Drawing.Size(76, 23);
            this.ShowLogs.TabIndex = 4;
            this.ShowLogs.Text = "Logs";
            this.ShowLogs.UseVisualStyleBackColor = false;
            this.ShowLogs.Click += new System.EventHandler(this.ShowLogs_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(12, 41);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(308, 171);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel3.Controls.Add(this.FStop);
            this.panel3.Controls.Add(this.StartServer);
            this.panel3.Controls.Add(this.ServerStatus);
            this.panel3.Location = new System.Drawing.Point(12, 218);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(308, 35);
            this.panel3.TabIndex = 6;
            // 
            // FStop
            // 
            this.FStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.FStop.FlatAppearance.BorderSize = 0;
            this.FStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FStop.ForeColor = System.Drawing.Color.White;
            this.FStop.Location = new System.Drawing.Point(147, 6);
            this.FStop.Name = "FStop";
            this.FStop.Size = new System.Drawing.Size(76, 23);
            this.FStop.TabIndex = 5;
            this.FStop.Text = "Force Stop";
            this.FStop.UseVisualStyleBackColor = false;
            this.FStop.Click += new System.EventHandler(this.FStop_Click);
            // 
            // StartServer
            // 
            this.StartServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.StartServer.FlatAppearance.BorderSize = 0;
            this.StartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartServer.ForeColor = System.Drawing.Color.White;
            this.StartServer.Location = new System.Drawing.Point(229, 6);
            this.StartServer.Name = "StartServer";
            this.StartServer.Size = new System.Drawing.Size(76, 23);
            this.StartServer.TabIndex = 4;
            this.StartServer.Text = "Start Server";
            this.StartServer.UseVisualStyleBackColor = false;
            this.StartServer.Click += new System.EventHandler(this.ServerT_Click);
            // 
            // ServerStatus
            // 
            this.ServerStatus.AutoSize = true;
            this.ServerStatus.ForeColor = System.Drawing.Color.White;
            this.ServerStatus.Location = new System.Drawing.Point(10, 11);
            this.ServerStatus.Name = "ServerStatus";
            this.ServerStatus.Size = new System.Drawing.Size(108, 13);
            this.ServerStatus.TabIndex = 2;
            this.ServerStatus.Text = "Server Status : Down";
            // 
            // DragPanel
            // 
            this.DragPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.DragPanel.Controls.Add(this.Traybtn);
            this.DragPanel.Controls.Add(this.button4);
            this.DragPanel.Controls.Add(this.button2);
            this.DragPanel.Location = new System.Drawing.Point(0, 0);
            this.DragPanel.Name = "DragPanel";
            this.DragPanel.Size = new System.Drawing.Size(334, 35);
            this.DragPanel.TabIndex = 7;
            this.DragPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseDown);
            this.DragPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseMove);
            this.DragPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseUp);
            // 
            // Traybtn
            // 
            this.Traybtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Traybtn.FlatAppearance.BorderSize = 0;
            this.Traybtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Traybtn.ForeColor = System.Drawing.Color.White;
            this.Traybtn.Location = new System.Drawing.Point(254, 2);
            this.Traybtn.Name = "Traybtn";
            this.Traybtn.Size = new System.Drawing.Size(22, 32);
            this.Traybtn.TabIndex = 10;
            this.Traybtn.Text = "ᐯ";
            this.Traybtn.UseVisualStyleBackColor = false;
            this.Traybtn.Click += new System.EventHandler(this.Traybtn_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(282, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 32);
            this.button4.TabIndex = 9;
            this.button4.Text = "-";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.Minimize_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(309, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 32);
            this.button2.TabIndex = 8;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Exit_Click);
            // 
            // ShortcutUpdater
            // 
            this.ShortcutUpdater.Interval = 2000;
            this.ShortcutUpdater.Tick += new System.EventHandler(this.ShortcutUpdater_Tick);
            // 
            // TRAY
            // 
            this.TRAY.BalloonTipText = "Discord Unblocker Tray Icon";
            this.TRAY.BalloonTipTitle = "Discord Unblocker Tray Icon";
            this.TRAY.ContextMenuStrip = this.TRAY_CONTEXT;
            this.TRAY.Icon = ((System.Drawing.Icon)(resources.GetObject("TRAY.Icon")));
            this.TRAY.Text = "Discord Unblocker";
            // 
            // TRAY_CONTEXT
            // 
            this.TRAY_CONTEXT.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TRAY_SERVER_STATUS_C,
            this.TRAY_SERVER_TOGGLE_C,
            this.TRAY_SHOW_APP_C,
            this.TRAY_EXIT_C});
            this.TRAY_CONTEXT.Name = "TRAY_CONTEXT";
            this.TRAY_CONTEXT.Size = new System.Drawing.Size(182, 92);
            // 
            // TRAY_SERVER_STATUS_C
            // 
            this.TRAY_SERVER_STATUS_C.Enabled = false;
            this.TRAY_SERVER_STATUS_C.Name = "TRAY_SERVER_STATUS_C";
            this.TRAY_SERVER_STATUS_C.Size = new System.Drawing.Size(181, 22);
            this.TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
            // 
            // TRAY_SERVER_TOGGLE_C
            // 
            this.TRAY_SERVER_TOGGLE_C.Name = "TRAY_SERVER_TOGGLE_C";
            this.TRAY_SERVER_TOGGLE_C.Size = new System.Drawing.Size(181, 22);
            this.TRAY_SERVER_TOGGLE_C.Text = "Start Server";
            this.TRAY_SERVER_TOGGLE_C.Click += new System.EventHandler(this.TRAY_SERVER_TOGGLE_C_Click);
            // 
            // TRAY_SHOW_APP_C
            // 
            this.TRAY_SHOW_APP_C.Name = "TRAY_SHOW_APP_C";
            this.TRAY_SHOW_APP_C.Size = new System.Drawing.Size(181, 22);
            this.TRAY_SHOW_APP_C.Text = "Show App";
            this.TRAY_SHOW_APP_C.Click += new System.EventHandler(this.TRAY_SHOW_APP_C_Click);
            // 
            // TRAY_EXIT_C
            // 
            this.TRAY_EXIT_C.Name = "TRAY_EXIT_C";
            this.TRAY_EXIT_C.Size = new System.Drawing.Size(181, 22);
            this.TRAY_EXIT_C.Text = "Exit";
            this.TRAY_EXIT_C.Click += new System.EventHandler(this.TRAY_EXIT_C_Click);
            // 
            // Versionlbl
            // 
            this.Versionlbl.AutoSize = true;
            this.Versionlbl.ForeColor = System.Drawing.Color.White;
            this.Versionlbl.Location = new System.Drawing.Point(9, 291);
            this.Versionlbl.Name = "Versionlbl";
            this.Versionlbl.Size = new System.Drawing.Size(69, 13);
            this.Versionlbl.TabIndex = 1;
            this.Versionlbl.Text = "Version : ???";
            // 
            // MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(332, 320);
            this.Controls.Add(this.DragPanel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ShowLogs);
            this.Controls.Add(this.settings_btn);
            this.Controls.Add(this.Versionlbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MAIN";
            this.Text = "Discord Proxifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.DragPanel.ResumeLayout(false);
            this.TRAY_CONTEXT.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button startwproxy;
        private System.Windows.Forms.Button settings_btn;
        private System.Windows.Forms.Button proxify;
        private System.Windows.Forms.Button ShowLogs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label ServerStatus;
        private System.Windows.Forms.Button StartServer;
        private System.Windows.Forms.TextBox proxified_label;
        private System.Windows.Forms.TextBox name_label;
        private System.Windows.Forms.Panel DragPanel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer ShortcutUpdater;
        private System.Windows.Forms.Button FStop;
        private System.Windows.Forms.NotifyIcon TRAY;
        private System.Windows.Forms.ContextMenuStrip TRAY_CONTEXT;
        private System.Windows.Forms.ToolStripMenuItem TRAY_SERVER_STATUS_C;
        private System.Windows.Forms.ToolStripMenuItem TRAY_SERVER_TOGGLE_C;
        private System.Windows.Forms.ToolStripMenuItem TRAY_SHOW_APP_C;
        private System.Windows.Forms.ToolStripMenuItem TRAY_EXIT_C;
        private System.Windows.Forms.Button Traybtn;
        private System.Windows.Forms.Label Versionlbl;
        }
    }

