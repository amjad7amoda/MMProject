namespace Project
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblDrop = new Label();
            btnBrowse = new Button();
            lblFilePath = new Label();
            btnPlay = new Button();
            btnStop = new Button();
            lblCurrentTime = new Label();
            btnPause = new Button();
            plotWaveform = new OxyPlot.WindowsForms.PlotView();
            SuspendLayout();
            // 
            // lblDrop
            // 
            lblDrop.AutoSize = true;
            lblDrop.Location = new Point(129, 180);
            lblDrop.Name = "lblDrop";
            lblDrop.Size = new Size(229, 20);
            lblDrop.TabIndex = 1;
            lblDrop.Text = "اسحب الملف الصوتي او أضغط هنا";
            lblDrop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Bottom;
            btnBrowse.Location = new Point(29, 176);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(94, 29);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browser";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblFilePath
            // 
            lblFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblFilePath.Location = new Point(235, 128);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(385, 20);
            lblFilePath.TabIndex = 3;
            // 
            // btnPlay
            // 
            btnPlay.Enabled = false;
            btnPlay.Location = new Point(651, 186);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(124, 29);
            btnPlay.TabIndex = 4;
            btnPlay.Text = "▶ تشغيل";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(651, 256);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(124, 29);
            btnStop.TabIndex = 5;
            btnStop.Text = "⏹ إيقاف";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblCurrentTime
            // 
            lblCurrentTime.AutoSize = true;
            lblCurrentTime.Location = new Point(669, 163);
            lblCurrentTime.Name = "lblCurrentTime";
            lblCurrentTime.Size = new Size(93, 20);
            lblCurrentTime.TabIndex = 6;
            lblCurrentTime.Text = "00:00 / 00:00";
            // 
            // btnPause
            // 
            btnPause.Enabled = false;
            btnPause.Location = new Point(651, 221);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(124, 29);
            btnPause.TabIndex = 8;
            btnPause.Text = "⏸ إيقاف مؤقت";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // plotWaveform
            // 
            plotWaveform.AllowDrop = true;
            plotWaveform.BackColor = SystemColors.ActiveCaption;
            plotWaveform.Dock = DockStyle.Top;
            plotWaveform.Location = new Point(0, 0);
            plotWaveform.Name = "plotWaveform";
            plotWaveform.PanCursor = Cursors.Hand;
            plotWaveform.Size = new Size(800, 150);
            plotWaveform.TabIndex = 9;
            plotWaveform.Text = "plotView1";
            plotWaveform.ZoomHorizontalCursor = Cursors.SizeWE;
            plotWaveform.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotWaveform.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 514);
            Controls.Add(plotWaveform);
            Controls.Add(btnPause);
            Controls.Add(lblCurrentTime);
            Controls.Add(btnStop);
            Controls.Add(btnPlay);
            Controls.Add(lblFilePath);
            Controls.Add(btnBrowse);
            Controls.Add(lblDrop);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblDrop;
        private Button btnBrowse;
        private Label lblFilePath;
        private Button btnPlay;
        private Button btnStop;
        private Label lblCurrentTime;
        private Button btnPause;
        private OxyPlot.WindowsForms.PlotView plotWaveform;
    }
}
