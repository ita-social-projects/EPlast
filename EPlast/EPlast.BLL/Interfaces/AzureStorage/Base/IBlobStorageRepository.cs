using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.AzureStorage.Base
{
    public interface IBlobStorageRepository
    {
        /// <summary>
        /// Delete if exists blob
        /// </summary>
        /// <param name="blobNameWithExtension">Name of file with extension</param>
        Task DeleteBlobAsync(string blobNameWithExtension);

        /// <summary>
        /// Uoload blob by IFormFile
        /// </summary>
        /// <param name="blobfile">File in format IFormFile</param>
        /// <param name="fileName">Name of file with extension</param>
        Task UploadBlobAsync(IFormFile blobfile, string fileName);

        /// <summary>
        /// Get blob by CloudBlockBlob
        /// </summary>
        /// <param name="blobNameWithExtension">Name of file with extension</param>
        /// <returns>CloudBlockBlob</returns>
        Task<CloudBlockBlob> GetBlobAsync(string blobNameWithExtension);

        /// <summary>
        /// Uoload blob by base64
        /// </summary>
        /// <param name="base64">File in format base64</param>
        /// <param name="fileName">Name of file with extension</param>
        Task UploadBlobForBase64Async(string base64, string fileName);

        /// <summary>
        ///  Get blob in base64 format
        /// </summary>
        /// <param name="blobName">Name of file with extension</param>
        /// <returns>Base64 format in string</returns>
        Task<string> GetBlobBase64Async(string blobName);
    }
}
