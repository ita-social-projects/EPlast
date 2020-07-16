using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AzureStorage
{
    public class ClubBlobStorageRepository : BlobStorageRepository, IClubBlobStorageRepository
    {
        private const string Container = "ClubImages";

        public ClubBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<CloudBlockBlob> GetBlobAsync(string blobName)
        {
            return await this.GetBlobAsync(blobName, Container);
        }

        public async Task<string> GetBlobBase64Async(string blobName)
        {
            return await this.GetBlobBase64Async(blobName, Container);
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            await this.DeleteBlobAsync(blobName, Container);
        }

        public async Task UploadBlobAsync(IFormFile blobFile, string fileName)
        {
            await this.UploadBlobAsync(blobFile, fileName, Container);
        }

        public async Task UploadBlobForBase64Async(string base64, string fileName)
        {
            await this.UploadBlobForBase64Async(base64, fileName, Container);
        }
    }
}