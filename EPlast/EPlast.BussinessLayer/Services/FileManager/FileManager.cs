using System.IO;

namespace EPlast.BussinessLayer
{
    public class FileManager : IFileManager
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}