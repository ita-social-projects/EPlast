using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.AzureStorage.Base
{
    public interface IAzureBlobConnectionFactory
    {
        Task<CloudBlobContainer> GetBlobContainer(string containerName);
    }
}
