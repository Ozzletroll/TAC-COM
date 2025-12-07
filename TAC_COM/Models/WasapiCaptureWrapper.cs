using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for the <see cref="WasapiCapture"/>, to facilitate
    /// easier testing.
    /// </summary>
    public class WasapiCaptureWrapper : IWasapiCaptureWrapper
    {
        private readonly CancellationToken cancellationToken;
        private readonly bool useExclusiveMode;

        /// <summary>
        /// Initialises a new instance of the <see cref="WasapiCaptureWrapper"/>.
        /// </summary>
        /// <param name="_useExclusiveMode">Boolean value representing if the wasapicapture should run
        /// in exclusive mode.</param>
        /// <param name="_token"> Cancellation token issued upon playback start toggle.</param>
        public WasapiCaptureWrapper(bool _useExclusiveMode, int channels, CancellationToken _token)
        {
            cancellationToken = _token;
            useExclusiveMode = _useExclusiveMode;

            wasapiCapture = CreateWasapiCapture(useExclusiveMode, channels);
        }

        private WasapiCapture wasapiCapture;

        public WasapiCapture WasapiCapture
        {
            get => wasapiCapture;
            set
            {
                wasapiCapture = value;
            }
        }

        public MMDevice Device
        {
            get => wasapiCapture.Device;
            set
            {
                wasapiCapture.Device = value;
            }
        }

        public event EventHandler<DataAvailableEventArgs> DataAvailable
        {
            add => wasapiCapture.DataAvailable += value;
            remove => wasapiCapture.DataAvailable -= value;
        }

        public event EventHandler<RecordingStoppedEventArgs> Stopped
        {
            add => wasapiCapture.Stopped += value;
            remove => wasapiCapture.Stopped -= value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WasapiCapture"/> class.
        /// </summary>
        /// <param name="useExclusiveMode"> Indicates whether to use the device in exclusive mode.</param>
        /// <returns> A <see cref="WasapiCapture"/> instance.</returns>
        private static WasapiCapture CreateWasapiCapture(bool useExclusiveMode, int channels)
        {
            WaveFormat waveFormat;
            if (channels < 2)
            {
                waveFormat = new WaveFormat(48000, 24, channels);
            }
            else
            {
                Guid guid = WaveFormatExtensible.SubTypeFromWaveFormat(new WaveFormat(48000, 24, channels));
                waveFormat = new WaveFormatExtensible(48000, 24, channels, guid);
            }

            if (useExclusiveMode)
            {
                return new WasapiCapture(
                    false,
                    AudioClientShareMode.Exclusive,
                    25,
                    waveFormat,
                    ThreadPriority.Highest);
            }
            else
            {
                return new WasapiCapture(
                    true,
                    AudioClientShareMode.Shared,
                    25,
                    waveFormat,
                    ThreadPriority.Highest);
            }
        }

        public void Dispose()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    GC.SuppressFinalize(this);
                    wasapiCapture.Dispose();
                }
                catch (Exception e)
                {
                    DebugService.ShowErrorMessage(e);
                }
            }
            ;
        }

        public void Initialise()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    wasapiCapture.Initialize();
                }
                catch (CoreAudioAPIException)
                {
                    // Retry initialisation in shared mode if exclusive mode fails
                    try
                    {
                        MMDevice device = wasapiCapture.Device;
                        wasapiCapture = CreateWasapiCapture(false, device.DeviceFormat.Channels);
                        wasapiCapture.Device = device;
                        wasapiCapture.Initialize();
                    }
                    catch (Exception e)
                    {
                        DebugService.ShowErrorMessage(e);
                    }
                }
                catch (Exception e)
                {
                    DebugService.ShowErrorMessage(e);
                }
            }
        }

        public void Start()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    wasapiCapture.Start();
                }
                catch (Exception e)
                {
                    DebugService.ShowErrorMessage(e);
                }
            }
        }

        public void Stop()
        {
            if (wasapiCapture.RecordingState != RecordingState.Stopped
                && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    wasapiCapture.Stop();
                }
                catch (Exception e)
                {
                    DebugService.ShowErrorMessage(e);
                }
            }
        }
    }
}
