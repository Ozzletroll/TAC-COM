using System.IO;
using CSCore;
using CSCore.Codecs;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    public class SFXFileService(string filepath) : ISFXFileService
    {
        private readonly string sfxFilePath = filepath;

        public IWaveSource GetOpenSFX(string fileSuffix)
        {
            var sfxName = "GateOpen" + fileSuffix;
            var filename = GetFile(sfxFilePath + "/GateOpen", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetCloseSFX(string profile)
        {
            var sfxName = "GateClose" + profile;
            var filename = GetFile(sfxFilePath + "/GateClose", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetNoiseSFX(string profile)
        {
            var sfxName = "Noise" + profile;
            var filename = GetFile(sfxFilePath + "/Noise", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        private static string? GetFile(string folder, string filename)
        {
            string subfolderPath = Path.Combine(Directory.GetCurrentDirectory(), folder);

            if (!Directory.Exists(subfolderPath))
            {
                return null;
            }
            var files = Directory.GetFiles(subfolderPath);
            var matchingFile = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == filename);

            return matchingFile;
        }
    }
}
