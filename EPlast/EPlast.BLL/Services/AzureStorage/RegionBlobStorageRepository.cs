using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class RegionBlobStorageRepository : BlobStorageRepository, IRegionBlobStorageRepository
    {
        private const string CONTAINER = "RegionImages";
        public RegionBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
