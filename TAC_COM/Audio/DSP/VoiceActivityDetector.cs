using System.Timers;
using CSCore;
using WebRtcVadSharp;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Analyses a given <see cref="IWaveSource"/> for voice activity using a <see cref="webRtcVad"/>.
    /// </summary>
    /// <param name="_waveSource"> The <see cref="IWaveSource"/> to analyse for voice activity.</param>
    public class VoiceActivityDetector : IWaveSource
    {
        private readonly IWaveSource waveSource;
        private System.Timers.Timer timer;
        private readonly WebRtcVad webRtcVad = new()
        {
            FrameLength = FrameLength.Is30ms,
            SampleRate = SampleRate.Is48kHz,
            OperatingMode = OperatingMode.HighQuality,
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="VoiceActivityDetector"/>.
        /// </summary>
        /// <param name="_waveSource"> The <see cref="IWaveSource"/> to monitor.</param>
        public VoiceActivityDetector(IWaveSource _waveSource)
        {
            waveSource = _waveSource;
            timer = new(HoldTime);
            timer.Elapsed += OnCloseTimerElapsed;
            timer.AutoReset = false;
        }

        private double holdTime = 1000;

        /// <summary>
        /// Gets or sets the double value representing the
        /// delay after voice activity stops and the 
        /// <see cref="VoiceActivityStopped"/> event triggering.
        /// </summary>
        public double HoldTime
        {
            get => holdTime;
            set
            {
                holdTime = value;
                timer.Elapsed -= OnCloseTimerElapsed;
                timer = new(HoldTime);
                timer.Elapsed += OnCloseTimerElapsed;
                timer.AutoReset = false;
            }
        }

        private bool state;
        private bool isOpenEventTriggered = false;

        /// <summary>
        /// Gets or sets the boolean value representing if speech
        /// is detected.
        /// </summary>
        public bool State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    if (state)
                    {
                        timer.Stop();

                        if (!isOpenEventTriggered)
                        {
                            OnVoiceActivityDetected(EventArgs.Empty);
                            isOpenEventTriggered = true;
                        }
                    }
                    else
                    {
                        timer.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Event triggered when voice activity is detected;
        /// </summary>
        public event EventHandler? VoiceActivityDetected;

        /// <summary>
        /// Raises the <see cref="VoiceActivityDetected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnVoiceActivityDetected(EventArgs e)
        {
            VoiceActivityDetected?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered when voice activity ends and the 
        /// <see cref="HoldTime"/> has elapsed.
        /// </summary>
        public event EventHandler? VoiceActivityStopped;

        /// <summary>
        /// Raises the <see cref="VoiceActivityStopped"/> event
        /// and resets the event triggers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            isOpenEventTriggered = false;
            VoiceActivityStopped?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public bool CanSeek => waveSource.CanSeek;

        /// <inheritdoc/>
        public WaveFormat WaveFormat => waveSource.WaveFormat;

        /// <inheritdoc/>
        public long Position
        {
            get => waveSource.Position;
            set => waveSource.Position = value;
        }

        /// <inheritdoc/>
        public long Length => waveSource.Length;

        /// <inheritdoc/>
        /// <remarks>
        /// This is where the <see cref="webRtcVad"/> analyses
        /// the buffer for voice activity.
        /// </remarks>
        public int Read(byte[] buffer, int offset, int count)
        {
            var bytes = waveSource.Read(buffer, offset, count);
            State = webRtcVad.HasSpeech(buffer);
            return bytes;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            waveSource.Dispose();
        }
    }
}
