using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.Services.CityClub
{
    public class CityClubBase: ICityClubBase
    {
        public string GetChangedPhoto(string path, IFormFile file, string oldImageName, string webPath, string uniqueId)
        {
            string logo;
            if (file != null && file.Length > 0)
            {
                using var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(webPath, path);
                if (!string.IsNullOrEmpty(oldImageName))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }

                var fileName = $"{uniqueId}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                img.Save(filePath);
                logo = fileName;
            }
            else
            {
                logo = oldImageName;
            }

            return logo;
        }
    }
}
