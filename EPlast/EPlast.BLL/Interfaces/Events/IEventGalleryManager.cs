using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event gallery.
    /// </summary>
    public interface IEventGalleryManager
    {
        /// <summary>
        /// Add pictures to gallery of specific event by event Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="id">The Id of event</param>
        /// <param name="files">List of uploaded pictures</param>
        Task<IEnumerable<EventGalleryDto>> AddPicturesAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="id">The Id of picture</param>
        Task<int> DeletePictureAsync(int id);

        /// <summary>
        /// Get pictures in Base64 format by event Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="eventId">The Id of event</param>
        Task<IEnumerable<EventGalleryDto>> GetPicturesInBase64(int eventId);
    }
}
