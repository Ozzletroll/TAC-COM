﻿using System.Windows.Media.Imaging;
using NWaves.Effects;
using NWaves.Signals.Builders;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the SSC profile configuration.
    /// </summary>
    public class SSCProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="SSCProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public SSCProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "SSC Hamadryas Stealth Tranceiver";
            FileIdentifier = "SSC";
            Theme = UriProvider.GetThemeUri("SSC");
            Icon = new BitmapImage(UriProvider.GetIconUri("GMS"));
            Settings = new EffectParameters()
            {
                DistortionType = typeof(DistortionWrapper),
                DistortionMode = DistortionMode.SoftClipping,
                DistortionInput = 16,
                DistortionOutput = 24,
                DistortionWet = 0.2f,
                DistortionDry = 0.8f,
                RingModulatorType = typeof(SquareWaveBuilder),
                RingModulatorParameters =
                {
                    { "frequency", 220f },
                },
                PreDistortionSignalChain = new SSCChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new SSCChain().GetPostDistortionEffects(),
                HighpassFrequency = 600,
                LowpassFrequency = 2400,
                GainAdjust = -3,
            };
        }
    }
}
