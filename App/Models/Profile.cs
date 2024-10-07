﻿using App.Audio.DSP.NWaves;
using CSCore;
using CSCore.Streams.Effects;
using System.Reflection;
using System.Windows.Media.Imaging;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Models
{
    public class Profile(string profileName, string fileIdentifier, Uri theme, System.Windows.Media.ImageSource icon)
    {
        public string ProfileName = profileName;
        public string FileIdentifier = fileIdentifier;
        public Uri Theme = theme;
        public IWaveSource? NoiseSource;
        public IWaveSource? OpenSFX;
        public IWaveSource? CloseSFX;
        private readonly FilePlayer FilePlayer = new();
        public AudioSettings Settings = new();
        public System.Windows.Media.ImageSource Icon = icon;

        public void LoadSources()
        {
            if (FileIdentifier != null)
            {
                NoiseSource = FilePlayer.GetNoiseSFX(FileIdentifier);
                OpenSFX = FilePlayer.GetOpenSFX(FileIdentifier);
                CloseSFX = FilePlayer.GetCloseSFX(FileIdentifier);
            }
        }

        public override string ToString()
        {
            return ProfileName ?? string.Empty;
        }
    }

    public class AudioSettings
    {
        public bool ChorusEnabled = false;
        public float PitchShiftFactor = 1f;
        public Type DistortionType = typeof(DmoDistortionEffect);
        public List<EffectReference>? SignalChain;
    }
}
