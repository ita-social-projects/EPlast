using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class GoverningBodySectorFilesBlobStorageRepository : BlobStorageRepository, IGoverningBodySectorFilesBlobStorageRepository
    {
        private const string CONTAINER = "GoverningBodySectorsFiles";

        public GoverningBodySectorFilesBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}