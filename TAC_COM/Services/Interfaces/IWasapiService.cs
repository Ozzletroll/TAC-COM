using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services.Interfaces
{
    public interface IWasapiService
    {
        IWasapiCaptureWrapper CreateWasapiCapture();
        IWasapiOutWrapper CreateWasapiOut();
    }
}