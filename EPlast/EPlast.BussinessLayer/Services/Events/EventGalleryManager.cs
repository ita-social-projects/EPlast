using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventGalleryManager : IEventGalleryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;

        public EventGalleryManager(IRepositoryWrapper repoWrapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _env = env;
        }

        public async Task<int> AddPicturesAsync(int id, IList<IFormFile> files)
        {
            try
            {
                foreach (IFormFile file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        using (var img = Image.FromStream(file.OpenReadStream()))
                        {
                            var uploads = Path.Combine(_env.WebRootPath, "images\\EventsGallery");
                            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploads, fileName);
                            img.Save(filePath);
                            var gallery = new Gallary() { GalaryFileName = fileName };
                            await _repoWrapper.Gallary.CreateAsync(gallery);
                            await _repoWrapper.EventGallary.CreateAsync(new EventGallary { EventID = id, Gallary = gallery });
                        }
                    }
                }
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<int> DeletePictureAsync(int id)
        {
            try
            {
                Gallary objectToDelete = await _repoWrapper.Gallary
                    .GetFirstAsync(predicate: g => g.ID == id);
                _repoWrapper.Gallary.Delete(objectToDelete);
                var picturePath = Path.Combine(_env.WebRootPath, "images\\EventsGallery", objectToDelete.GalaryFileName);
                if (File.Exists(picturePath))
                {
                    File.Delete(picturePath);
                }
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
