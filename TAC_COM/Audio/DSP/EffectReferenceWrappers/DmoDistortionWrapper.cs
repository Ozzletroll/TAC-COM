using CSCore.Streams.Effects;
using CSCore;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    public class DmoDistortionWrapper : ISampleSource
    {
        private readonly IWaveSource source;
        private readonly ISampleSource sampleSource;
        private readonly DmoDistortionEffect distortionEffect;
        private readonly Gain gain;

        /// <summary>
        /// Initialises a new instance of the <see cref="DmoDistortionEffect"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="ISampleSource"/> input is converted to a
        /// <see cref="IWaveSource"/>, then the effect is applied before
        /// converting back to a <see cref="ISampleSource"/>.
        /// </remarks>
        /// <param name="inputSource">The <see cref="ISampleSource"/> to apply the effect to.</param>
        public DmoDistortionWrapper(ISampleSource inputSource)
        {
            source = inputSource.ToWaveSource();
            distortionEffect = new DmoDistortionEffect(source)
            {
                Gain = -60,
                Edge = 75,
                PostEQCenterFrequency = 3500,
                PostEQBandwidth = 4800,
                PreLowpassCutoff = 8000
            };

            sampleSource = distortionEffect.ToSampleSource().AppendSource(x => new Gain(x)
            {
                GainDB = -45,
            }, out gain);
        }

        /// <summary>
        /// Gets or sets the value representing the gain after distortion in dB,
        /// ranging from -60 to 0 dB.
        /// </summary>
        public float Gain
        {
            get => distortionEffect.Gain;
            set
            {
                distortionEffect.Gain = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the additional gain offset after distortion in dB.
        /// </summary>
        public float OffsetGain
        {
            get => gain.GainDB;
            set
            {
                gain.GainDB = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the percentage of distortion intensity,
        /// ranges from 0 to 100.
        /// </summary>
        public float Edge
        {
            get => distortionEffect.Edge;
            set
            {
                distortionEffect.Edge = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the center frequency of harmonic content addition
        /// in Hz.
        /// </summary>
        public float PostEQCenterFrequency
        {
            get => distortionEffect.PostEQCenterFrequency;
            set
            {
                distortionEffect.PostEQCenterFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the bandwidth of harmonic content addition.
        /// </summary>
        public float PostEQBandwidth
        {
            get => distortionEffect.PostEQBandwidth;
            set
            {
                distortionEffect.PostEQBandwidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the cutoff filter for high-frequency harmonics
        /// attenuation in Hz.
        /// </summary>
        public float PreLowpassCutoff
        {
            get => distortionEffect.PreLowpassCutoff;
            set
            {
                distortionEffect.PreLowpassCutoff = value;
            }
        }

        /// <inheritdoc/>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = sampleSource.Read(buffer, offset, count);
            return samples;
        }

        /// <inheritdoc/>
        public bool CanSeek
        {
            get { return sampleSource.CanSeek; }
        }

        /// <inheritdoc/>
        public WaveFormat WaveFormat
        {
            get { return sampleSource.WaveFormat; }
        }

        /// <inheritdoc/>
        public long Position
        {
            get
            {
                return sampleSource.Position;
            }
            set
            {
                sampleSource.Position = value;
            }
        }

        /// <inheritdoc/>
        public long Length
        {
            get { return sampleSource.Length; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            source?.Dispose();
            sampleSource?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
