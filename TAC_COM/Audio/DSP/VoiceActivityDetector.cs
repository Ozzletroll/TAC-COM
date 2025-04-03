using System.Timers;
using CSCore;
using WebRtcVadSharp;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Analyses a given <see cref="IWaveSource"/> for voice activity using a <see cref="webRtcVad"/>.
    /// </summary>
    /// <param name="_waveSource"> The <see cref="IWaveSource"/> to analyse for voice activity.</param>
    public class VoiceActivityDetector : IDisposable
    {
        private System.Timers.Timer timer;
        private readonly WebRtcVad webRtcVad = new()
        {
            FrameLength = FrameLength.Is30ms,
            SampleRate = SampleRate.Is48kHz,
            OperatingMode = OperatingMode.VeryAggressive,
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="VoiceActivityDetector"/>.
        /// </summary>
        /// <param name="_waveSource"> The <see cref="IWaveSource"/> to monitor.</param>
        public VoiceActivityDetector()
        {
            timer = new(HoldTime);
            timer.Elapsed += OnCloseTimerElapsed;
            timer.AutoReset = false;
        }

        /// <summary>
        /// Gets or sets the <see cref="WebRtcVad.OperatingMode"/>.
        /// </summary>
        public OperatingMode OperatingMode
        {
            get => webRtcVad.OperatingMode;
            set
            {
                webRtcVad.OperatingMode = value;
            }
        }

        private double holdTime = 1000;

        /// <summary>
        /// Gets or sets the double value representing the
        /// delay in Ms after voice activity stops and the 
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

        public void Process(byte[] buffer)
        {
            State = webRtcVad.HasSpeech(buffer);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            webRtcVad.Dispose();
        }
    }
}
