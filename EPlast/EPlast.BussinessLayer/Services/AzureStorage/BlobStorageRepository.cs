using EPlast.BussinessLayer.Interfaces.AzureStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.AzureStorage
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IAzureBlobConnectionFactory _connectionFactory;
        public BlobStorageRepository(IAzureBlobConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CloudBlockBlob> GetBlobAsync(string blobName, string containerName)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(containerName);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            return blockBlob;
        }
        public async Task<bool> DeleteBlobAsync(string blobName, string containerName)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(containerName);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            bool deleted = blockBlob.DeleteIfExists();
            return deleted;
        }

        public async Task<bool> UploadBlobAsync(IFormFile blobfile, string fileName, string containerName)
        {
            if (blobfile == null)
            {
                return false;
            }

            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(containerName);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            using (var fileStream = (blobfile.OpenReadStream()))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return true;
        }
    }
}
