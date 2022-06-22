using System;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Sector;

namespace EPlast.BLL.Services.GoverningBodies.Announcement
{
    public class GoverningBodyBlobStorageService : IGoverningBodyBlobStorageService
    {
        private readonly IGoverningBodyBlobStorageRepository _blobStorage;
        public GoverningBodyBlobStorageService(
           IGoverningBodyBlobStorageRepository blobStorage
        )
        {
            _blobStorage = blobStorage;
        }
        public async Task<string> GetImageAsync(string imageName)
        {
            return await _blobStorage.GetBlobBase64Async(imageName);
        }

        public async Task<string> UploadImageAsync(string imageBase64)
        {
            var fileName = "";
            if (!string.IsNullOrWhiteSpace(imageBase64) && imageBase64.Length > 0)
            {
                var logoBase64Parts = imageBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                fileName = $"{Guid.NewGuid()}{extension}";
                await _blobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
            }
            return fileName;
        }
    }
}
