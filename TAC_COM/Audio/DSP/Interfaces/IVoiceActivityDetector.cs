using WebRtcVadSharp;

namespace TAC_COM.Audio.DSP.Interfaces
{
    public interface IVoiceActivityDetector : IDisposable
    {
        /// <summary>
        /// Gets or sets the double value representing the
        /// delay in Ms after voice activity stops and the 
        /// <see cref="VoiceActivityStopped"/> event triggering.
        /// </summary>
        double HoldTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="WebRtcVad.OperatingMode"/>.
        /// </summary>
        OperatingMode OperatingMode { get; set; }

        /// <summary>
        /// Gets or sets the boolean value representing if speech
        /// is detected.
        /// </summary>
        bool State { get; set; }

        /// <summary>
        /// Event triggered when voice activity is detected;
        /// </summary>
        event EventHandler? VoiceActivityDetected;

        /// <summary>
        /// Event triggered when voice activity ends and the 
        /// <see cref="HoldTime"/> has elapsed.
        /// </summary>
        event EventHandler? VoiceActivityStopped;

        /// <summary>
        /// Method to analyse a buffer of bytes for voice activity.
        /// </summary>
        /// <remarks>
        /// The buffer must be exactly 30ms of 48kHz 16bit PCM audio.
        /// </remarks>
        /// <param name="buffer"></param>
        void Process(byte[] buffer);
    }
}