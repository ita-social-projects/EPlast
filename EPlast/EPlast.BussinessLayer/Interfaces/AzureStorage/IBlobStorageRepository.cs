using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.AzureStorage
{
    public interface IBlobStorageRepository
    {
        // Task<bool> DownloadBlob(string file, string fileExtension);
        Task<bool> DeleteBlobAsync(string blobNameWithExtension, string containerName);
        //Task<IEnumerable<BlobViewModel>> GetBlobsAsync();
        Task<bool> UploadBlobAsync(IFormFile blobfile, string blobNameWithExtension, string containerNameKey);
        Task<CloudBlockBlob> GetBlobAsync(string blobNameWithExtension, string containerNameKey);
    }
}
