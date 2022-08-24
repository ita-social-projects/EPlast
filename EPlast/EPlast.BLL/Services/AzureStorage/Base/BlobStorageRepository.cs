using EPlast.BLL.Interfaces.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AzureStorage.Base
{
    public abstract class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IAzureBlobConnectionFactory _connectionFactory;
        private readonly string _containerNameKey;
        protected BlobStorageRepository(IAzureBlobConnectionFactory connectionFactory, string containerNameKey)
        {
            _connectionFactory = connectionFactory;
            _containerNameKey = containerNameKey;
        }

        /// <inheritdoc />
        public async Task<CloudBlockBlob> GetBlobAsync(string blobNameWithExtension)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(_containerNameKey);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobNameWithExtension);

            return blockBlob;
        }

        /// <inheritdoc />
        public async Task<string> GetBlobBase64Async(string blobName)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(_containerNameKey);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            blockBlob.FetchAttributes();
            byte[] arr = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArray(arr, 0);
            var azureBase64 = Convert.ToBase64String(arr);
            var result = $"data:{blockBlob.Properties.ContentType};base64," + azureBase64;
            // var result = blockBlob.Uri.OriginalString;
            return result;
        }

        /// <inheritdoc />
        public async Task DeleteBlobAsync(string blobNameWithExtension)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(_containerNameKey);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobNameWithExtension);
            await blockBlob.DeleteIfExistsAsync();
        }

        /// <inheritdoc />
        public async Task UploadBlobAsync(IFormFile blobfile, string fileName)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(_containerNameKey);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            using (var fileStream = (blobfile.OpenReadStream()))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        /// <inheritdoc />
        public async Task UploadBlobForBase64Async(string base64, string fileName)
        {
            var cloudBlobContainer = await _connectionFactory.GetBlobContainer(_containerNameKey);
            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                blockBlob.UploadFromStream(ms);
            }

        }

    }
}
