using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.AboutBase
{
    public class AboutBasePicturesManager : IAboutBasePicturesManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IAboutBaseBlobStorageRepository _aboutBaseBlobStorage;

        public AboutBasePicturesManager(
            IRepositoryWrapper repoWrapper,
            IAboutBaseBlobStorageRepository aboutBaseBlobStorage
        )
        {
            _repoWrapper = repoWrapper;
            _aboutBaseBlobStorage = aboutBaseBlobStorage;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SubsectionPicturesDto>> AddPicturesAsync(int id, IList<IFormFile> files)
        {

            var uploadedPictures = new List<SubsectionPicturesDto>();
            var createdGalleries = new List<Pictures>();
            foreach (IFormFile file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
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
                uploadedPictures.Add(new SubsectionPicturesDto
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
        public async Task<IEnumerable<SubsectionPicturesDto>> GetPicturesInBase64(int subsectionId)
        {
            var galleries = (await _repoWrapper.SubsectionPictures
                .GetAllAsync(
                eg => eg.SubsectionID == subsectionId,
                source => source.Include(eg => eg.Pictures)
                ))
                .Select(eg => eg.Pictures);

            List<SubsectionPicturesDto> pictures = new List<SubsectionPicturesDto>();
            foreach (var gallery in galleries)
            {
                pictures.Add(new SubsectionPicturesDto
                {
                    PictureId = gallery.ID,
                    FileName = await _aboutBaseBlobStorage.GetBlobBase64Async(gallery.PictureFileName)
                });
            }

            return pictures;
        }
    }
}
