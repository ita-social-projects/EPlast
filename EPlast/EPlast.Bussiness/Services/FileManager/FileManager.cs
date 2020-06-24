using System.IO;

namespace EPlast.BusinessLogicLayer
{
    public class FileManager : IFileManager
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}