using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.AzureStorage.Base
{
    public interface IAzureBlobConnectionFactory
    {
        /// <summary>
        /// Get blob container by container name
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <returns>CloudBlockContainer</returns>
        Task<CloudBlobContainer> GetBlobContainer(string containerName);
    }
}
