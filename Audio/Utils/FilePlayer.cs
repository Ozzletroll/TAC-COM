using CSCore;
using CSCore.Codecs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Audio.Utils
{
    internal class FilePlayer
    {
        private readonly FileManager fileManager = new(Directory.GetCurrentDirectory());

        public IWaveSource GetOpenSFX()
        {
            var filename = fileManager.GetRandomFile("Static/SFX/GateOpen");
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetCloseSFX()
        {
            var filename = fileManager.GetRandomFile("Static/SFX/GateClose");
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetNoiseSFX(string profile)
        {
            var sfxName = "Noise" + profile;
            var filename = fileManager.GetFile("Static/SFX/Noise", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }
    }
}
