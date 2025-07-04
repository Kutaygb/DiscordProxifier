namespace DiscordUnblocker
    {
    partial class Logs
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
            this.CloseLogger = new System.Windows.Forms.Button();
            this.DragPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.DragPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseLogger
            // 
            this.CloseLogger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CloseLogger.FlatAppearance.BorderSize = 0;
            this.CloseLogger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseLogger.ForeColor = System.Drawing.Color.Red;
            this.CloseLogger.Location = new System.Drawing.Point(468, 1);
            this.CloseLogger.Name = "CloseLogger";
            this.CloseLogger.Size = new System.Drawing.Size(22, 32);
            this.CloseLogger.TabIndex = 8;
            this.CloseLogger.Text = "X";
            this.CloseLogger.UseVisualStyleBackColor = false;
            this.CloseLogger.Click += new System.EventHandler(this.CloseLogger_Click);
            // 
            // DragPanel
            // 
            this.DragPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.DragPanel.Controls.Add(this.CloseLogger);
            this.DragPanel.Location = new System.Drawing.Point(0, -1);
            this.DragPanel.Name = "DragPanel";
            this.DragPanel.Size = new System.Drawing.Size(493, 35);
            this.DragPanel.TabIndex = 13;
            this.DragPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseDown);
            this.DragPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseMove);
            this.DragPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DragPanel_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 40);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(468, 398);
            this.flowLayoutPanel1.TabIndex = 14;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // Logs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(492, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.DragPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Logs";
            this.Text = "Logs";
            this.DragPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.Button CloseLogger;
        private System.Windows.Forms.Panel DragPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        }
    }