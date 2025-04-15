using CSCore;
using RNNoise.NET;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Noise suppressor utilising <see cref="RNNoise"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ISampleSource"/> that this is applied to will be
    /// converted to 16bit PCM mono.
    /// </remarks>
    /// <param name="inputSource"> The <see cref="ISampleSource"/> to apply noise suppression to.</param>
    public class NoiseSuppressor(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource.ToWaveSource(16).ToSampleSource().ToMono();
        private readonly Denoiser denoiser = new();
        private const int FrameSize = 480;

        /// <inheritdoc/>
        /// <remarks>
        /// This is where the noise suppression is applied to all samples in
        /// the buffer.
        /// </remarks>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            int end = offset + samples;

            for (int i = offset; i + FrameSize <= end; i += FrameSize)
            {
                denoiser.Denoise(buffer.AsSpan(i, FrameSize), false);
            }

            int remainingSamples = end % FrameSize;
            if (remainingSamples > 0)
            {
                int startOfRemainder = end - remainingSamples;
                denoiser.Denoise(buffer.AsSpan(startOfRemainder, remainingSamples), false);
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
            denoiser?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
