using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Models
{
    internal class Profile(string identifier, string fileIdentifier)
    {
        public readonly string ProfileName = identifier;
        public readonly string Filename = fileIdentifier;
    }
}
