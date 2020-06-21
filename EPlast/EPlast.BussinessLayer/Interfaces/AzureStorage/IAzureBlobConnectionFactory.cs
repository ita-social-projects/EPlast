using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.AzureStorage
{
    public interface IAzureBlobConnectionFactory
    {
        // Task<CloudBlobContainer> GetUserPhotosBlobContainer();
        Task<CloudBlobContainer> GetBlobContainer(string containerName);
    }
}
