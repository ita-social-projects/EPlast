using EPlast.BLL.DTO.Events;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventGalleryManager
    {
        Task<IEnumerable<EventGalleryDTO>> AddPicturesAsync(int id, IList<IFormFile> files);
        Task<int> DeletePictureAsync(int id);
        Task<IEnumerable<EventGalleryDTO>> GetPicturesInBase64(int eventId);
    }
}
