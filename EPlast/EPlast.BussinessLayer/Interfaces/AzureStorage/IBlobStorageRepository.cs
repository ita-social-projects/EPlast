using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.AzureStorage
{
    public interface IBlobStorageRepository
    {
        Task<bool> DeleteBlobAsync(string blobNameWithExtension, string containerName);
        Task<bool> UploadBlobAsync(IFormFile blobfile, string blobNameWithExtension, string containerNameKey);
        Task<CloudBlockBlob> GetBlobAsync(string blobNameWithExtension, string containerNameKey);
    }
}
