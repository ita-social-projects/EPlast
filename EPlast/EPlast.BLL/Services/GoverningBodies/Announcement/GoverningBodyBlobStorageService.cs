using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Sector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies.Announcement
{
    public class GoverningBodyBlobStorageService : IGoverningBodyBlobStorageService
    {
        private readonly IGoverningBodyBlobStorageRepository _blobStorage;
        private readonly IUniqueIdService _uniqueId;
        public GoverningBodyBlobStorageService(
           IGoverningBodyBlobStorageRepository blobStorage, 
           IUniqueIdService uniqueId)
        {
            _blobStorage = blobStorage;
            _uniqueId = uniqueId;
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

                fileName = $"{_uniqueId.GetUniqueId()}{extension}";
                await _blobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
            }
            return fileName;
        }
    }
}
