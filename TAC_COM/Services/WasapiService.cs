using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    public class WasapiService : IWasapiService
    {
        public IWasapiCaptureWrapper CreateWasapiCapture()
        {
            return new WasapiCaptureWrapper();
        }

        public IWasapiOutWrapper CreateWasapiOut()
        {
            return new WasapiOutWrapper();
        }
    }
}
