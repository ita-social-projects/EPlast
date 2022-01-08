using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class AboutBaseBlobStorageRepository : BlobStorageRepository, IAboutBaseBlobStorageRepository
    {
        private const string CONTAINER = "AboutBaseImages";
        public AboutBaseBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
