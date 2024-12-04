using System.IO;
using CSCore;
using CSCore.Codecs;

namespace TAC_COM.Services
{
    public class SFXFileService
    {
        public static IWaveSource GetOpenSFX(string fileSuffix)
        {
            var sfxName = "GateOpen" + fileSuffix;
            var filename = GetFile("Static/SFX/GateOpen", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public static IWaveSource GetCloseSFX(string profile)
        {
            var sfxName = "GateClose" + profile;
            var filename = GetFile("Static/SFX/GateClose", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public static IWaveSource GetNoiseSFX(string profile)
        {
            var sfxName = "Noise" + profile;
            var filename = GetFile("Static/SFX/Noise", sfxName);
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
