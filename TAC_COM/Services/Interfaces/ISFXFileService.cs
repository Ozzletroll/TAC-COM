using CSCore;

namespace TAC_COM.Services.Interfaces
{
    public interface ISFXFileService
    {
        IWaveSource GetCloseSFX(string profile);
        IWaveSource GetNoiseSFX(string profile);
        IWaveSource GetOpenSFX(string fileSuffix);
    }
}