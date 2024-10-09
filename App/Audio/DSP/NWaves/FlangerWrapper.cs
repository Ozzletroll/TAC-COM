using CSCore;
using NWaves.Effects;
using NWaves.Signals.Builders.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Audio.DSP.NWaves
{
    internal class FlangerWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource Source = inputSource;
        private readonly FlangerEffect Flanger = new(inputSource.WaveFormat.SampleRate)
        {
            Wet = 0.5f,
            Dry = 0.5f,
        };

        public float Wet
        {
            get => Flanger.Wet;
            set
            {
                Flanger.Wet = value;
            }
        }

        public float Dry
        {
            get => Flanger.Dry;
            set
            {
                Flanger.Dry = value;
            }
        }

        public float LFOFrequency
        {
            get => Flanger.LfoFrequency;
            set
            {
                Flanger.LfoFrequency = value;
            }
        }

        public float Width
        {
            get => Flanger.Width;
            set
            {
                Flanger.Width = value;
            }
        }

        public float Depth
        {
            get => Flanger.Depth;
            set
            {
                Flanger.Depth = value;
            }
        }

        public float Feedback
        {
            get => Flanger.Feedback;
            set
            {
                Flanger.Feedback = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Flanger.Process(buffer[i]);
            }
            return samples;
        }

        public bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        public WaveFormat WaveFormat
        {
            get { return Source.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return Source.Position;
            }
            set
            {
                Source.Position = value;
            }
        }

        public long Length
        {
            get { return Source.Length; }
        }

        public void Dispose()
        {
        }
    }
}
