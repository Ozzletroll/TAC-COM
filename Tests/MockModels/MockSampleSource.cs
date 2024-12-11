using CSCore;

namespace Tests.MockModels
{
    public class MockSampleSource : ISampleSource
    {
        public bool CanSeek => throw new NotImplementedException();

        public WaveFormat WaveFormat => new();

        public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long Length => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
