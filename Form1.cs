using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private float[] fullSamples;
        private CancellationTokenSource compressionCts;
        private System.Diagnostics.Stopwatch compressionStopwatch = new System.Diagnostics.Stopwatch();
        private int audioChannels = 1;
        private int waveformDisplayLength = 0;
        private PlotModel compressionPlot;
        private LineSeries ratioSeries;
        private LineSeries speedSeries;
        private int lastReportedSample = 0;
        private System.Diagnostics.Stopwatch segmentWatch = new System.Diagnostics.Stopwatch();
        private int bitsPerSample = 16;

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
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "أختر الملف الصوتي";
                ofd.Filter = "Audio Files|*.wav;*.mp3;*.aac";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadAudioFile(ofd.FileName);
                }
            }
        }

        private void LoadAudioFile(string filePath)
        {
            StopPlayback();

            currentFilePath = filePath;
            lblFilePath.Text = $"📂 {Path.GetFileName(filePath)}";
            seekSample = 0;

            LoadWaveformSamples(filePath);

            btnPlay.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = false;

            plotWaveform.Invalidate();

            audioFileDetails();
        }

        private void audioFileDetails()
        {

            TimeSpan duration   = audioReader.TotalTime;
            int channels = audioReader.WaveFormat.Channels;
            int sampleRate = audioReader.WaveFormat.SampleRate;
            int bitRate = audioReader.WaveFormat.AverageBytesPerSecond * 8;
            string encoding = audioReader.WaveFormat.Encoding.ToString();
            FileInfo fi = new FileInfo(currentFilePath);
            double fileSize = fi.Length / (1024.0 * 1024.0);


            audioDetailsLbl.Text =
                $"duration  : {duration  :mm\\:ss}\n" +
                $"Channels: {channels}\n" +
                $"Sample Rate: {sampleRate} Hz\n" +
                $"Bit Rate: {bitRate / 1000} kbps\n" +
                $"Encoding: {encoding}\n" +
                $"File Size: {fileSize:F2} MB";

            nudSampleRate.Value = sampleRate; nudSampleRate.Enabled = true;
            nudQuantLevels.Value = bitRate; nudQuantLevels.Enabled = true;
        }

        private void LoadWaveformSamples(string filePath)
        {

            string ext = Path.GetExtension(filePath).ToLower();

            if (ext == ".wav")
            {
                using (var wfr = new WaveFileReader(filePath))
                    bitsPerSample = wfr.WaveFormat.BitsPerSample;
            }

            audioReader = new AudioFileReader(filePath);
            audioChannels = audioReader.WaveFormat.Channels;

            int bytesPerFrame = (audioReader.WaveFormat.BitsPerSample / 8) * audioReader.WaveFormat.Channels;
            totalSamples = (int)(audioReader.Length / bytesPerFrame);

            var fullList = new List<float>();
            float[] tmpBuf = new float[4092];
            int read;

            // Full Samples for Compression (not downsampled)
            while ((read = audioReader.Read(tmpBuf, 0, tmpBuf.Length)) > 0)
                for (int i = 0; i < read; i++)
                    fullList.Add(tmpBuf[i]);
            
            fullSamples = fullList.ToArray();


            // Drawing Samples (Downsampled for Display)
            int displaySamples = plotWaveform.Width > 0 ? plotWaveform.Width : 800;
            int chunkSize = Math.Max(1, totalSamples / audioChannels / displaySamples);
            var displayList = new List<float>();

            for (int i = 0; i < fullSamples.Length; i += chunkSize)
            {
                float max = 0;
                int end = Math.Min(i + chunkSize, fullSamples.Length);
                for (int j = i; j < end; j++)
                    if (Math.Abs(fullSamples[j]) > max)
                        max = Math.Abs(fullSamples[j]);
                displayList.Add(max);
            }

            cachedSamples = displayList.ToArray();
            waveformDisplayLength = cachedSamples.Length;
            DrawWaveform(cachedSamples, 0);
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
                IsAxisVisible = true,
                Minimum = -1,
                Maximum = 1,
                MinimumPadding = 0,
                MaximumPadding = 0
            });

            int n = samples.Length;

            // Played Part (Blue)
            var playedSeriesTop = new LineSeries { Color = OxyColors.DeepSkyBlue, StrokeThickness = 1 };
            var playedSeriesBottom = new LineSeries { Color = OxyColors.DeepSkyBlue, StrokeThickness = 1 };

            // Remain Part (Green)
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

            // White Line for Current Position
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
            if (IsDisposed || !IsHandleCreated)
                return;

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

            int bytesPerFrame = (audioReader.WaveFormat.BitsPerSample / 8) * audioReader.WaveFormat.Channels;
            seekSample = bytesPerFrame > 0 ? (int)(audioReader.Position / bytesPerFrame) : 0;

            TimeSpan current = audioReader.CurrentTime;
            TimeSpan total = audioReader.TotalTime;
            lblCurrentTime.Text = $"{current:mm\\:ss} / {total:mm\\:ss}";

            int playedIndex = totalSamples > 0 && waveformDisplayLength > 0
            ? (int)((float)seekSample / totalSamples * waveformDisplayLength)
            : 0;

            DrawWaveform(cachedSamples, playedIndex);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopPlayback();
            playbackTimer?.Dispose();
            base.OnFormClosing(e);
        }

        public string CurrentFilePath => currentFilePath;

        private async void btnCompress_Click(object sender, EventArgs e)
        {
            if (cachedSamples == null || cachedSamples.Length == 0)
            {
                MessageBox.Show("لا يوجد ملف صوتي محمّل.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "حفظ الملف المضغوط";
                sfd.Filter = "Compressed Audio|*.caud";
                sfd.FileName = Path.GetFileNameWithoutExtension(currentFilePath) + "_compressed";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string outputPath = sfd.FileName;
                int selectedAlgo = cmbAlgorithm.SelectedIndex;
                float[] samples = fullSamples;

                int sampleRate = (int)nudSampleRate.Value;
                int quantLevels = (int)nudQuantLevels.Value;

                // UI - Start Compress Edits
                compressBtn.Enabled = false;
                decompressBtn.Enabled = false;
                cancelBtn.Enabled = true;
                cancelBtn.Visible = true;
                

                compressionCts = new CancellationTokenSource();
                var token = compressionCts.Token;

                InitCompressionPlot();
                progressBar.Value = 0;
                progressBar.Visible = true;
                plotCompression.Visible = true;

                long originalSize = new FileInfo(currentFilePath).Length;

                var progress = new Progress<CompressionProgressData>(data =>
                {
                    int percent = (int)((float)data.SampleIndex / data.TotalSamples * 100);
                    progressBar.Value = Math.Min(percent, 100);
                    lblStatus.Text = $"Compressing... {percent}%";

                    double compressionRatio = (1.0 - (double)data.CompressedBytes / originalSize) * 100;
                    compressionRatio = Math.Max(0, Math.Min(compressionRatio, 100));

                    int samplesProcessed = data.SampleIndex - lastReportedSample;
                    double elapsedSec = segmentWatch.Elapsed.TotalSeconds;
                    double kSamplesPerSec = elapsedSec > 0 ? (samplesProcessed / 1000.0) / elapsedSec : 0;

                    lastReportedSample = data.SampleIndex;
                    segmentWatch.Restart();

                    ratioSeries.Points.Add(new DataPoint(percent, compressionRatio));
                    speedSeries.Points.Add(new DataPoint(percent, kSamplesPerSec));

                    compressionPlot.InvalidatePlot(true);
                });
                compressionStopwatch.Restart();

                string header;
                byte[] compressed = null;

                switch (selectedAlgo)
                {
                    case 0: header = "DLTA"; compressed = await Task.Run(() => CompressDelta(samples, token, progress)); break;
                    case 1: header = "ADLTA"; compressed = await Task.Run(() => CompressADM(samples, token, progress)); break;
                    case 2: header = "DPCM"; compressed = await Task.Run(() => CompressDPCM(samples, token, progress)); break;
                    default: return;
                }


                if (compressed == null)
                {
                    progressBar.Value = 0;
                    lblStatus.Text = "Compression canceled";
                    compressBtn.Enabled = true;
                    decompressBtn.Enabled = true;
                    cancelBtn.Enabled = false;
                    cancelBtn.Visible = false;
                    return;
                }

                compressionStopwatch.Stop();
                SaveCompressed(outputPath, header, compressed, sampleRate, quantLevels, fullSamples.Length);

                long compressedSize = new FileInfo(outputPath).Length;
                double ratio = (1.0 - (double)compressedSize / originalSize) * 100;
                double elapsed2 = compressionStopwatch.Elapsed.TotalSeconds;

                ShowCompressionReport(header, originalSize, compressedSize, ratio, elapsed2, sampleRate, quantLevels);

                lblStatus.Text = $"Compressed. You save {ratio:F1}%";
                progressBar.Value = 100;
                compressBtn.Enabled = true;
                decompressBtn.Enabled = true;
                cancelBtn.Enabled = false;
                cancelBtn.Visible = false;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            compressionCts?.Cancel();
        }
        private void btnDecompress_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "اختر الملف المضغوط";
                ofd.Filter = "Compressed Audio|*.caud";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "حفظ الملف بعد فك الضغط";
                    sfd.Filter = "WAV Files|*.wav";
                    sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName) + "_decompressed";

                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    string header;
                    int srOut, qlOut, channelsOut, bitsPerSample;
                    float[] decompressed = LoadAndDecompress(ofd.FileName, out header, out srOut, out qlOut, out channelsOut, out bitsPerSample);

                    if (decompressed == null || decompressed.Length == 0)
                    {
                        MessageBox.Show("Failed to decompress.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Must use IEEE float format to match WriteSamples
                    var format = new WaveFormat(srOut, bitsPerSample, channelsOut);
                    MessageBox.Show($"{bitsPerSample}");
                    using (var writer = new WaveFileWriter(sfd.FileName, format))
                    {
                        if (bitsPerSample == 8)
                        {
                            // 8-bit WAV unsigned (0–255)
                            byte[] pcmBytes = new byte[decompressed.Length];
                            for (int i = 0; i < decompressed.Length; i++)
                                pcmBytes[i] = (byte)Math.Clamp((decompressed[i] + 1f) * 127.5f, 0, 255);
                            writer.Write(pcmBytes, 0, pcmBytes.Length);
                        }
                        else // 16-bit
                        {
                            short[] pcm = new short[decompressed.Length];
                            for (int i = 0; i < decompressed.Length; i++)
                                pcm[i] = (short)Math.Clamp(decompressed[i] * 32767f, -32768f, 32767f);
                            byte[] pcmBytes = new byte[pcm.Length * 2];
                            Buffer.BlockCopy(pcm, 0, pcmBytes, 0, pcmBytes.Length);
                            writer.Write(pcmBytes, 0, pcmBytes.Length);
                        }
                    }

                    MessageBox.Show(
                        $"Algorithm : {header}\n" +
                        $"Samples   : {decompressed.Length:N0}\n" +
                        $"Saved to  : {Path.GetFileName(sfd.FileName)}",
                        "Decompression Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private float[] LoadAndDecompress(string path, out string header, out int sampleRate, out int quantLevels, out int channels, out int bitsPerSample)
        {
            header = null; sampleRate = 44100; quantLevels = 256; channels = 1; bitsPerSample = 8;
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                using (var br = new BinaryReader(fs))
                {
                    header = new string(br.ReadChars(5)).Trim();
                    sampleRate = br.ReadInt32();
                    quantLevels = br.ReadInt32();
                    int byteCount = br.ReadInt32();
                    channels = br.ReadInt32();
                    int originalSampleCount = br.ReadInt32();
                    bitsPerSample = br.ReadInt32();
                    byte[] data = br.ReadBytes(byteCount);

                    switch (header)
                    {
                        case "DLTA": return DecompressDelta(data, originalSampleCount);
                        case "ADLTA": return DecompressADM(data, originalSampleCount);
                        case "DPCM": return DecompressDPCM(data, originalSampleCount);
                        default: return null;
                    }
                }
            }
            catch { return null; }
        }
        private void SaveCompressed(string path, string header, byte[] data, int sampleRate, int quantLevels, int originalSampleCount)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(header.PadRight(5).ToCharArray(), 0, 5);
                bw.Write(sampleRate);
                bw.Write(quantLevels);
                bw.Write(data.Length);
                bw.Write(audioChannels);
                bw.Write(originalSampleCount);
                bw.Write(bitsPerSample);
                bw.Write(data);
            }
        }


        private record CompressionProgressData(int SampleIndex, int TotalSamples, long CompressedBytes);


        // DPCM 
        private byte[] CompressDPCM(float[] samples, CancellationToken token, IProgress<CompressionProgressData> progress)
        {
            if (samples == null || samples.Length == 0) return Array.Empty<byte>();

            byte[] result = new byte[samples.Length];
            float previous = 0f;

            for (int i = 0; i < samples.Length; i++)
            {
                if (token.IsCancellationRequested) return null;

                float diff = samples[i] - previous;

                int quantized = (int)Math.Clamp(diff * 127f, -128f, 127f);
                result[i] = (byte)(quantized & 0xFF);
                previous = Math.Clamp(previous + quantized / 127f, -1f, 1f);

                if (i % 10000 == 0)
                    progress?.Report(new CompressionProgressData(i, samples.Length, i));
            }

            progress?.Report(new CompressionProgressData(samples.Length, samples.Length, samples.Length));
            return result;
        }

        private float[] DecompressDPCM(byte[] data, int originalCount)
        {
            if (data == null || data.Length == 0) return Array.Empty<float>();

            float[] samples = new float[originalCount];
            float previous = 0f;

            for (int i = 0; i < originalCount; i++)
            {
                // Convert back from unsigned byte to signed
                int quantized = (sbyte)data[i];

                previous = Math.Clamp(previous + quantized / 127f, -1f, 1f);
                samples[i] = previous;
            }

            return samples;
        }


        // Delta Modualtion
        private byte[] CompressDelta(float[] samples, CancellationToken token, IProgress<CompressionProgressData> progress)
        {
            if (samples == null || samples.Length == 0) return Array.Empty<byte>();

            int byteCount = (samples.Length + 7) / 8;
            byte[] result = new byte[byteCount];
            float predicted = 0f;
            float step = 0.01f;

            for (int i = 0; i < samples.Length; i++)
            {
                if (token.IsCancellationRequested) return null;

                int byteIndex = i / 8;
                int bitIndex = i % 8;

                if (samples[i] >= predicted)
                {
                    result[byteIndex] |= (byte)(1 << bitIndex);
                    predicted += step;
                }
                else
                {
                    predicted -= step;
                }

                predicted = Math.Clamp(predicted, -1f, 1f);

                if (i % 10000 == 0)
                    progress?.Report(new CompressionProgressData(i, samples.Length, i / 8));

            }

            progress?.Report(new CompressionProgressData(samples.Length, samples.Length, samples.Length / 8));
            return result;
        }
        private float[] DecompressDelta(byte[] data, int originalCount)
        {
            if (data == null || data.Length == 0) return Array.Empty<float>();

            float[] samples = new float[originalCount];
            float predicted = 0f;
            float step = 0.01f;

            for (int i = 0; i < originalCount; i++)
            {
                int byteIndex = i / 8;
                int bitIndex = i % 8;
                
                bool bit = (data[byteIndex] & (1 << bitIndex)) != 0;

                if (bit)
                    predicted += step;
                else
                    predicted -= step;

                predicted = Math.Clamp(predicted, -1f, 1f);
                samples[i] = predicted;
            }

            return samples;
        }
            



        // Adaptive Delta Modulation
        private byte[] CompressADM(float[] samples, CancellationToken token, IProgress<CompressionProgressData> progress)
        {
            if (samples == null || samples.Length == 0)
                return Array.Empty<byte>();

            int byteCount = (samples.Length + 7) / 8;
            byte[] result = new byte[byteCount];

            float predicted = 0f;

            float step = 0.01f;
            const float minStep = 0.0005f;
            const float maxStep = 0.1f;

            int previousBit = -1;

            for (int i = 0; i < samples.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return null;

                int currentBit;

                if (samples[i] >= predicted)
                {
                    currentBit = 1;
                    predicted += step;

                    result[i / 8] |= (byte)(1 << (i % 8));
                }
                else
                {
                    currentBit = 0;
                    predicted -= step;
                }

                // Adaptive step update
                if (previousBit != -1)
                {
                    if (currentBit == previousBit)
                    {
                        step *= 1.5f; // increase step
                    }
                    else
                    {
                        step *= 0.75f; // decrease step
                    }

                    step = Math.Clamp(step, minStep, maxStep);
                }

                previousBit = currentBit;
                predicted = Math.Clamp(predicted, -1f, 1f);
                if (i % 10000 == 0)
                    progress?.Report(new CompressionProgressData(i, samples.Length, i / 8));
            }

            progress?.Report(new CompressionProgressData(samples.Length, samples.Length, samples.Length / 8));
            return result;
        }

        private float[] DecompressADM(byte[] data, int originalCount)
        {
            float[] samples = new float[originalCount];

            float predicted = 0f;

            float step = 0.01f;
            const float minStep = 0.0005f;
            const float maxStep = 0.1f;

            int previousBit = -1;

            for (int i = 0; i < originalCount; i++)
            {
                bool bit = (data[i / 8] & (1 << (i % 8))) != 0;

                int currentBit = bit ? 1 : 0;

                if (bit)
                    predicted += step;
                else
                    predicted -= step;

                predicted = Math.Clamp(predicted, -1f, 1f);

                samples[i] = predicted;

                // Adaptive step update
                if (previousBit != -1)
                {
                    if (currentBit == previousBit)
                        step *= 1.5f;
                    else
                        step *= 0.75f;

                    step = Math.Clamp(step, minStep, maxStep);
                }

                previousBit = currentBit;
            }

            return samples;
        }



        private void InitCompressionPlot()
        {
            int compressionStep = 0;
            lastReportedSample = 0;

            compressionPlot = new PlotModel
            {
                Background = OxyColors.Black,
                PlotAreaBorderColor = OxyColors.Gray,
                TextColor = OxyColors.White,
                TitleColor = OxyColors.White,
                Title = "Compression Progress"
            };

            compressionPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Progress %",
                Minimum = 0,
                Maximum = 100,
                TextColor = OxyColors.White,
                TitleColor = OxyColors.White,
                TicklineColor = OxyColors.Gray
            });

            // Y-axis for Compression Ratio (Left)
            compressionPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Compression Ratio %",
                Minimum = 0,
                Maximum = 100,
                TextColor = OxyColors.DeepSkyBlue,
                TitleColor = OxyColors.DeepSkyBlue,
                TicklineColor = OxyColors.Gray,
                Key = "ratio"
            });

            // Y-axis for Speed (Right)
            compressionPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Right,
                Title = "Speed (kSamples/sec)",
                Minimum = 0,
                TextColor = OxyColors.Yellow,
                TitleColor = OxyColors.Yellow,
                TicklineColor = OxyColors.Gray,
                Key = "speed"
            });

            ratioSeries = new LineSeries
            {
                Title = "Compression Ratio %",
                Color = OxyColors.DeepSkyBlue,
                StrokeThickness = 2,
                YAxisKey = "ratio"
            };

            speedSeries = new LineSeries
            {
                Title = "Speed (kSamples/sec)",
                Color = OxyColors.Yellow,
                StrokeThickness = 2,
                YAxisKey = "speed"
            };

            compressionPlot.Series.Add(ratioSeries);
            compressionPlot.Series.Add(speedSeries);

            compressionPlot.Legends.Add(new OxyPlot.Legends.Legend
            {
                LegendPosition = OxyPlot.Legends.LegendPosition.TopLeft,
                LegendTextColor = OxyColors.White
            });

            plotCompression.Model = compressionPlot;
            segmentWatch.Restart();
        }

        private void ShowCompressionReport(string algo, long original, long compressed,
            double ratio, double elapsed, int sampleRate, int quantLevels)
        {
            string report =
                $"━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                $"  Compression Report\n" +
                $"━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                $"Algorithm    : {algo}\n" +
                $"Sample Rate  : {sampleRate} Hz\n" +
                $"Quant Levels : {quantLevels}\n\n" +
                $"Original     : {original / 1024.0:F1} KB\n" +
                $"Compressed   : {compressed / 1024.0:F1} KB\n" +
                $"Saved        : {ratio:F1}%\n\n" +
                $"Time Elapsed : {elapsed:F2} sec\n" +
                $"━━━━━━━━━━━━━━━━━━━━━━━━";

            MessageBox.Show(report, "Compression Report",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
