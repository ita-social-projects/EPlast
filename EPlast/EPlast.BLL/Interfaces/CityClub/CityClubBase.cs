using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.Services.CityClub
{
    public interface ICityClubBase
    {
        string GetChangedPhoto(string path, IFormFile file, string oldImageName, string webPath, string uniqueId);
    }
}
