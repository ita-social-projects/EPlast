using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody;
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
        private readonly ISecurityModel _securityModel; 
        private const string SecuritySettingsFile = "GoverningBodyAccessSettings.json";

        public GoverningBodiesService(IRepositoryWrapper repoWrapper,
                                      IMapper mapper,
                                      IUniqueIdService uniqueId,
                                      IGoverningBodyBlobStorageRepository governingBodyBlobStorage,
                                      ISecurityModel securityModel)
        {
            _securityModel = securityModel;
            _securityModel.SetSettingsFile(SecuritySettingsFile);
            _uniqueId = uniqueId;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _governingBodyBlobStorage = governingBodyBlobStorage;
        }

        public async Task<int> CreateAsync(GoverningBodyDTO governingBodyDto)
        {
            await UploadPhotoAsync(governingBodyDto);
            var governingBody = await CreateGoverningBodyAsync(governingBodyDto);

            _repoWrapper.GoverningBody.Attach(governingBody);
            await _repoWrapper.GoverningBody.CreateAsync(governingBody);
            await _repoWrapper.SaveAsync();

            return governingBody.ID;
        }

        private Task<Organization> CreateGoverningBodyAsync(GoverningBodyDTO governingBody)
        {
            return Task.FromResult(_mapper.Map<GoverningBodyDTO, Organization>(governingBody));
        }

        public async Task<int> EditAsync(GoverningBodyDTO governingBody)
        {
            await UploadPhotoAsync(governingBody);
            var createdGoveringBody = await CreateGoverningBodyAsync(governingBody);

            _repoWrapper.GoverningBody.Attach(createdGoveringBody);
            _repoWrapper.GoverningBody.Update(createdGoveringBody);
            await _repoWrapper.SaveAsync();

            return createdGoveringBody.ID;
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

        public async Task<string> GetLogoBase64Async(string logoName)
        {
            return await _governingBodyBlobStorage.GetBlobBase64Async(logoName);
        }

        public async Task<GoverningBodyProfileDTO> GetProfileAsync(int id, User user)
        {
            GoverningBodyProfileDTO gbProfile = new GoverningBodyProfileDTO
            {
                GoverningBody = await GetProfileByIdAsync(id),
            };

            return gbProfile;
        }

        public async Task<GoverningBodyDTO> GetProfileByIdAsync(int id)
        {
            return _mapper.Map<GoverningBodyDTO>(await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync( gb => gb.ID == id ));
        }

        public async Task<int> RemoveAsync(int governingBodyId)
        {
            var governingBody = await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(gb => gb.ID == governingBodyId);

            if (governingBody.Logo != null)
            {
                await _governingBodyBlobStorage.DeleteBlobAsync(governingBody.Logo);
            }

            _repoWrapper.GoverningBody.Delete(governingBody);
            await _repoWrapper.SaveAsync();
            return governingBodyId;
        }

        public async Task<Dictionary<string, bool>> GetUserAccessAsync(string userId)
        {
            var userAcesses = await _securityModel.GetUserAccessAsync(userId);
            return userAcesses;
        }
    }
}
