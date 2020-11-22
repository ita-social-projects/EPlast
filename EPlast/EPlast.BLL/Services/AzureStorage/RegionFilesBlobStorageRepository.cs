using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Interfaces.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class RegionFilesBlobStorageRepository: BlobStorageRepository, IRegionFilesBlobStorageRepository
    {
        private const string CONTAINER = "RegionFiles";
        public RegionFilesBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
