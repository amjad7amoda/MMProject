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
            btnCompress = new Button();
            btnDecompress = new Button();
            cmbAlgorithm = new ComboBox();
            compressBox = new GroupBox();
            btnReset = new Button();
            btnReport = new Button();
            btnSave = new Button();
            plotCompression = new OxyPlot.WindowsForms.PlotView();
            nudSampleRate = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            nudQuantLevels = new NumericUpDown();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            btnCancel = new Button();
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
            btnBrowse.Location = new Point(437, 226);
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
            // btnCompress
            // 
            btnCompress.Location = new Point(680, 26);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(94, 29);
            btnCompress.TabIndex = 11;
            btnCompress.Text = "ضغط";
            btnCompress.UseVisualStyleBackColor = true;
            btnCompress.Click += btnCompress_Click;
            // 
            // btnDecompress
            // 
            btnDecompress.Location = new Point(680, 67);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(94, 29);
            btnDecompress.TabIndex = 12;
            btnDecompress.Text = "فك ضغط";
            btnDecompress.UseVisualStyleBackColor = true;
            btnDecompress.Click += btnDecompress_Click;
            // 
            // cmbAlgorithm
            // 
            cmbAlgorithm.FormattingEnabled = true;
            cmbAlgorithm.Items.AddRange(new object[] { "DPCM", "DLTA", "ADLTA" });
            cmbAlgorithm.Location = new Point(134, 26);
            cmbAlgorithm.Name = "cmbAlgorithm";
            cmbAlgorithm.Size = new Size(525, 28);
            cmbAlgorithm.TabIndex = 13;
            // 
            // compressBox
            // 
            compressBox.Controls.Add(btnReset);
            compressBox.Controls.Add(btnReport);
            compressBox.Controls.Add(btnSave);
            compressBox.Controls.Add(plotCompression);
            compressBox.Controls.Add(nudSampleRate);
            compressBox.Controls.Add(label2);
            compressBox.Controls.Add(label1);
            compressBox.Controls.Add(nudQuantLevels);
            compressBox.Controls.Add(lblStatus);
            compressBox.Controls.Add(progressBar);
            compressBox.Controls.Add(btnCancel);
            compressBox.Controls.Add(cmbAlgorithm);
            compressBox.Controls.Add(btnCompress);
            compressBox.Controls.Add(btnDecompress);
            compressBox.Location = new Point(44, 308);
            compressBox.Name = "compressBox";
            compressBox.Size = new Size(906, 395);
            compressBox.TabIndex = 14;
            compressBox.TabStop = false;
            compressBox.Text = "Compress Box";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(792, 67);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(94, 29);
            btnReset.TabIndex = 24;
            btnReset.Text = "إعادة تعيين";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnReport
            // 
            btnReport.Location = new Point(18, 71);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(94, 29);
            btnReport.TabIndex = 23;
            btnReport.Text = "تقرير";
            btnReport.UseVisualStyleBackColor = true;
            btnReport.Click += btnReport_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(18, 26);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 22;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // plotCompression
            // 
            plotCompression.BackColor = SystemColors.ControlDark;
            plotCompression.Location = new Point(18, 147);
            plotCompression.Name = "plotCompression";
            plotCompression.PanCursor = Cursors.Hand;
            plotCompression.Size = new Size(868, 242);
            plotCompression.TabIndex = 21;
            plotCompression.Text = "plotView1";
            plotCompression.ZoomHorizontalCursor = Cursors.SizeWE;
            plotCompression.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotCompression.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // nudSampleRate
            // 
            nudSampleRate.Enabled = false;
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
            nudQuantLevels.Enabled = false;
            nudQuantLevels.Location = new Point(571, 73);
            nudQuantLevels.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nudQuantLevels.Name = "nudQuantLevels";
            nudQuantLevels.Size = new Size(88, 27);
            nudQuantLevels.TabIndex = 16;
            nudQuantLevels.TextAlign = HorizontalAlignment.Center;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(695, 111);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 16;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(134, 108);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(525, 29);
            progressBar.TabIndex = 15;
            progressBar.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(792, 26);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
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
            ClientSize = new Size(962, 715);
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
        private Button btnCompress;
        private Button btnDecompress;
        private ComboBox cmbAlgorithm;
        private GroupBox compressBox;
        private GroupBox groupBox1;
        private Button btnCancel;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label label2;
        private Label label1;
        private NumericUpDown nudQuantLevels;
        private NumericUpDown nudSampleRate;
        private OxyPlot.WindowsForms.PlotView plotCompression;
        private Button btnSave;
        private Button btnReport;
        private Button btnReset;
    }
}
