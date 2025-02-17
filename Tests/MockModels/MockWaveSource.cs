using CSCore;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the <see cref="IWaveSource"/> stream during testing.
    /// </summary>
    public class MockWaveSource : IWaveSource
    {
        public bool CanSeek => throw new NotImplementedException();

        public WaveFormat WaveFormat => new();

        public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long Length => throw new NotImplementedException();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int Read(byte[] buffer, int offset, int count) => count;
    }
}
