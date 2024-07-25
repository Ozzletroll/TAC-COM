using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Audio.Utils
{
    internal class FileManager
    {
        private readonly string baseDirectory;

        public FileManager(string directoryName)
        {
            baseDirectory = directoryName;
        }

        public string? GetRandomFile(string folder)
        {
            string subfolderPath = Path.Combine(baseDirectory, folder);

            if (!Directory.Exists(subfolderPath))
            {
                return null;
            }

            string[] files = Directory.GetFiles(subfolderPath);

            if (files.Length == 0)
            {
                return null;
            }
            else
            {
                Random random = new Random();
                int randomIndex = random.Next(0, files.Length);

                return files[randomIndex];
            }
        }
    }
}
