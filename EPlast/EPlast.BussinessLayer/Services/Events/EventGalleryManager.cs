using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

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


        public int AddPictures(int id, IList<IFormFile> files)
        {
            try
            {
                foreach (IFormFile file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var img = Image.FromStream(file.OpenReadStream());
                        var uploads = Path.Combine(_env.WebRootPath, "images\\EventsGallery");
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        img.Save(filePath);
                        var gallery = new Gallary() { GalaryFileName = fileName };
                        _repoWrapper.Gallary.Create(gallery);
                        _repoWrapper.EventGallary.Create(new EventGallary { EventID = id, Gallary = gallery });
                    }
                }
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int DeletePicture(int id)
        {
            try
            {
                Gallary objectToDelete = _repoWrapper.Gallary.FindByCondition(g => g.ID == id).First();
                _repoWrapper.Gallary.Delete(objectToDelete);
                var picturePath = Path.Combine(_env.WebRootPath, "images\\EventsGallery", objectToDelete.GalaryFileName);
                if (File.Exists(picturePath))
                {
                    File.Delete(picturePath);
                }
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
