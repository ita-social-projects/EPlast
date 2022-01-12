using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AboutBase
{
    public class PicturesManager : IPicturesManager
    {
        private readonly IAboutBaseWrapper _aboutBaseWrapper;

        public PicturesManager(IAboutBaseWrapper aboutBaseWrapper)
        {
            _aboutBaseWrapper = aboutBaseWrapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubsectionPicturesDTO>> GetPicturesAsync(int id)
        {
            return await _aboutBaseWrapper.AboutBasePicturesManager.GetPicturesInBase64(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubsectionPicturesDTO>> FillSubsectionPicturesAsync(int id, IList<IFormFile> files)
        {
            return await _aboutBaseWrapper.AboutBasePicturesManager.AddPicturesAsync(id, files);
        }

        /// <inheritdoc />
        public async Task<int> DeletePictureAsync(int id)
        {
            return await _aboutBaseWrapper.AboutBasePicturesManager.DeletePictureAsync(id);
        }
    }
}
