using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EPlast.Bussiness
{
    public class FileStreamManager : IFileStreamManager, IDisposable
    {
        private readonly FileStream fileStream;

        public FileStreamManager()
        {
        }

        public FileStreamManager(string path, FileMode mode)
        {
            fileStream = new FileStream(path, mode);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            fileStream?.Dispose();
        }

        public Stream GetStream()
        {
            return fileStream;
        }

        public FileStreamManager GenerateFileStreamManager(string path, FileMode mode)
        {
            return new FileStreamManager(path, mode);
        }

        public async Task CopyToAsync(MemoryStream memory)
        {
            await fileStream.CopyToAsync(memory);
        }

        public async Task CopyToAsync(IFormFile from, Stream memoryTo)
        {
            await from.CopyToAsync(memoryTo);
        }

        public async Task CopyToAsync(Stream from, Stream memoryTo)
        {
            await from.CopyToAsync(memoryTo);
        }
    }
}