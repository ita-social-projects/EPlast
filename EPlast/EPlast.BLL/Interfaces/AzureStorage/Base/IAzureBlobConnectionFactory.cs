using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.AzureStorage.Base
{
    public interface IAzureBlobConnectionFactory
    {
        Task<CloudBlobContainer> GetBlobContainer(string containerName);
    }
}
