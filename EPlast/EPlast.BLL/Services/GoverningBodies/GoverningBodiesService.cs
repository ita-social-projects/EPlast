﻿using AutoMapper;
using AutoMapper.Internal;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Entities.GoverningBody;
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
        private readonly IGoverningBodyAdministrationService _governingBodyAdministrationService;
        private readonly IGoverningBodyBlobStorageRepository _governingBodyBlobStorage;
        private readonly ISectorService _sectorService;
        private readonly ISecurityModel _securityModel;
        private const string SecuritySettingsFile = "GoverningBodyAccessSettings.json";

        public GoverningBodiesService(IRepositoryWrapper repoWrapper,
                                      IMapper mapper,
                                      IUniqueIdService uniqueId,
                                      IGoverningBodyBlobStorageRepository governingBodyBlobStorage,
                                      ISecurityModel securityModel,
                                      IGoverningBodyAdministrationService governingBodyAdministrationService,
                                      ISectorService sectorService)
        {
            _securityModel = securityModel;
            _securityModel.SetSettingsFile(SecuritySettingsFile);
            _uniqueId = uniqueId;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _governingBodyBlobStorage = governingBodyBlobStorage;
            _governingBodyAdministrationService = governingBodyAdministrationService;
            _sectorService = sectorService;
        }

        public async Task<int> CreateAsync(GoverningBodyDTO governingBodyDto)
        {
            var existingGoverningBody = await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(x => x.OrganizationName == governingBodyDto.GoverningBodyName && x.IsActive);
            if(existingGoverningBody != null)
            {
                throw new ArgumentException("The governing body with the same name already exists");
            }
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
            return _mapper.Map<IEnumerable<GoverningBodyDTO>>((await _repoWrapper.GoverningBody.GetAllAsync(x => x.IsActive)));
        }

        public async Task<IEnumerable<GoverningBodyDTO>> GetSectorsListAsync(int governingBodyId)
        {
            var governingBody = await GetGoverningBodyByIdAsync(governingBodyId);
            if (governingBody == null)
            {
                return null;
            }
            governingBody.GoverningBodySectors = governingBody.GoverningBodySectors?.Where(x => x.IsActive);

            return _mapper.Map<IEnumerable<GoverningBodyDTO>>(governingBody);
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
            governingBody.GoverningBodySectors = governingBody.GoverningBodySectors?.Where(x => x.IsActive);
            var governingBodyHead = governingBody.GoverningBodyAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.GoverningBodyHead
                                     && (DateTime.Now < a.EndDate || a.EndDate == null));

            var governingBodyAdmins = governingBody.GoverningBodyAdministration?
                .Where(a => a.AdminType.AdminTypeName != Roles.GoverningBodyHead
                            && (DateTime.Now < a.EndDate || a.EndDate == null))
                .Take(6)
                .ToList();

            governingBody.AdministrationCount = governingBody.GoverningBodyAdministration == null ? 0 :
                governingBody.GoverningBodyAdministration.Count(a => (DateTime.Now < a.EndDate || a.EndDate == null));

            var governingBodyDoc = governingBody.GoverningBodyDocuments?.Take(6).ToList();

            var governingBodySectors = governingBody.GoverningBodySectors?.Take(6).ToList();

            var governingBodyAnnouncements = governingBody.GoverningBodyAnnouncements?.ToList();

            var governingBodyProfileDto = new GoverningBodyProfileDTO
            {
                GoverningBody = governingBody,
                Head = governingBodyHead,
                GoverningBodyAdministration = governingBodyAdmins,
                Documents = governingBodyDoc,
                Announcements = governingBodyAnnouncements,
                Sectors = governingBodySectors
            };

            return governingBodyProfileDto;
        }

        public async Task<GoverningBodyDTO> GetGoverningBodyByIdAsync(int id)
        {
            var governingBody = await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(
                gb => gb.ID == id && gb.IsActive,
                source => source
                    .Include(g => g.GoverningBodySectors)
                    .Include(g => g.GoverningBodyAdministration)
                        .ThenInclude(a => a.AdminType)
                    .Include(g => g.GoverningBodyAdministration)
                        .ThenInclude(a => a.User)
                    .Include(g => g.GoverningBodyDocuments)
                        .ThenInclude(d => d.GoverningBodyDocumentType)
                     .Include(g => g.GoverningBodyAnnouncement)
                        .ThenInclude(d => d.Images)
                     .Include(g => g.GoverningBodyAnnouncement)
                        .ThenInclude(d => d.User));
            return _mapper.Map<Organization, GoverningBodyDTO>(governingBody);
        }

        public async Task<int> RemoveAsync(int governingBodyId)
        {
            var governingBody = await _repoWrapper.GoverningBody.GetFirstOrDefaultAsync(gb => gb.ID == governingBodyId);
            governingBody.IsActive = false;

            var sectors = await _sectorService.GetSectorsByGoverningBodyAsync(governingBodyId);
            foreach(var sector in sectors)
            {
                await _sectorService.RemoveAsync(sector.Id);
            }

            var admins = (await _repoWrapper.GoverningBodyAdministration.GetAllAsync(x => x.GoverningBodyId == governingBodyId))
                ?? new List<GoverningBodyAdministration>();
            foreach (var admin in admins)
            {
                await _governingBodyAdministrationService.RemoveAdministratorAsync(admin.Id);
            }

            _repoWrapper.GoverningBody.Update(governingBody);
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

            var governingBodyDoc = DocumentsSorter<GoverningBodyDocumentsDTO>.SortDocumentsBySubmitDate(governingBody.GoverningBodyDocuments);

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

        public async Task<IEnumerable<GoverningBodyAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repoWrapper.GoverningBodyAdministration.GetAllAsync(a => a.UserId == UserId  && a.Status,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.GoverningBody)
                 );

            admins.Where(x => x.GoverningBody != null).ForAll(x => x.GoverningBody.GoverningBodyAdministration = null);
            return _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(admins);
        }

        public async Task<IEnumerable<GoverningBodyAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId)
        {
            var admins = await _repoWrapper.GoverningBodyAdministration.GetAllAsync(a => a.UserId == UserId && a.EndDate < DateTime.Now,
                 include:
                 source => source.Include(c => c.User).Include(c => c.AdminType).Include(a => a.GoverningBody)
                 );

            admins.Where(x => x.GoverningBody != null).ForAll(x => x.GoverningBody.GoverningBodyAdministration = null);
            return _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(admins).Reverse();
        }
        
        /// <inheritdoc />
        public async Task<Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>> GetAdministrationForTableAsync(
            string userId, bool isActive, int pageNumber, int pageSize)
        {
            var admins = await _repoWrapper.GoverningBodyAdministration.GetAllAsync(
                predicate: g => g.UserId == userId && g.Status == isActive,
                include: source => source
                    .OrderBy(o => o.StartDate)
                    .Include(u => u.User)
                    .Include(a => a.AdminType)
                    .Include(b => b.GoverningBody));

            var rowCount = admins.Count();

            admins = admins.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return new Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>(
                _mapper.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(
                    admins), rowCount);
        }

        /// <inheritdoc />
        public async Task ContinueGoverningBodyAdminsDueToDateAsync()
        {
            var admins = await _repoWrapper.GoverningBodyAdministration.GetAllAsync(x => x.Status);

            foreach (var admin in admins)
            {
                if (admin.EndDate == null || DateTime.Compare((DateTime) admin.EndDate, DateTime.Now) >= 0) continue;
                admin.EndDate = admin.EndDate.Value.AddYears(1);
                _repoWrapper.GoverningBodyAdministration.Update(admin);
            }

            await _repoWrapper.SaveAsync();
        }
    }
}
