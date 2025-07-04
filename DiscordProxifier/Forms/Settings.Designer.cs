namespace DiscordUnblocker
    {
    partial class Settings
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
            this.DragPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ModeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Discard_Btn = new System.Windows.Forms.Button();
            this.Save_Btn = new System.Windows.Forms.Button();
            this.GostPort_Label = new System.Windows.Forms.Label();
            this.TorPort_Label = new System.Windows.Forms.Label();
            this.GostPort_box = new System.Windows.Forms.TextBox();
            this.MainPort_Box = new System.Windows.Forms.TextBox();
            this.AutoUpdateShortcust_Box = new System.Windows.Forms.CheckBox();
            this.OpenAtStart_Box = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DragPanel
            // 
            this.DragPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.DragPanel.Location = new System.Drawing.Point(0, -1);
            this.DragPanel.Name = "DragPanel";
            this.DragPanel.Size = new System.Drawing.Size(280, 35);
            this.DragPanel.TabIndex = 13;
            this.DragPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseDown);
            this.DragPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseMove);
            this.DragPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ModeBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Discard_Btn);
            this.panel1.Controls.Add(this.Save_Btn);
            this.panel1.Controls.Add(this.GostPort_Label);
            this.panel1.Controls.Add(this.TorPort_Label);
            this.panel1.Controls.Add(this.GostPort_box);
            this.panel1.Controls.Add(this.MainPort_Box);
            this.panel1.Controls.Add(this.AutoUpdateShortcust_Box);
            this.panel1.Controls.Add(this.OpenAtStart_Box);
            this.panel1.Location = new System.Drawing.Point(8, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 249);
            this.panel1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(9, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "!Only Proxified Shortcuts are launched on startup!";
            // 
            // ModeBox
            // 
            this.ModeBox.FormattingEnabled = true;
            this.ModeBox.Items.AddRange(new object[] {
            "Tor"});
            this.ModeBox.Location = new System.Drawing.Point(97, 50);
            this.ModeBox.Name = "ModeBox";
            this.ModeBox.Size = new System.Drawing.Size(83, 21);
            this.ModeBox.TabIndex = 12;
            this.ModeBox.SelectedIndexChanged += new System.EventHandler(this.ModeBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Mode ";
            // 
            // Discard_Btn
            // 
            this.Discard_Btn.AutoEllipsis = true;
            this.Discard_Btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Discard_Btn.FlatAppearance.BorderSize = 0;
            this.Discard_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Discard_Btn.ForeColor = System.Drawing.Color.Red;
            this.Discard_Btn.Location = new System.Drawing.Point(122, 205);
            this.Discard_Btn.Name = "Discard_Btn";
            this.Discard_Btn.Size = new System.Drawing.Size(58, 32);
            this.Discard_Btn.TabIndex = 10;
            this.Discard_Btn.Text = "Discard";
            this.Discard_Btn.UseVisualStyleBackColor = false;
            this.Discard_Btn.Click += new System.EventHandler(this.Discard_Btn_Click);
            // 
            // Save_Btn
            // 
            this.Save_Btn.AutoEllipsis = true;
            this.Save_Btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Save_Btn.FlatAppearance.BorderSize = 0;
            this.Save_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save_Btn.ForeColor = System.Drawing.Color.White;
            this.Save_Btn.Location = new System.Drawing.Point(68, 205);
            this.Save_Btn.Name = "Save_Btn";
            this.Save_Btn.Size = new System.Drawing.Size(48, 32);
            this.Save_Btn.TabIndex = 9;
            this.Save_Btn.Text = "Save";
            this.Save_Btn.UseVisualStyleBackColor = false;
            this.Save_Btn.Click += new System.EventHandler(this.Save_Btn_Click);
            // 
            // GostPort_Label
            // 
            this.GostPort_Label.AutoSize = true;
            this.GostPort_Label.ForeColor = System.Drawing.Color.White;
            this.GostPort_Label.Location = new System.Drawing.Point(3, 106);
            this.GostPort_Label.Name = "GostPort_Label";
            this.GostPort_Label.Size = new System.Drawing.Size(51, 13);
            this.GostPort_Label.TabIndex = 5;
            this.GostPort_Label.Text = "Gost Port";
            // 
            // TorPort_Label
            // 
            this.TorPort_Label.AutoSize = true;
            this.TorPort_Label.ForeColor = System.Drawing.Color.White;
            this.TorPort_Label.Location = new System.Drawing.Point(3, 80);
            this.TorPort_Label.Name = "TorPort_Label";
            this.TorPort_Label.Size = new System.Drawing.Size(81, 13);
            this.TorPort_Label.TabIndex = 4;
            this.TorPort_Label.Text = "Main Proxy Port";
            // 
            // GostPort_box
            // 
            this.GostPort_box.Location = new System.Drawing.Point(97, 103);
            this.GostPort_box.Name = "GostPort_box";
            this.GostPort_box.Size = new System.Drawing.Size(83, 20);
            this.GostPort_box.TabIndex = 3;
            this.GostPort_box.TextChanged += new System.EventHandler(this.GostPort_box_TextChanged);
            // 
            // MainPort_Box
            // 
            this.MainPort_Box.Location = new System.Drawing.Point(97, 77);
            this.MainPort_Box.Name = "MainPort_Box";
            this.MainPort_Box.Size = new System.Drawing.Size(83, 20);
            this.MainPort_Box.TabIndex = 2;
            this.MainPort_Box.TextChanged += new System.EventHandler(this.MainPort_Box_TextChanged);
            // 
            // AutoUpdateShortcust_Box
            // 
            this.AutoUpdateShortcust_Box.AutoSize = true;
            this.AutoUpdateShortcust_Box.ForeColor = System.Drawing.Color.White;
            this.AutoUpdateShortcust_Box.Location = new System.Drawing.Point(3, 26);
            this.AutoUpdateShortcust_Box.Name = "AutoUpdateShortcust_Box";
            this.AutoUpdateShortcust_Box.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoUpdateShortcust_Box.Size = new System.Drawing.Size(177, 17);
            this.AutoUpdateShortcust_Box.TabIndex = 1;
            this.AutoUpdateShortcust_Box.Text = "Auto Update Proxified Shortcuts";
            this.AutoUpdateShortcust_Box.UseVisualStyleBackColor = true;
            this.AutoUpdateShortcust_Box.CheckedChanged += new System.EventHandler(this.AutoUpdateShortcust_Box_CheckedChanged);
            // 
            // OpenAtStart_Box
            // 
            this.OpenAtStart_Box.AutoSize = true;
            this.OpenAtStart_Box.ForeColor = System.Drawing.Color.White;
            this.OpenAtStart_Box.Location = new System.Drawing.Point(3, 3);
            this.OpenAtStart_Box.Name = "OpenAtStart_Box";
            this.OpenAtStart_Box.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.OpenAtStart_Box.Size = new System.Drawing.Size(99, 17);
            this.OpenAtStart_Box.TabIndex = 0;
            this.OpenAtStart_Box.Text = "Open at startup";
            this.OpenAtStart_Box.UseVisualStyleBackColor = true;
            this.OpenAtStart_Box.CheckedChanged += new System.EventHandler(this.OpenAtStart_Box_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(278, 325);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DragPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.Panel DragPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox AutoUpdateShortcust_Box;
        private System.Windows.Forms.CheckBox OpenAtStart_Box;
        private System.Windows.Forms.Label GostPort_Label;
        private System.Windows.Forms.Label TorPort_Label;
        private System.Windows.Forms.TextBox GostPort_box;
        private System.Windows.Forms.TextBox MainPort_Box;
        private System.Windows.Forms.Button Discard_Btn;
        private System.Windows.Forms.Button Save_Btn;
        private System.Windows.Forms.ComboBox ModeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        }
    }