using System.Windows.Media;
using CSCore;

namespace TAC_COM.Models.Interfaces
{
    public interface IProfile
    {
        IWaveSource? CloseSFX { get; set; }
        string FileIdentifier { get; set; }
        ImageSource Icon { get; set; }
        IWaveSource? NoiseSource { get; set; }
        IWaveSource? OpenSFX { get; set; }
        string ProfileName { get; set; }
        EffectParameters Settings { get; set; }
        Uri Theme { get; set; }

        void LoadSources();
        string ToString();
    }
}