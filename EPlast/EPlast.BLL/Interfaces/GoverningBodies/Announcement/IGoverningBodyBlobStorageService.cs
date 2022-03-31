using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public interface IGoverningBodyBlobStorageService
    {
        public Task<string> UploadImageAsync(string imageBase64);
        public Task<string> GetImageAsync(string imageName);
    }
}