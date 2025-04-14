using CSCore;
using WebRtcVadSharp;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface representing the processor responsible for
    /// assembling the signal processing chain, ready for use
    /// by the <see cref="IAudioManager"/>.
    /// </summary>
    public interface IAudioProcessor
    {
        /// <summary>
        /// Gets or sets the value representing if the <see cref="IAudioProcessor"/>
        /// is initialised and ready for playback and recording.
        /// </summary>
        bool HasInitialised { get; set; }

        /// <summary>
        /// Gets or sets the value of the noise gate
        /// threshold level in decibels.
        /// </summary>
        float NoiseGateThreshold { get; set; }

        /// <summary>
        /// Gets or sets the value of the user gain level
        /// adjustment in decibels.
        /// </summary>
        float UserGainLevel { get; set; }

        /// <summary>
        /// Gets or sets the value of the looping noise sfx
        /// volume adjustment as a value between 0 and 1.
        /// </summary>
        float UserNoiseLevel { get; set; }

        /// <summary>
        /// Gets or sets the current frequency value of the ring
        /// modulator as a value between 0 and 1.
        /// </summary>
        float RingModulationWetDryMix { get; set; }

        /// <summary>
        /// Gets or sets the value representing if noise
        /// suppression is enabled.
        /// </summary>
        bool UseNoiseSuppressor { get; set; }

        /// <summary>
        /// Gets or sets the size of the buffer to use
        /// in milliseconds.
        /// </summary>
        int BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the value representing if
        /// the <see cref="Audio.DSP.VoiceActivityDetector"/>
        /// is required.
        /// </summary>
        bool UseVoiceActivityDetector { get; set; }

        /// <summary>
        /// Gets or sets the value representing if the 
        /// <see cref="Audio.DSP.VoiceActivityDetector.OperatingMode"/>.
        /// </summary>
        OperatingMode OperatingMode { get; set; }

        /// <summary>
        /// Gets or sets the value representing the time between
        /// voice activity ending and the end of the transmission
        /// effect in ms.
        /// </summary>
        double HoldTime { get; set; }

        /// <summary>
        /// Method to manually dispose of the <see cref="IAudioProcessor"/>'s resources.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Initialises the various <see cref="SoundInSource"/>'s for use in
        /// the other signal chains. This method must be called prior to
        /// <see cref="ReturnCompleteSignalChain"/>.
        /// </summary>
        /// <param name="inputWrapper">The <see cref="IWasapiCaptureWrapper"/> from 
        /// which the <see cref="SoundInSource"/> is created.</param>
        /// <param name="profile">The <see cref="Profile"/> to be set as <see cref="activeProfile"/>.</param>
        void Initialise(IWasapiCaptureWrapper inputWrapper, IProfile profile, CancellationToken token);

        /// <summary>
        /// Returns the full combined <see cref="IWaveSource"/> signal chain for 
        /// initialisation with the CSCore soundOut in the <see cref="AudioManager"/>.
        /// </summary>
        /// <returns>The complete assembled <see cref="IWaveSource"/>.</returns>
        IWaveSource? ReturnCompleteSignalChain();

        /// <summary>
        /// Occurs when voice activity is detected.
        /// </summary>
        event EventHandler VoiceActivityDetected;

        /// <summary>
        /// Occurs when voice activity stops.
        /// </summary>
        event EventHandler VoiceActivityStopped;

        /// <summary>
        /// Sets the respective volume levels of the <see cref="wetNoiseMixLevel"/> and <see cref="dryMixLevel"/>.
        /// </summary>
        /// <param name="bypassState"> Boolean representing <see cref="AudioManager.BypassState"/>. </param>
        void SetMixerLevels(bool bypassState);
    }
}