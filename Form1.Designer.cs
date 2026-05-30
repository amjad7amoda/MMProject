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
            audioDetailsLbl = new Label();
            compressBtn = new Button();
            decompressBtn = new Button();
            cmbAlgorithm = new ComboBox();
            compressBox = new GroupBox();
            nudSampleRate = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            nudQuantLevels = new NumericUpDown();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            cancelBtn = new Button();
            groupBox1 = new GroupBox();
            compressBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSampleRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantLevels).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblDrop
            // 
            lblDrop.AutoSize = true;
            lblDrop.Location = new Point(373, 186);
            lblDrop.Name = "lblDrop";
            lblDrop.Size = new Size(229, 20);
            lblDrop.TabIndex = 1;
            lblDrop.Text = "اسحب الملف الصوتي او أضغط هنا";
            lblDrop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Bottom;
            btnBrowse.Location = new Point(437, 221);
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
            lblFilePath.Size = new Size(547, 20);
            lblFilePath.TabIndex = 3;
            // 
            // btnPlay
            // 
            btnPlay.Enabled = false;
            btnPlay.Location = new Point(72, 28);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(124, 29);
            btnPlay.TabIndex = 4;
            btnPlay.Text = "▶ تشغيل";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(72, 98);
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
            lblCurrentTime.Location = new Point(90, 5);
            lblCurrentTime.Name = "lblCurrentTime";
            lblCurrentTime.Size = new Size(94, 17);
            lblCurrentTime.TabIndex = 6;
            lblCurrentTime.Text = "00:00 / 00:00";
            // 
            // btnPause
            // 
            btnPause.Enabled = false;
            btnPause.Location = new Point(72, 63);
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
            plotWaveform.Size = new Size(962, 150);
            plotWaveform.TabIndex = 9;
            plotWaveform.Text = "plotView1";
            plotWaveform.ZoomHorizontalCursor = Cursors.SizeWE;
            plotWaveform.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotWaveform.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // audioDetailsLbl
            // 
            audioDetailsLbl.AutoSize = true;
            audioDetailsLbl.Location = new Point(12, 163);
            audioDetailsLbl.Name = "audioDetailsLbl";
            audioDetailsLbl.Size = new Size(0, 20);
            audioDetailsLbl.TabIndex = 10;
            // 
            // compressBtn
            // 
            compressBtn.Location = new Point(680, 26);
            compressBtn.Name = "compressBtn";
            compressBtn.Size = new Size(94, 29);
            compressBtn.TabIndex = 11;
            compressBtn.Text = "ضغط";
            compressBtn.UseVisualStyleBackColor = true;
            compressBtn.Click += btnCompress_Click;
            // 
            // decompressBtn
            // 
            decompressBtn.Location = new Point(18, 26);
            decompressBtn.Name = "decompressBtn";
            decompressBtn.Size = new Size(94, 29);
            decompressBtn.TabIndex = 12;
            decompressBtn.Text = "فك ضغط";
            decompressBtn.UseVisualStyleBackColor = true;
            decompressBtn.Click += btnDecompress_Click;
            // 
            // cmbAlgorithm
            // 
            cmbAlgorithm.FormattingEnabled = true;
            cmbAlgorithm.Items.AddRange(new object[] { "DLTA", "ADLTA" });
            cmbAlgorithm.Location = new Point(134, 26);
            cmbAlgorithm.Name = "cmbAlgorithm";
            cmbAlgorithm.Size = new Size(525, 28);
            cmbAlgorithm.TabIndex = 13;
            // 
            // compressBox
            // 
            compressBox.Controls.Add(nudSampleRate);
            compressBox.Controls.Add(label2);
            compressBox.Controls.Add(label1);
            compressBox.Controls.Add(nudQuantLevels);
            compressBox.Controls.Add(lblStatus);
            compressBox.Controls.Add(progressBar);
            compressBox.Controls.Add(cancelBtn);
            compressBox.Controls.Add(cmbAlgorithm);
            compressBox.Controls.Add(compressBtn);
            compressBox.Controls.Add(decompressBtn);
            compressBox.Location = new Point(44, 370);
            compressBox.Name = "compressBox";
            compressBox.Size = new Size(906, 183);
            compressBox.TabIndex = 14;
            compressBox.TabStop = false;
            compressBox.Text = "Compress Box";
            // 
            // nudSampleRate
            // 
            nudSampleRate.Location = new Point(242, 73);
            nudSampleRate.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudSampleRate.Name = "nudSampleRate";
            nudSampleRate.Size = new Size(105, 27);
            nudSampleRate.TabIndex = 20;
            nudSampleRate.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(473, 76);
            label2.Name = "label2";
            label2.Size = new Size(93, 20);
            label2.TabIndex = 19;
            label2.Text = "Quant Levels";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(143, 76);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 18;
            label1.Text = "Sample Rate";
            // 
            // nudQuantLevels
            // 
            nudQuantLevels.Location = new Point(571, 73);
            nudQuantLevels.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nudQuantLevels.Name = "nudQuantLevels";
            nudQuantLevels.Size = new Size(88, 27);
            nudQuantLevels.TabIndex = 16;
            nudQuantLevels.TextAlign = HorizontalAlignment.Center;
            nudQuantLevels.Value = new decimal(new int[] { 256, 0, 0, 0 });
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(695, 121);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 16;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(134, 121);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(525, 29);
            progressBar.TabIndex = 15;
            progressBar.Visible = false;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(792, 26);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(94, 29);
            cancelBtn.TabIndex = 14;
            cancelBtn.Text = "إلغاء";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Visible = false;
            cancelBtn.Click += btnCancel_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.MistyRose;
            groupBox1.Controls.Add(btnPause);
            groupBox1.Controls.Add(btnPlay);
            groupBox1.Controls.Add(btnStop);
            groupBox1.Controls.Add(lblCurrentTime);
            groupBox1.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(685, 163);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(265, 139);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 619);
            Controls.Add(groupBox1);
            Controls.Add(compressBox);
            Controls.Add(audioDetailsLbl);
            Controls.Add(plotWaveform);
            Controls.Add(lblFilePath);
            Controls.Add(btnBrowse);
            Controls.Add(lblDrop);
            Name = "Form1";
            Text = "Form1";
            compressBox.ResumeLayout(false);
            compressBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSampleRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantLevels).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private Label audioDetailsLbl;
        private Button compressBtn;
        private Button decompressBtn;
        private ComboBox cmbAlgorithm;
        private GroupBox compressBox;
        private GroupBox groupBox1;
        private Button cancelBtn;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label label2;
        private Label label1;
        private NumericUpDown nudQuantLevels;
        private NumericUpDown nudSampleRate;
    }
}
