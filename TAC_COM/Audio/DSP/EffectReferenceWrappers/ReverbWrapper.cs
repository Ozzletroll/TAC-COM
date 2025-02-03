using CSCore;
using CSCore.Streams.Effects;


namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply a <see cref="DmoWavesReverbEffect"/> to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    public class ReverbWrapper : ISampleSource
    {
        private readonly IWaveSource source;
        private readonly ISampleSource sampleSource;
        private readonly DmoWavesReverbEffect reverbEffect;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReverbWrapper"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="ISampleSource"/> input is converted to a
        /// <see cref="IWaveSource"/>, then the effect is applied before
        /// converting back to a <see cref="ISampleSource"/>.
        /// </remarks>
        /// <param name="inputSource"></param>
        public ReverbWrapper(ISampleSource inputSource)
        {
            source = inputSource.ToWaveSource();
            reverbEffect = new DmoWavesReverbEffect(source)
            {
                ReverbTime = 300f,
                ReverbMix = -10f,
            };
            sampleSource = reverbEffect.ToSampleSource();
        }

        /// <summary>
        /// Gets or sets the value representing the length
        /// of the reverb tail in milliseconds, in a range 
        /// from 0.001 to 3000.
        /// </summary>
        public float ReverbTime
        {
            get => reverbEffect.ReverbTime;
            set
            {
                reverbEffect.ReverbTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the mix in dB,
        /// in a range from -96 to 0.
        /// </summary>
        public float ReverbMix
        {
            get => reverbEffect.ReverbMix;
            set
            {
                reverbEffect.ReverbMix = value;
            }
        }

        /// <inheritdoc/>
        /// <remarks> 
        /// This is where the effect is applied to all
        /// samples in the buffer.
        /// </remarks>
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
