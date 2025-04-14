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
    /// <param name="inputSource"></param>
    public class NoiseSuppressor(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource.ToWaveSource(16).ToSampleSource().ToMono();
        private readonly Denoiser denoiser = new();

        /// <inheritdoc/>
        /// <remarks>
        /// This is where the noise suppression is applied to all samples in
        /// the buffer.
        /// </remarks>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            int frameSize = 480;
            int end = offset + samples;

            // Process the buffer in chunks of 480 samples
            for (int i = offset; i + frameSize <= end; i += frameSize)
            {
                denoiser.Denoise(buffer.AsSpan(i, frameSize), false);
            }

            // Handle any remaining samples (excess samples less than a frame size)
            int remainingSamples = end % frameSize;
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
            GC.SuppressFinalize(this);
        }
    }
}
