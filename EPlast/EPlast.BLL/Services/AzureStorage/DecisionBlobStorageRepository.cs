using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class DecisionBlobStorageRepository : BlobStorageRepository, IDecisionBlobStorageRepository
    {
        private const string CONTAINER = "DecisionsFiles";
        public DecisionBlobStorageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }
    }
}
