using EPlast.BLL.Interfaces.AzureStorage;
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
        public EventBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}