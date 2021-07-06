using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodiesService : IGoverningBodiesService
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
            var oldImageName = (await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(i => i.ID == governingBody.Id))?.Logo;
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

        public async Task<GoverningBodyProfileDTO> GetGoverningBodyProfileAsync(int governingBodyId)
        {
            var governingBody = await GetGoverningBodyByIdAsync(governingBodyId);
            if (governingBody == null)
            {
                return null;
            }

            var governingBodyHead = governingBody.GoverningBodyAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.GoverningBodyHead
                                     && (DateTime.Now < a.EndDate || a.EndDate == null));
            var governingBodyAdmins = governingBody.GoverningBodyAdministration?
                .Where(a => a.AdminType.AdminTypeName != Roles.GoverningBodyHead
                            && (DateTime.Now < a.EndDate || a.EndDate == null))
                .Take(6)
                .ToList();
            governingBody.AdministrationCount = governingBody.GoverningBodyAdministration
                .Count(a => (DateTime.Now < a.EndDate || a.EndDate == null));
            var governingBodyDoc = governingBody.GoverningBodyDocuments?.Take(6).ToList();

            var governingBodyProfileDto = new GoverningBodyProfileDTO
            {
                GoverningBody = governingBody,
                Head = governingBodyHead,
                GoverningBodyAdministration = governingBodyAdmins,
                Documents = governingBodyDoc,
            };

            return governingBodyProfileDto;
        }

        public async Task<GoverningBodyDTO> GetGoverningBodyByIdAsync(int id)
        {
            var governingBody = await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(
                gb => gb.ID == id,
                source => source
                    .Include(g => g.GoverningBodyAdministration)
                        .ThenInclude(a => a.AdminType)
                    .Include(g => g.GoverningBodyAdministration)
                        .ThenInclude(a => a.User)
                    .Include(g => g.GoverningBodyDocuments)
                    .ThenInclude(d => d.GoverningBodyDocumentType));
            return _mapper.Map<Organization, GoverningBodyDTO>(governingBody);
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

        public async Task<GoverningBodyProfileDTO> GetGoverningBodyDocumentsAsync(int governingBodyId)
        {
            var governingBody = await GetGoverningBodyByIdAsync(governingBodyId);
            if (governingBody == null)
            {
                return null;
            }

            var governingBodyDoc = governingBody.GoverningBodyDocuments.ToList();

            var governingBodyProfileDto = new GoverningBodyProfileDTO
            {
                GoverningBody = governingBody,
                Documents = governingBodyDoc
            };

            return governingBodyProfileDto;
        }

        public async Task<Dictionary<string, bool>> GetUserAccessAsync(string userId)
        {
            return await _securityModel.GetUserAccessAsync(userId);
        }
    }
}
