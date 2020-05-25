using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventGalleryManager
    {
        int AddPictures(int id, IList<IFormFile> files);
        int DeletePicture(int id);
    }
}
