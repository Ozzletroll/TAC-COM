using CSCore;

namespace Tests.MockModels
{
    internal class MockWaveSource : IWaveSource
    {
        public bool CanSeek => throw new NotImplementedException();

        public WaveFormat WaveFormat => new();

        public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long Length => throw new NotImplementedException();

        public void Dispose() { }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
