using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBSector = EPlast.DataAccess.Entities.GoverningBody.Sector.Sector;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorService : ISectorService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IUniqueIdService _uniqueId;
        private readonly IGoverningBodySectorBlobStorageRepository _sectorBlobStorage;
        private readonly ISecurityModel _securityModel;
        private const string SecuritySettingsFile = "GoverningBodySectorAccessSettings.json";
        private const int TakingItemsCount = 6;

        public SectorService(IRepositoryWrapper repoWrapper,
                             IMapper mapper,
                             IUniqueIdService uniqueId,
                             IGoverningBodySectorBlobStorageRepository sectorBlobStorage,
                             ISecurityModel securityModel)
        {
            _securityModel = securityModel;
            _securityModel.SetSettingsFile(SecuritySettingsFile);
            _uniqueId = uniqueId;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _sectorBlobStorage = sectorBlobStorage;
        }

        private async Task UploadPhotoAsync(SectorDTO sectorDto)
        {
            var oldImageName = (await _repoWrapper.GoverningBodySector.GetFirstOrDefaultAsync(i => i.Id == sectorDto.Id))?.Logo;
            var logoBase64 = sectorDto.Logo;

            if (!string.IsNullOrWhiteSpace(logoBase64))
            {
                var logoBase64Parts = logoBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

                await _sectorBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                sectorDto.Logo = fileName;
            }

            if (!string.IsNullOrWhiteSpace(oldImageName))
            {
                await _sectorBlobStorage.DeleteBlobAsync(oldImageName);
            }
        }

        public async Task<int> CreateAsync(SectorDTO sectorDto)
        {

            var existingSector = await _repoWrapper.GoverningBodySector.GetFirstOrDefaultAsync(x => x.Name == sectorDto.Name);
            if (existingSector != null)
            {
                throw new ArgumentException("The organization with the same name already exists");
            }
            await UploadPhotoAsync(sectorDto);
            var newSector = await CreateSectorAsync(sectorDto);

            _repoWrapper.GoverningBodySector.Attach(newSector);
            await _repoWrapper.GoverningBodySector.CreateAsync(newSector);
            await _repoWrapper.SaveAsync();

            return newSector.Id;
        }

        private Task<GBSector> CreateSectorAsync(SectorDTO sector)
        {
            return Task.FromResult(_mapper.Map<SectorDTO, GBSector>(sector));
        }

        public async Task<IEnumerable<SectorDTO>> GetSectorsByGoverningBodyAsync(int governingBodyId)
        {
            var sectors = await _repoWrapper.GoverningBodySector.GetAllAsync(
                s => s.GoverningBodyId == governingBodyId);
            return _mapper.Map<IEnumerable<GBSector>, IEnumerable <SectorDTO>>(sectors);
        }

        public async Task<string> GetLogoBase64Async(string logoName)
        {
            return await _sectorBlobStorage.GetBlobBase64Async(logoName);
        }

        public async Task<SectorProfileDTO> GetSectorProfileAsync(int sectorId)
        {
            var sector = await GetSectorByIdAsync(sectorId);
            if (sector == null)
            {
                return null;
            }

            var sectorHead = sector.Administration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.GoverningBodySectorHead 
                                     && (DateTime.Now < a.EndDate || a.EndDate == null));

            var sectorAdmins = sector.Administration?
                .Where(a => a.AdminType.AdminTypeName != Roles.GoverningBodySectorHead
                            && (DateTime.Now < a.EndDate || a.EndDate == null))
                .Take(TakingItemsCount)
                .ToList();

            sector.AdministrationCount = sector.Administration == null ? 0 :
                sector.Administration.Count(
                    a => (DateTime.Now < a.EndDate || a.EndDate == null));

            var sectorDocuments = sector.Documents?
                .Take(TakingItemsCount)
                .ToList();

            var sectorProfileDto = new SectorProfileDTO
            {
                Sector = sector,
                Head = sectorHead,
                Administration = sectorAdmins,
                Documents = sectorDocuments,
            };

            return sectorProfileDto;
        }

        public async Task<SectorDTO> GetSectorByIdAsync(int id)
        {
            var sector = await _repoWrapper.GoverningBodySector.GetFirstOrDefaultAsync(
                s => s.Id == id,
                source => source
                    .Include(s => s.Administration)
                        .ThenInclude(a => a.AdminType)
                    .Include(s => s.Administration)
                        .ThenInclude(a => a.User)
                    .Include(s => s.Documents)
                    .ThenInclude(d => d.SectorDocumentType));
            return _mapper.Map<GBSector, SectorDTO>(sector);
        }

        public async Task<SectorProfileDTO> GetSectorDocumentsAsync(int sectorId)
        {
            var sector = await GetSectorByIdAsync(sectorId);
            if (sector == null)
            {
                return null;
            }

            var sectorDocuments = DocumentsSorter<SectorDocumentsDTO>.SortDocumentsBySubmitDate(sector.Documents);

            var sectorProfileDto = new SectorProfileDTO()
            {
                Sector = sector,
                Documents = sectorDocuments
            };

            return sectorProfileDto;
        }

        public async Task<Dictionary<string, bool>> GetUserAccessAsync(string userId)
        {
            return await _securityModel.GetUserAccessAsync(userId);
        }

        public async Task<int> EditAsync(SectorDTO sector)
        {
            await UploadPhotoAsync(sector);
            var newSector = await CreateSectorAsync(sector);

            _repoWrapper.GoverningBodySector.Attach(newSector);
            _repoWrapper.GoverningBodySector.Update(newSector);
            await _repoWrapper.SaveAsync();

            return newSector.Id;
        }

        public async Task<int> RemoveAsync(int sectorId)
        {
            var sector = await _repoWrapper.GoverningBodySector.GetFirstOrDefaultAsync(s => s.Id == sectorId);

            if (!string.IsNullOrWhiteSpace(sector.Logo))
            {
                await _sectorBlobStorage.DeleteBlobAsync(sector.Logo);
            }

            _repoWrapper.GoverningBodySector.Delete(sector);
            await _repoWrapper.SaveAsync();
            return sectorId;
        }
    }
}
