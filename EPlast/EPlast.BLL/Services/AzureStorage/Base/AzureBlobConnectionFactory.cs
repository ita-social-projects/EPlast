using EPlast.BLL.Interfaces.AzureStorage.Base;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AzureStorage.Base
{
    public class AzureBlobConnectionFactory : IAzureBlobConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private CloudBlobClient _blobClient;

        public AzureBlobConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<CloudBlobContainer> GetBlobContainer(string containerNameKey)
        {
            var containerName = _configuration.GetValue<string>(containerNameKey);
            var blobClient = GetBlobClient();

            CloudBlobContainer _blobContainer = blobClient.GetContainerReference(containerName);

            if (await _blobContainer.CreateIfNotExistsAsync())
            {
                await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions
                { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return _blobContainer;
        }
        private CloudBlobClient GetBlobClient()
        {
            if (_blobClient != null)
            {
                return _blobClient;
            }

            var storageConnectionString = _configuration.GetValue<string>("StorageConnectionString");

            if (!CloudStorageAccount.TryParse(storageConnectionString, out var storageAccount))
            {
                throw new ArgumentException("Could not create storage account with StorageConnectionString configuration");
            }

            _blobClient = storageAccount.CreateCloudBlobClient();
            return _blobClient;
        }
    }
}
