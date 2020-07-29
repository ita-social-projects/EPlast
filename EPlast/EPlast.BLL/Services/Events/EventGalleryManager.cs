using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class EventGalleryManager : IEventGalleryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEventBlobStorageRepository _eventBlobStorage;


        public EventGalleryManager(IRepositoryWrapper repoWrapper, IEventBlobStorageRepository eventBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _eventBlobStorage = eventBlobStorage;
        }

        public async Task<int> AddPicturesAsync(int id, IList<IFormFile> files)
        {
            try
            {
                foreach (IFormFile file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        await _eventBlobStorage.UploadBlobAsync(file, fileName);
                        var gallery = new Gallary() { GalaryFileName = fileName };
                        await _repoWrapper.Gallary.CreateAsync(gallery);
                        await _repoWrapper.EventGallary.CreateAsync(new EventGallary { EventID = id, Gallary = gallery });
                    }
                }
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

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


        public async Task<IEnumerable<EventGalleryDTO>> ConvertPicturesToBase64(IEnumerable<EventGalleryDTO> galleryDTOs)
        {
            List<EventGalleryDTO> dto = new List<EventGalleryDTO>();
            foreach(var galleryDTO in galleryDTOs)
            {
                dto.Add(new EventGalleryDTO
                {
                    GalleryId=galleryDTO.GalleryId,
                    FileName =await _eventBlobStorage.GetBlobBase64Async(galleryDTO.FileName)
                });
            }
            return dto;
        }
    }
}
