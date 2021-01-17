using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;

namespace EPlast.BLL.Services.AzureStorage
{
    public class MethodicDocumentBlobStarageRepository : BlobStorageRepository, IMethodicDocumentBlobStorageRepository
    {
        private const string CONTAINER = "MethodicDocumentsFiles";
        public MethodicDocumentBlobStarageRepository(IAzureBlobConnectionFactory connectionFactory) : base(connectionFactory, CONTAINER)
        {
        }

    }
}
