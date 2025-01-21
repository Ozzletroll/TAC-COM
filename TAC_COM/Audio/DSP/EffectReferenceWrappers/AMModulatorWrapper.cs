using System.Reflection;
using CSCore;
using NWaves.Operations;
using NWaves.Signals.Builders.Base;
using NWaves.Signals;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply AM modulation to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// Can be used as a <see cref="Models.EffectReference"/>.
    /// </remarks>
    /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class AMModulatorWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;

        /// <summary>
        /// Gets or sets the value of the "wet" processed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Wet { get; set; }

        /// <summary>
        /// Gets or sets the value of the "dry" unprocessed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Dry { get; set; }

        /// <summary>
        /// Gets or sets the frequency of the AM Modulator in Hz.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// Gets or sets the modulation index (depth) of the AM Modulator.
        /// </summary>
        public float ModulationIndex { get; set; }

        /// <summary>
        /// Method to process one sample of the buffer with
        /// one sample of the ring modulated stream, mixing them
        /// according to the <see cref="Wet"/> and <see cref="Dry"/>
        /// values.
        /// </summary>
        /// <param name="bufferSample"> The sample from the buffer to process.</param>
        /// <param name="modulatorSample"> The sample from the AM modulator to process.</param>
        /// <returns></returns>
        private float Process(float bufferSample, float modulatorSample)
        {
            return (Wet * modulatorSample) + (Dry * bufferSample);
        }

        /// <inheritdoc/>
        /// <remarks> 
        /// This is where the effect is applied to all
        /// samples in the buffer.
        /// </remarks>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            DiscreteSignal carrierSignal = new(source.WaveFormat.SampleRate, buffer);

            var amModulator = Modulator.Amplitude(carrierSignal, Frequency, ModulationIndex);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Process(buffer[i], amModulator.Samples[i]);
            }
            return samples;
        }

        /// <inheritdoc/>
        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        /// <inheritdoc/>
        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        /// <inheritdoc/>
        public long Position
        {
            get
            {
                return source.Position;
            }
            set
            {
                source.Position = value;
            }
        }

        /// <inheritdoc/>
        public long Length
        {
            get { return source.Length; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            source?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
