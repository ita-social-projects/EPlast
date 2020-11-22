using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Services.AzureStorage.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

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
