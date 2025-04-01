using CSCore;
using WebRtcVadSharp;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Analyses a given <see cref="IWaveSource"/> for voice activity using a <see cref="webRtcVad"/>.
    /// </summary>
    /// <param name="_waveSource"> The <see cref="IWaveSource"/> to analyse for voice activity.</param>
    public class VoiceActivityDetector(IWaveSource _waveSource) : IWaveSource
    {
        private readonly IWaveSource waveSource = _waveSource;

        private readonly WebRtcVad webRtcVad = new()
        {
            FrameLength = FrameLength.Is30ms,
            SampleRate = SampleRate.Is48kHz,
            OperatingMode = OperatingMode.LowBitrate,
        };

        private bool state;

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
                        OnVoiceActivityDetected(EventArgs.Empty);
                    }
                    else
                    {
                        OnVoiceActivityStopped(EventArgs.Empty);
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
        /// Event triggered when voice activity ends.
        /// </summary>
        public event EventHandler? VoiceActivityStopped;

        /// <summary>
        /// Raises the <see cref="VoiceActivityStopped"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnVoiceActivityStopped(EventArgs e)
        {
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
            State = webRtcVad.HasSpeech(buffer);
            return waveSource.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            waveSource.Dispose();
        }
    }
}
