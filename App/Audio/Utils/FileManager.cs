using System.IO;

namespace App.Audio.Utils
{
    internal class FileManager(string directoryName)
    {
        private readonly string baseDirectory = directoryName;

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
