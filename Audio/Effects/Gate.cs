using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;
using TAC_COM.Audio.Utils;


namespace TAC_COM.Audio.Effects
{
    internal class Gate : ISampleSource
    {
        readonly ISampleSource source;
        private readonly EnveloperFollower envelopeFollower;

        private float gainLinear;
        private float gainDB;
        public float GainDB
        {
            set
            {
                gainLinear = LinearDBConverter.DecibelToLinear(value);
                gainDB = value;
            }
        }

        private float thresholdLinear;
        private float thresholdDB;
        public float ThresholdDB
        {
            set
            {
                thresholdLinear = LinearDBConverter.DecibelToLinear(value);
                thresholdDB = value;
            }
        }
        public float Attack
        {
            get => envelopeFollower.Release / TimeCoefficient;
            set
            {
                envelopeFollower.Attack = (value / 1000) * TimeCoefficient;
            }
        }
        public float Release
        {
            get => envelopeFollower.Attack / TimeCoefficient;
            set
            {
                envelopeFollower.Release = (value / 1000) * TimeCoefficient;
            }
        }

        private readonly float TimeCoefficient = 1 / (float)Math.Log(9);

        private float ratio;
        public float Ratio
        {
            get => ratio;
            set
            {
                ratio = value;
            }
        }

        private float minAmplitudeDB;
        private readonly int sampleRate;

        public Gate(ISampleSource inputSource, float minAmplitudeDB = -120)
        {
            source = inputSource;
            sampleRate = source.WaveFormat.SampleRate;
            envelopeFollower = new(sampleRate);
            this.minAmplitudeDB = minAmplitudeDB;
        }

        public float Process(float sample)
        {
            var xg = sample > 1e-6f ? LinearDBConverter.LinearToDecibel(sample) : minAmplitudeDB;
            var yg = 0f;
            yg = xg > thresholdDB ? xg : thresholdDB + (xg - thresholdDB) * Ratio;

            var envelope = envelopeFollower.Process(yg - xg);
            var sampleGain = gainLinear - envelope;
            return sample * sampleGain;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Process(buffer[i]);
            }

            return samples;
        }

        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

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

        public long Length
        {
            get { return source.Length; }
        }

        public void Dispose()
        {
        }
    }

}
