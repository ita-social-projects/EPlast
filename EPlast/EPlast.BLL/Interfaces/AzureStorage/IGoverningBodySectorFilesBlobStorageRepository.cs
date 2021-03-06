﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;

namespace EPlast.BLL.Interfaces.AzureStorage
{
    public interface IGoverningBodySectorFilesBlobStorageRepository
    {
        Task<CloudBlockBlob> GetBlobAsync(string blobName);

        Task DeleteBlobAsync(string blobName);

        Task UploadBlobAsync(IFormFile blobFile, string fileName);

        Task UploadBlobForBase64Async(string base64, string fileName);

        Task<string> GetBlobBase64Async(string blobName);
    }
}