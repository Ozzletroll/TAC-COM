using CSCore;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Adjustable noise gate for use with an <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Algorithm based on:
    /// </para>
    /// <para>
    /// Hämäläinen, P. (2002). "Smoothing of the Control Signal Without Clipped Output in Digital Peak Limiters."
    /// </para>
    /// </remarks>
    public class Gate : ISampleSource
    {
        readonly ISampleSource source;
        private readonly int sampleRate;
        private float envelope;
        private float gateControl;
        private readonly float[] gainArray = new float[2];
        private int attackCounter;
        private int releaseCounter;

        private float thresholdLinear;
        private float thresholdDB;

        /// <summary>
        /// Gets or sets the threshold level at which attenuation begins
        /// in decibels.
        /// </summary>
        public float ThresholdDB
        {
            get => thresholdDB;
            set
            {
                thresholdDB = value;
                thresholdLinear = LinearDBConverter.DecibelToLinear(value);
            }
        }

        private float attack;
        private float attackCoefficient;

        /// <summary>
        /// Gets or sets the attack value and coefficient in
        /// milliseconds.
        /// </summary>
        public float Attack
        {
            get => attack;
            set
            {
                attack = value / 1000;
                attackCoefficient = (float)Math.Exp(-Math.Log(9) / (sampleRate * attack));
            }
        }

        private float hold;
        private float holdTime;

        /// <summary>
        /// Gets or sets the hold value and time
        /// in milliseconds.
        /// </summary>
        public float Hold
        {
            get => hold;
            set
            {
                hold = value / 1000;
                holdTime = hold * sampleRate;
            }
        }

        private float release;
        private float releaseCoefficient;

        /// <summary>
        /// Gets or sets the release value and coeffecient
        /// in milliseconds.
        /// </summary>
        public float Release
        {
            get => release;
            set
            {
                release = value / 1000;
                releaseCoefficient = (float)Math.Exp(-Math.Log(9) / (sampleRate * release));
            }
        }

        private float ratio;

        /// <summary>
        /// Gets or sets the ratio of the noise
        /// gate attenuation.
        /// </summary>
        public float Ratio
        {
            get => ratio;
            set
            {
                ratio = value;
            }
        }

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
            sampleRate = source.WaveFormat.SampleRate;
        }

        /// <summary>
        /// Processes a single sample and applies
        /// noise gate attenuation.
        /// </summary>
        /// <param name="sample"> The sample to be processed.</param>
        /// <returns> The processed sample.</returns>
        public float Process(float sample)
        {
            var absSample = Math.Abs(sample);

            envelope = releaseCoefficient * envelope + (1 - attackCoefficient)
                * ((absSample - envelope > 0) ? absSample - envelope : 0);

            if (envelope < thresholdLinear)
            {
                gateControl = 0;
            }
            else
            {
                gateControl = 1;
            }

            if (gateControl < gainArray[0])
            {
                // Attack mode
                releaseCounter = 0;
                if (++attackCounter < holdTime)
                {
                    // Hold mode
                    gainArray[0] = gainArray[1];
                }
                else
                {
                    gainArray[0] = attackCoefficient * gainArray[1] + (1 - attackCoefficient) * gateControl;
                }
                gainArray[1] = gainArray[0];
            }
            else
            {
                // Release mode
                attackCounter = 0;
                if (++releaseCounter < holdTime)
                {
                    // Hold mode
                    gainArray[0] = gainArray[1];
                }
                else
                {
                    gainArray[0] = releaseCoefficient * gainArray[1] + (1 - releaseCoefficient) * gateControl;
                }
                gainArray[1] = gainArray[0];
            }

            // Apply gain
            return gainArray[0] * sample;
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Read method,
        /// in which the noise gate is applied to the sample buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Process(buffer[i]);
            }

            return samples;
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> CanSeek
        /// property.
        /// </summary>
        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> WaveFormat
        /// property.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Position
        /// property.
        /// </summary>
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

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Length
        /// property.
        /// </summary>
        public long Length
        {
            get { return source.Length; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Dispose
        /// method.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}
