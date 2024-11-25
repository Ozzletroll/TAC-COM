﻿using CSCore;
using TAC_COM.Audio.Utils;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class Profile(string profileName, string fileIdentifier, Uri theme, System.Windows.Media.ImageSource icon) : IProfile
    {
        private readonly FilePlayer FilePlayer = new();

        private string profileName = profileName;
        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
            }
        }

        private string fileIdentifier = fileIdentifier;
        public string FileIdentifier
        {
            get => fileIdentifier;
            set
            {
                fileIdentifier = value;
            }
        }

        private Uri theme = theme;
        public Uri Theme
        {
            get => theme;
            set
            {
                theme = value;
            }
        }

        private System.Windows.Media.ImageSource icon = icon;
        public System.Windows.Media.ImageSource Icon
        {
            get => icon;
            set
            {
                icon = value;
            }
        }

        private EffectParameters settings = new();
        public EffectParameters Settings
        {
            get => settings;
            set
            {
                settings = value;
            }
        }

        private IFileSourceWrapper? noiseSource;
        public IFileSourceWrapper? NoiseSource
        {
            get => noiseSource;
            set
            {
                noiseSource = value;
            }
        }

        private IFileSourceWrapper? openSFXSource;
        public IFileSourceWrapper? OpenSFXSource
        {
            get => openSFXSource;
            set
            {
                openSFXSource = value;
            }
        }

        private IFileSourceWrapper? closeSFXSource;
        public IFileSourceWrapper? CloseSFXSource
        {
            get => closeSFXSource;
            set
            {
                closeSFXSource = value;
            }
        }

        public void LoadSources()
        {
            if (FileIdentifier != null)
            {
                NoiseSource = new FileSourceWrapper
                {
                    WaveSource = FilePlayer.GetNoiseSFX(FileIdentifier)
                };
                OpenSFXSource = new FileSourceWrapper
                {
                    WaveSource = FilePlayer.GetOpenSFX(FileIdentifier)
                }; 
                CloseSFXSource = new FileSourceWrapper
                {
                    WaveSource = FilePlayer.GetCloseSFX(FileIdentifier)
                }; 
            }
        }

        public override string ToString()
        {
            return ProfileName ?? string.Empty;
        }
    }
}
