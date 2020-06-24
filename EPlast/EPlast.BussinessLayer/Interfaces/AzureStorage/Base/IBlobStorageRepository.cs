using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.AzureStorage.Base
{
    public interface IBlobStorageRepository
    {
        Task DeleteBlobAsync(string blobNameWithExtension, string containerName);
        Task UploadBlobAsync(IFormFile blobfile, string blobNameWithExtension, string containerNameKey);
        Task<CloudBlockBlob> GetBlobAsync(string blobNameWithExtension, string containerNameKey);
        Task UploadBlobForBase64Async(string base64, string fileName, string containerName);
        Task<string> GetBlobBase64Async(string blobName, string containerName);
    }
}
