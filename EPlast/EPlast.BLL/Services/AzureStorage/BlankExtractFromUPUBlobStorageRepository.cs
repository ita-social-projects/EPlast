
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AzureStorage
{
   public class BlankExtractFromUPUBlobStorageRepository:BlobStorageRepository, IBlankExtractFromUPUBlobStorageRepository
    {
        private const string CONTAINER = "ExtractFromUPUContainer";
        public BlankExtractFromUPUBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public async Task<CloudBlockBlob> GetBlobAsync(string blobName)
        {
            return await GetBlobAsync(blobName, CONTAINER);
        }
        public async Task<string> GetBlobBase64Async(string blobName)
        {
            return await GetBlobBase64Async(blobName, CONTAINER);
        }
        public async Task DeleteBlobAsync(string blobName)
        {
            await DeleteBlobAsync(blobName, CONTAINER);
        }
        public async Task UploadBlobAsync(IFormFile blobfile, string fileName)
        {
            await UploadBlobAsync(blobfile, fileName, CONTAINER);
        }
        public async Task UploadBlobForBase64Async(string base64, string fileName)
        {
            await UploadBlobForBase64Async(base64, fileName, CONTAINER);
        }

    }
}
