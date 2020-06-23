using EPlast.BussinessLayer.Interfaces.AzureStorage;
using EPlast.BussinessLayer.Interfaces.AzureStorage.Base;
using EPlast.BussinessLayer.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.AzureStorage
{
    public class UserBlobStorageRepository : BlobStorageRepository, IUserBlobStorageRepository
    {
        private const string CONTAINER = "UserImages";
        public UserBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public async Task<CloudBlockBlob> GetBlobAsync(string blobName)
        {
            return await this.GetBlobAsync(blobName, CONTAINER);
        }
        public async Task DeleteBlobAsync(string blobName)
        {
            await this.DeleteBlobAsync(blobName, CONTAINER);
        }
        public async Task UploadBlobAsync(IFormFile blobfile, string fileName)
        {
            await this.UploadBlobAsync(blobfile, fileName, CONTAINER);
        }
        public async Task UploadBlobForBase64Async(string base64, string fileName)
        {
            await this.UploadBlobForBase64Async(base64, fileName, CONTAINER);
        }
    }
}
