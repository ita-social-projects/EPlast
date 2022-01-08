using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.AboutBase
{
    public class AboutBasePicturesManager : IAboutBasePicturesManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IAboutBaseBlobStorageRepository _aboutBaseBlobStorage;
        private readonly IUniqueIdService _uniqueId;

        public AboutBasePicturesManager(IRepositoryWrapper repoWrapper, IAboutBaseBlobStorageRepository aboutBaseBlobStorage, IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _aboutBaseBlobStorage = aboutBaseBlobStorage;
            _uniqueId = uniqueId;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubsectionPicturesDTO>> AddPicturesAsync(int id, IList<IFormFile> files)
        {

            var uploadedPictures = new List<SubsectionPicturesDTO>();
            var createdGalleries = new List<Pictures>();
            foreach (IFormFile file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{_uniqueId.GetUniqueId()}{Path.GetExtension(file.FileName)}";
                    await _aboutBaseBlobStorage.UploadBlobAsync(file, fileName);
                    var gallery = new Pictures() { PictureFileName = fileName };
                    await _repoWrapper.Pictures.CreateAsync(gallery);
                    var subsectionPictures = new SubsectionPictures { SubsectionID = id, Pictures = gallery };
                    await _repoWrapper.SubsectionPictures.CreateAsync(subsectionPictures);
                    createdGalleries.Add(gallery);
                }
            }
            await _repoWrapper.SaveAsync();
            foreach (var gallery in createdGalleries)
            {
                uploadedPictures.Add(new SubsectionPicturesDTO
                {
                    PictureId = gallery.ID,
                    FileName = await _aboutBaseBlobStorage.GetBlobBase64Async(gallery.PictureFileName)
                });
            }

            return uploadedPictures;
        }

        /// <inheritdoc />
        public async Task<int> DeletePictureAsync(int id)
        {
            try
            {
                Pictures objectToDelete = await _repoWrapper.Pictures
                    .GetFirstAsync(g => g.ID == id);
                _repoWrapper.Pictures.Delete(objectToDelete);
                await _aboutBaseBlobStorage.DeleteBlobAsync(objectToDelete.PictureFileName);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubsectionPicturesDTO>> GetPicturesInBase64(int subsectionId)
        {
            var galleries = (await _repoWrapper.SubsectionPictures
                .GetAllAsync(
                eg => eg.SubsectionID == subsectionId,
                source => source.Include(eg => eg.Pictures)
                ))
                .Select(eg => eg.Pictures);

            List<SubsectionPicturesDTO> pictures = new List<SubsectionPicturesDTO>();
            foreach (var gallery in galleries)
            {
                pictures.Add(new SubsectionPicturesDTO
                {
                    PictureId = gallery.ID,
                    FileName = await _aboutBaseBlobStorage.GetBlobBase64Async(gallery.PictureFileName)
                });
            }

            return pictures;
        }
    }
}
