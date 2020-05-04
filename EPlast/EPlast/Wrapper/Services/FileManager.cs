using System.IO;

namespace EPlast.Wrapper
{
    public class FileManager : IFileManager
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}