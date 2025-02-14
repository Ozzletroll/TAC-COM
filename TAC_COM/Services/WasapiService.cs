﻿using System.Threading;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for creating new <see cref="IWasapiCaptureWrapper"/>
    /// and <see cref="IWasapiOutWrapper"/>.
    /// </summary>
    public class WasapiService : IWasapiService
    {
        public IWasapiCaptureWrapper CreateWasapiCapture(CancellationToken cancellationToken)
        {
            return new WasapiCaptureWrapper(cancellationToken);
        }

        public IWasapiOutWrapper CreateWasapiOut(CancellationToken cancellationToken)
        {
            return new WasapiOutWrapper(cancellationToken);
        }
    }
}
