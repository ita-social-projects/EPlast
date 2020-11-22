using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
   public class BlankAchievementBlobStorageRepository: BlobStorageRepository,IBlankAchievementBlobStorageRepository
    {
        private const string CONTAINER = "AchievementContainer";
        public BlankAchievementBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
