using System.Windows.Media;
using CSCore;

namespace TAC_COM.Models.Interfaces
{
    public interface IProfile
    {
        IFileSourceWrapper? CloseSFXSource { get; set; }
        string FileIdentifier { get; set; }
        ImageSource Icon { get; set; }
        IFileSourceWrapper? NoiseSource { get; set; }
        IFileSourceWrapper? OpenSFXSource { get; set; }
        string ProfileName { get; set; }
        EffectParameters Settings { get; set; }
        Uri Theme { get; set; }

        void LoadSources();
        string ToString();
    }
}