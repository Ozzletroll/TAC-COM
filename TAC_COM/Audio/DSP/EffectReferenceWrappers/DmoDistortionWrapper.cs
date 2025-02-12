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

        public float Gain
        {
            get => distortionEffect.Gain;
            set
            {
                distortionEffect.Gain = value;
            }
        }

        public float OffsetGain
        {
            get => gain.GainDB;
            set
            {
                gain.GainDB = value;
            }
        }

        public float Edge
        {
            get => distortionEffect.Edge;
            set
            {
                distortionEffect.Edge = value;
            }
        }

        public float PostEQCenterFrequency
        {
            get => distortionEffect.PostEQCenterFrequency;
            set
            {
                distortionEffect.PostEQCenterFrequency = value;
            }
        }

        public float PostEQBandwidth
        {
            get => distortionEffect.PostEQBandwidth;
            set
            {
                distortionEffect.PostEQBandwidth = value;
            }
        }

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
