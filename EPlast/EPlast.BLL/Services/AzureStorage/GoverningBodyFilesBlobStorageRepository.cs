using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class GoverningBodyFilesBlobStorageRepository : BlobStorageRepository, IGoverningBodyFilesBlobStorageRepository
    {
        private const string CONTAINER = "GoverningBodyFiles";

        public GoverningBodyFilesBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
