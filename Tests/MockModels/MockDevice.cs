using CSCore.CoreAudioAPI;
using CSCore.Win32;
using Moq;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the audio device during testing.
    /// </summary>
    /// <param name="_friendlyName"></param>
    public class MockDevice(string _friendlyName) : IMMDevice
    {
        private readonly string friendlyName = _friendlyName;

        public override string ToString()
        {
            return friendlyName;
        }

        public int Activate(Guid iid, CLSCTX clsctx, nint activationParams, out nint pinterface)
        {
            throw new NotImplementedException();
        }

        public int OpenPropertyStore(StorageAccess access, out nint propertystore)
        {
            throw new NotImplementedException();
        }

        public int GetId(out string deviceId)
        {
            throw new NotImplementedException();
        }

        public int GetState(out DeviceState state)
        {
            throw new NotImplementedException();
        }

        public int QueryInterface(ref Guid riid, out nint ppvObject)
        {
            throw new NotImplementedException();
        }

        public int AddRef()
        {
            throw new NotImplementedException();
        }

        public int Release()
        {
            throw new NotImplementedException();
        }

        public static implicit operator MMDevice(MockDevice v)
        {
            nint ptr = 1;

            var mock = new Mock<MMDevice>(MockBehavior.Loose, [ptr]);
            mock.Setup(device => device.ToString()).Returns(v.friendlyName);

            return mock.Object;
        }
    }
}
