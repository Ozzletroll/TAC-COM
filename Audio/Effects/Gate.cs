using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;

namespace TAC_COM.Audio.Effects
{
    internal class Gate : ISampleSource
    {
        readonly ISampleSource source;
        readonly PeakMeter peakMeter;
        public float GainReductionDB { get; set; }
        public float ThresholdDB { get; set; }

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
            peakMeter = new PeakMeter(source);
            peakMeter.Interval = 5;
            peakMeter.PeakCalculated += OnPeakCalculated;
        }

        private void OnPeakCalculated(object? sender, PeakEventArgs e)
        {
            Console.WriteLine(peakMeter.PeakValue);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = peakMeter.Read(buffer, offset, count);
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
