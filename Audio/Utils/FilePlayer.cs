﻿using CSCore;
using CSCore.Codecs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;

namespace TAC_COM.Audio.Utils
{
    internal class FilePlayer
    {
        private readonly FileManager fileManager = new(Directory.GetCurrentDirectory());

        public IWaveSource GetOpenSFX(string fileSuffix)
        {
            var sfxName = "GateOpen" + fileSuffix;
            var filename = fileManager.GetFile("Static/SFX/GateOpen", sfxName);
            return CodecFactory.Instance.GetCodec(filename).ToMono();
        }

        public IWaveSource GetCloseSFX(string profile)
        {
            var sfxName = "GateClose" + profile;
            var filename = fileManager.GetFile("Static/SFX/GateClose", sfxName);
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
