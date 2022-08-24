using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Events
{
    public class EventGalleryManager : IEventGalleryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEventBlobStorageRepository _eventBlobStorage;

        public EventGalleryManager(
            IRepositoryWrapper repoWrapper,
            IEventBlobStorageRepository eventBlobStorage
        )
        {
            _repoWrapper = repoWrapper;
            _eventBlobStorage = eventBlobStorage;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDto>> AddPicturesAsync(int id, IList<IFormFile> files)
        {

            var uploadedPictures = new List<EventGalleryDto>();
            var createdGalleries = new List<Gallary>();
            foreach (IFormFile file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    await _eventBlobStorage.UploadBlobAsync(file, fileName);
                    var gallery = new Gallary() { GalaryFileName = fileName };
                    await _repoWrapper.Gallary.CreateAsync(gallery);
                    var eventGallery = new EventGallary { EventID = id, Gallary = gallery };
                    await _repoWrapper.EventGallary.CreateAsync(eventGallery);
                    createdGalleries.Add(gallery);
                }
            }
            await _repoWrapper.SaveAsync();
            foreach (var gallery in createdGalleries)
            {
                uploadedPictures.Add(new EventGalleryDto
                {
                    GalleryId = gallery.ID,
                    FileName = await _eventBlobStorage.GetBlobBase64Async(gallery.GalaryFileName)
                });
            }

            return uploadedPictures;
        }

        /// <inheritdoc />
        public async Task<int> DeletePictureAsync(int id)
        {
            try
            {
                Gallary objectToDelete = await _repoWrapper.Gallary
                    .GetFirstAsync(g => g.ID == id);
                _repoWrapper.Gallary.Delete(objectToDelete);
                await _eventBlobStorage.DeleteBlobAsync(objectToDelete.GalaryFileName);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDto>> GetPicturesInBase64(int eventId)
        {
            var galleries = (await _repoWrapper.EventGallary
                .GetAllAsync(
                eg => eg.EventID == eventId,
                source => source.Include(eg => eg.Gallary)
                ))
                .Select(eg => eg.Gallary);

            List<EventGalleryDto> pictures = new List<EventGalleryDto>();
            foreach (var gallery in galleries)
            {
                pictures.Add(new EventGalleryDto
                {
                    GalleryId = gallery.ID,
                    FileName = await _eventBlobStorage.GetBlobBase64Async(gallery.GalaryFileName),
                    BlobName = gallery.GalaryFileName
                });
            }

            return pictures;
        }
    }
}
