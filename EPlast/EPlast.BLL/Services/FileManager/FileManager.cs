using System.IO;

namespace EPlast.BLL
{
    public class FileManager : IFileManager
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}