using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.WebRequestMethods;

namespace TAC_COM.Audio.Utils
{
    internal class FileManager(string directoryName)
    {
        private readonly string baseDirectory = directoryName;

        public string GetRandomFile(string folder)
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

        public string? GetFile(string folder, string filename)
        {
            string subfolderPath = Path.Combine(baseDirectory, folder);

            if (!Directory.Exists(subfolderPath))
            {
                return null;
            }
            var files = Directory.GetFiles(subfolderPath);
            var matchingFile = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == filename);

            return matchingFile;
        }
    }
}
