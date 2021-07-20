using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class GoverningBodyBlobStorageRepository : BlobStorageRepository, IGoverningBodyBlobStorageRepository
    {
        private const string CONTAINER = "GoverningBodiesImages";
        public GoverningBodyBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
