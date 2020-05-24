using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IFileStreamManager
    {
        Stream GetStream();

        Task CopyToAsync(MemoryStream memory);

        Task CopyToAsync(IFormFile from, Stream memoryTo);

        Task CopyToAsync(Stream from, Stream memoryTo);

        FileStreamManager GenerateFileStreamManager(string path, FileMode mode);
    }
}