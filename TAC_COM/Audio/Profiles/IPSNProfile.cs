﻿using System.Windows.Media.Imaging;
using NWaves.Signals.Builders;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the IPSN profile configuration.
    /// </summary>
    public class IPSNProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="IPSNProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public IPSNProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "IPS-N Integrated Tactical Network";
            FileIdentifier = "IPSN";
            Theme = UriProvider.GetThemeUri("IPSN");
            Icon = new BitmapImage(UriProvider.GetIconUri("IPSN"));
            Settings = new EffectParameters()
            {
                RingModulatorType = typeof(KarplusStrongBuilder),
                RingModulatorGainAdjust = 45f,
                RingModulatorParameters =
                {
                    { "frequency", 1900f },
                    { "stretch", 4.8f },
                    { "feedback", 5f },
                },
                PreCompressionSignalChain = new IPSNChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new IPSNChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new IPSNChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new IPSNChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.9f,
                ParallelMix = 0.1f,
                GainAdjust = -4f,
                ParallelGainAdjust = -2f,
            };
        }
    }
}
