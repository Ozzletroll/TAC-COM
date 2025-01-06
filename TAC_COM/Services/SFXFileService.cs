using System.IO;
using CSCore;
using CSCore.Codecs;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for loading and returning sfx files
    /// as <see cref="IWaveSource"/>'s, ready for playback.
    /// </summary>
    /// <param name="filepath"></param>
    public class SFXFileService(string filepath) : ISFXFileService
    {
        private readonly string sfxFilePath = filepath;

        public IWaveSource GetOpenSFX(string fileSuffix)
        {
            var sfxName = "GateOpen" + fileSuffix;
            var filename = GetFile(sfxFilePath + "/GateOpen", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetCloseSFX(string fileSuffix)
        {
            var sfxName = "GateClose" + fileSuffix;
            var filename = GetFile(sfxFilePath + "/GateClose", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetNoiseSFX(string fileSuffix)
        {
            var sfxName = "Noise" + fileSuffix;
            var filename = GetFile(sfxFilePath + "/Noise", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        /// <summary>
        /// Static method to retrieve the path of a file within a specified folder.
        /// </summary>
        /// <param name="folder"> The name of the folder to search within.</param>
        /// <param name="filename">The name of the file (without extension) to search for.</param>
        /// <returns>The full path of the matching file if found; otherwise, <c>null</c></returns>
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
