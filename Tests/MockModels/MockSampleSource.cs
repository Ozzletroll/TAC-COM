using CSCore;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the <see cref="ISampleSource"/> stream during testing.
    /// </summary>
    public class MockSampleSource : ISampleSource
    {
        public bool CanSeek => throw new NotImplementedException();

        public WaveFormat WaveFormat => new();

        public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long Length => throw new NotImplementedException();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
