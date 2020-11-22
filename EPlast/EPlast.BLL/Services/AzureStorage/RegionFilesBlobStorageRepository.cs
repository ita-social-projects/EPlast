using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

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
