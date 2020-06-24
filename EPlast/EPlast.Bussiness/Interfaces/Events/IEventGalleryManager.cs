using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Events
{
    public interface IEventGalleryManager
    {
        Task<int> AddPicturesAsync(int id, IList<IFormFile> files);
        Task<int> DeletePictureAsync(int id);
    }
}
