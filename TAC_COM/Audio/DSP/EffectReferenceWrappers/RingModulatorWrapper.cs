using System.Reflection;
using CSCore;
using NWaves.Operations;
using NWaves.Signals;
using NWaves.Signals.Builders.Base;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply ring modulation to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// Can be used as a <see cref="Models.EffectReference"/>.
    /// </remarks>
    /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class RingModulatorWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;

        private float modulatedSignalAdjustmentLinear = 1;

        /// <summary>
        /// Gets or sets the gain adjustment of the carrier signal 
        /// in decibels.
        /// </summary>
        public float ModulatedSignalAdjustmentDB
        {
            get => LinearDBConverter.LinearToDecibel(modulatedSignalAdjustmentLinear);
            set
            {
                modulatedSignalAdjustmentLinear = LinearDBConverter.DecibelToLinear(value);
            }
        }

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
        /// Gets or sets the Type of the <see cref="SignalBuilder"/>
        /// to use for ring modulation.
        /// </summary>
        public Type? ModulatorSignalType {  get; set; }

        /// <summary>
        /// Gets or sets the parameters for the modulator signal.
        /// </summary>
        public Dictionary<string, object> ModulatorParameters { get; set; } = [];

        private SignalBuilder CreateSignalBuilder()
        {
            ConstructorInfo? constructor = ModulatorSignalType?.GetConstructors()
                .FirstOrDefault();

            if (constructor?.Invoke([]) is SignalBuilder instance) return instance;
            else throw new InvalidOperationException("Effect failed to instantiate.");
        }

        /// <summary>
        /// Static method to set the parameters of a <see cref="SignalBuilder"/>.
        /// </summary>
        /// <param name="builder"> The object to apply the parameters to.</param>
        /// <param name="parameters"> The dictionary of parameters and values to apply.</param>
        static void SetParameters(dynamic builder, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                object[] parameterObject = [parameter.Key, parameter.Value];

                MethodInfo setParameterMethod = builder.GetType().GetMethod("SetParameter");
                setParameterMethod.Invoke(builder, parameterObject);
            }
        }

        /// <summary>
        /// Method to process one sample of the buffer with
        /// one sample of the ring modulated stream, mixing them
        /// according to the <see cref="Wet"/> and <see cref="Dry"/>
        /// values, and adjusting the wet signal gain according to
        /// the <see cref="ModulatedSignalAdjustmentDB"/> parameter.
        /// </summary>
        /// <param name="bufferSample"> The sample from the buffer to process.</param>
        /// <param name="modulatorSample"> The sample from the ring modulator to process.</param>
        /// <returns></returns>
        private float Process(float bufferSample, float modulatorSample)
        {
            var gainAdjustModulatorSample = Math.Max(Math.Min(modulatorSample * modulatedSignalAdjustmentLinear, 1), -1);
            return (Wet * gainAdjustModulatorSample) + (Dry * bufferSample);
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

            var signalBuilder = CreateSignalBuilder();
            SetParameters(signalBuilder, ModulatorParameters);

            DiscreteSignal modulatorSignal = signalBuilder
                .OfLength(samples)
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
