using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class BlankFilesBlobStorageRepository : BlobStorageRepository, IBlankFilesBlobStorageRepository
    {
        private const string CONTAINER = "BlankImages";
        public BlankFilesBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
