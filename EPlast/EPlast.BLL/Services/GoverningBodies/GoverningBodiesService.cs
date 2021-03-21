using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodiesService: IGoverningBodiesService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IUniqueIdService _uniqueId;
        private readonly IGoverningBodyBlobStorageRepository _governingBodyBlobStorage;


        public GoverningBodiesService(IRepositoryWrapper repoWrapper,
                                      IMapper mapper,
                                      IUniqueIdService uniqueId,
                                      IGoverningBodyBlobStorageRepository governingBodyBlobStorage)
        {
            _uniqueId = uniqueId;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _governingBodyBlobStorage = governingBodyBlobStorage;
        }

        public async Task<int> CreateAsync(GoverningBodyDTO model)
        {
            await UploadPhotoAsync(model);
            var governingBody = await CreateGoverningBodyAsync(model);

            _repoWrapper.GoverningBody.Attach(governingBody);
            await _repoWrapper.GoverningBody.CreateAsync(governingBody);
            await _repoWrapper.SaveAsync();

            return governingBody.ID;
        }

        private async Task<Organization> CreateGoverningBodyAsync(GoverningBodyDTO model)
        {
            return _mapper.Map<GoverningBodyDTO, Organization>(model);
        }

        public async Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodiesListAsync()
        {
            return _mapper.Map<IEnumerable<GoverningBodyDTO>>((await _repoWrapper.GoverningBody.GetAllAsync()));
        }

        private async Task UploadPhotoAsync(GoverningBodyDTO governingBody)
        {
            var oldImageName = (await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(i => i.ID == governingBody.ID))?.Logo;
            var logoBase64 = governingBody.Logo;

            if (!string.IsNullOrWhiteSpace(logoBase64) && logoBase64.Length > 0)
            {
                var logoBase64Parts = logoBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

                await _governingBodyBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                governingBody.Logo = fileName;
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                await _governingBodyBlobStorage.DeleteBlobAsync(oldImageName);
            }
        }

        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _governingBodyBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }
    }
}
