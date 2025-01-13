using System.Reflection;
using CSCore;
using NWaves.Operations;
using NWaves.Signals;
using NWaves.Signals.Builders;
using NWaves.Signals.Builders.Base;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    public class RingModulatorWrapper(ISampleSource inputSource) : ISampleSource
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
        /// Gets or sets the frequency of the modulator signal
        /// in Hz.
        /// </summary>
        public float Frequency { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the Type of the <see cref="SignalBuilder"/>
        /// to use for ring modulation.
        /// </summary>
        public Type? ModulatorSignalType {  get; set; }

        private SignalBuilder CreateSignalBuilder()
        {
            ConstructorInfo? constructor = ModulatorSignalType?.GetConstructors()
                .FirstOrDefault();

            if (constructor?.Invoke([]) is SignalBuilder instance) return instance;
            else throw new InvalidOperationException("Effect failed to instantiate.");
        }

        /// <summary>
        /// Method to process one sample of the buffer with
        /// one sample of the ring modulated stream, mixing them
        /// according to the <see cref="Wet"/> and <see cref="Dry"/>
        /// values.
        /// </summary>
        /// <param name="bufferSample"> The sample from the buffer to process.</param>
        /// <param name="modulatorSample"> The sample from the ring modulator to process.</param>
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

            DiscreteSignal modulatorSignal = CreateSignalBuilder()
                .SetParameter("frequency", Frequency)
                .SetParameter("phase", Math.PI / 6)
                .OfLength(buffer.Length)
                .SampledAt(source.WaveFormat.SampleRate)
                .Build();

            var ringModulator = Modulator.Ring(carrierSignal, modulatorSignal);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Process(buffer[i], ringModulator.Samples[i]);
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
