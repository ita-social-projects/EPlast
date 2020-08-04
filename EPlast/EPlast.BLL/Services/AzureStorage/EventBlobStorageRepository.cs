﻿using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AzureStorage
{
    public class EventBlobStorageRepository : BlobStorageRepository, IEventBlobStorageRepository
    {
        private const string CONTAINER = "EventsImages";
        public EventBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public async Task<CloudBlockBlob> GetBlobAsync(string blobName)
        {
            return await this.GetBlobAsync(blobName, CONTAINER);
        }
        public async Task<string> GetBlobBase64Async(string blobName)
        {
            return await this.GetBlobBase64Async(blobName, CONTAINER);
        }
        public async Task DeleteBlobAsync(string blobName)
        {
            await this.DeleteBlobAsync(blobName, CONTAINER);
        }
        public async Task UploadBlobAsync(IFormFile blobFile, string fileName)
        {
            await this.UploadBlobAsync(blobFile, fileName, CONTAINER);
        }
        public async Task UploadBlobForBase64Async(string base64, string fileName)
        {
            await this.UploadBlobForBase64Async(base64, fileName, CONTAINER);
        }
    }
}