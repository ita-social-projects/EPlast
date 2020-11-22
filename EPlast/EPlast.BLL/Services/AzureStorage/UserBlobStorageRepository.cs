using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class UserBlobStorageRepository : BlobStorageRepository, IUserBlobStorageRepository
    {
        private const string CONTAINER = "UserImages";
        public UserBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
