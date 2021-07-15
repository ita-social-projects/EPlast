using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class GoverningBodySectorBlobStorageRepository : BlobStorageRepository, IGoverningBodySectorBlobStorageRepository
    {
        private const string CONTAINER = "GoverningBodySectorsImages";
        public GoverningBodySectorBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}