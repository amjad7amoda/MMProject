using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;

namespace Project
{
    public partial class Form1 : Form
    {

        private string currentFilePath = string.Empty;
        private PlotModel waveformPlot;
        private WaveOutEvent waveOut;
        private AudioFileReader audioReader;
        private System.Windows.Forms.Timer playbackTimer;
        private int seekSample = 0;
        private int totalSamples = 0;
        private float[] cachedSamples;

        public Form1()
        {
            InitializeComponent();
            SetupWaveformPanel();
            SetupPlaybackTimer();
        }



        private void SetupWaveformPanel()
        {
            plotWaveform.AllowDrop = true;
            this.AllowDrop = true;
            plotWaveform.DragEnter += PnlWaveform_DragEnter;
            plotWaveform.DragDrop += PnlWaveform_DragDrop;
            this.DragEnter += PnlWaveform_DragEnter;
            this.DragDrop += PnlWaveform_DragDrop;
            InitEmptyPlot();
        }

        private void InitEmptyPlot()
        {
            waveformPlot = new PlotModel
            {
                Background = OxyColors.Black,
                PlotAreaBorderColor = OxyColors.Transparent
            };

            // رسالة ترحيبية
            waveformPlot.Annotations.Add(
                new OxyPlot.Annotations.TextAnnotation
                {
                    Text = "📂 اسحب ملفاً صوتياً هنا أو اضغط Browse",
                    TextPosition = new DataPoint(0.5, 0.5),
                    TextColor = OxyColors.Gray,
                    FontSize = 12,
                    TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center
                });

            plotWaveform.Model = waveformPlot;
        }

        private void SetupPlaybackTimer()
        {
            playbackTimer = new System.Windows.Forms.Timer();
            playbackTimer.Interval = 100;
            playbackTimer.Tick += PlaybackTimer_Tick;
        }


        private void PnlWaveform_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string ext = Path.GetExtension(files[0]).ToLower();
                if (ext == ".wav" || ext == ".mp3" || ext == ".flac" || ext == ".aiff")
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void PnlWaveform_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadAudioFile(files[0]);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "أختر الملف الصوتي";
                ofd.Filter = "Audio Files|*.wav;*.mp3;*.acc";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadAudioFile(ofd.FileName);
                }
            }
        }

        private void LoadAudioFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("الملف غير موجود!", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ext = Path.GetExtension(filePath).ToLower();
            string[] allowed = { ".wav", ".mp3", ".flac", ".aiff" };
            if (Array.IndexOf(allowed, ext) < 0)
            {
                MessageBox.Show("صيغة الملف غير مدعومة!\nالصيغ المدعومة: WAV, MP3, FLAC, AIFF",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StopPlayback();

            currentFilePath = filePath;
            lblFilePath.Text = $"📂 {Path.GetFileName(filePath)}";
            seekSample = 0;

            LoadWaveformSamples(filePath);

            btnPlay.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = false;

            plotWaveform.Invalidate();
        }
        private void LoadWaveformSamples(string filePath)
        {
            float[] allSamples;

            using (var reader = new AudioFileReader(filePath))
            {
                int channels = reader.WaveFormat.Channels;
                int bitsPerSample = reader.WaveFormat.BitsPerSample;
                totalSamples = (int)(reader.Length / (bitsPerSample / 8));

                // نأخذ عدد عينات مناسب للعرض
                int displaySamples = plotWaveform.Width > 0 ? plotWaveform.Width : 800;
                int chunkSize = Math.Max(1, totalSamples / channels / displaySamples);

                var samplesList = new System.Collections.Generic.List<float>();
                float[] buffer = new float[chunkSize * channels];
                int read;

                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    float max = 0;
                    for (int i = 0; i < read; i++)
                        if (Math.Abs(buffer[i]) > max)
                            max = Math.Abs(buffer[i]);
                    samplesList.Add(max);
                }

                allSamples = samplesList.ToArray();

                cachedSamples = allSamples;
                DrawWaveform(allSamples, 0);
            }

            // ارسم الـ Waveform
            DrawWaveform(allSamples, 0);
        }

        private void DrawWaveform(float[] samples, int playedIndex)
        {
            if (samples == null || samples.Length == 0) return;

            waveformPlot = new PlotModel
            {
                Background = OxyColors.Black,
                PlotAreaBorderColor = OxyColors.Transparent,
                Padding = new OxyThickness(0)
            };

            // إخفاء المحاور
            waveformPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                IsAxisVisible = false,
                MinimumPadding = 0,
                MaximumPadding = 0
            });
            waveformPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                IsAxisVisible = false,
                Minimum = -1,
                Maximum = 1,
                MinimumPadding = 0,
                MaximumPadding = 0
            });

            int n = samples.Length;

            // السلسلة الأولى: الجزء المشغَّل (أزرق)
            var playedSeriesTop = new LineSeries { Color = OxyColors.DeepSkyBlue, StrokeThickness = 1 };
            var playedSeriesBottom = new LineSeries { Color = OxyColors.DeepSkyBlue, StrokeThickness = 1 };

            // السلسلة الثانية: الجزء الباقي (أخضر)
            var remainSeriesTop = new LineSeries { Color = OxyColors.LimeGreen, StrokeThickness = 1 };
            var remainSeriesBottom = new LineSeries { Color = OxyColors.LimeGreen, StrokeThickness = 1 };

            for (int i = 0; i < n; i++)
            {
                float amp = samples[i];

                if (i <= playedIndex)
                {
                    playedSeriesTop.Points.Add(new DataPoint(i, amp));
                    playedSeriesBottom.Points.Add(new DataPoint(i, -amp));
                }
                else
                {
                    remainSeriesTop.Points.Add(new DataPoint(i, amp));
                    remainSeriesBottom.Points.Add(new DataPoint(i, -amp));
                }
            }

            waveformPlot.Series.Add(playedSeriesTop);
            waveformPlot.Series.Add(playedSeriesBottom);
            waveformPlot.Series.Add(remainSeriesTop);
            waveformPlot.Series.Add(remainSeriesBottom);

            // خط أبيض لموضع التشغيل
            if (playedIndex > 0)
            {
                waveformPlot.Annotations.Add(new OxyPlot.Annotations.LineAnnotation
                {
                    Type = OxyPlot.Annotations.LineAnnotationType.Vertical,
                    X = playedIndex,
                    Color = OxyColors.White,
                    LineStyle = LineStyle.Solid,
                    StrokeThickness = 2
                });
            }

            plotWaveform.Model = waveformPlot;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            // إذا كان مؤقفاً Resume، إذا جديد ابدأ من أول
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                playbackTimer.Start();
                btnPlay.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                return;
            }

            StartPlayback(seekSample);
        }


        private void btnPause_Click(object sender, EventArgs e)
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                playbackTimer.Stop();
                btnPlay.Enabled = true;
                btnPause.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopPlayback();
            seekSample = 0;
            DrawWaveform(cachedSamples, 0);
            lblCurrentTime.Text = "00:00 / 00:00";
        }

        private void StartPlayback(int fromSample)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (audioReader != null)
            {
                audioReader.Dispose();
                audioReader = null;
            }

            audioReader = new AudioFileReader(currentFilePath);

            if (fromSample > 0)
            {
                long bytePos = (long)fromSample
                    * (audioReader.WaveFormat.BitsPerSample / 8);
                bytePos = Math.Min(bytePos, audioReader.Length - 1);
                audioReader.Position = bytePos;
            }

            waveOut = new WaveOutEvent();
            waveOut.Init(audioReader);
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            waveOut.Play();

            playbackTimer.Start();
            btnPlay.Enabled = false;
            btnPause.Enabled = true;
            btnStop.Enabled = true;
        }

        private void StopPlayback()
        {
            playbackTimer.Stop();

            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (audioReader != null)
            {
                audioReader.Dispose();
                audioReader = null;
            }

            if (btnPlay != null) btnPlay.Enabled = !string.IsNullOrEmpty(currentFilePath);
            if (btnPause != null) btnPause.Enabled = false;
            if (btnStop != null) btnStop.Enabled = false;
        }

        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                seekSample = 0;
                lblCurrentTime.Text = "00:00 / 00:00";
                DrawWaveform(cachedSamples, 0);
                btnPlay.Enabled = true;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                playbackTimer.Stop();
            }));
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (audioReader == null || waveOut == null) return;

            int bytesPerSample = audioReader.WaveFormat.BitsPerSample / 8;
            seekSample = bytesPerSample > 0
                ? (int)(audioReader.Position / bytesPerSample)
                : 0;

            TimeSpan current = audioReader.CurrentTime;
            TimeSpan total = audioReader.TotalTime;
            lblCurrentTime.Text = $"{current:mm\\:ss} / {total:mm\\:ss}";

            int waveformLength = waveformPlot?.Series.Count > 0
                ? (int)((OxyPlot.Series.LineSeries)waveformPlot.Series[2]).Points.Count
                  + (int)((OxyPlot.Series.LineSeries)waveformPlot.Series[0]).Points.Count
                : 0;

            int playedIndex = totalSamples > 0 && waveformLength > 0 ? (int)((float)seekSample / totalSamples * waveformLength) : 0;

            DrawWaveform(cachedSamples, playedIndex);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopPlayback();
            playbackTimer?.Dispose();
            base.OnFormClosing(e);
        }

        public string CurrentFilePath => currentFilePath;

    }


}
