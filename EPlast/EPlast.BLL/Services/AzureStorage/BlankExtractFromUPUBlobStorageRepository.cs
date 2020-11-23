
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
   public class BlankExtractFromUpuBlobStorageRepository:BlobStorageRepository, IBlankExtractFromUPUBlobStorageRepository
    {
        private const string CONTAINER = "ExtractFromUPUContainer";
        public BlankExtractFromUpuBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
