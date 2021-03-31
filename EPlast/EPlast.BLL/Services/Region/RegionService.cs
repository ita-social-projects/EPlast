using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataAccessRegion = EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.Region
{
    public class RegionService : IRegionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IRegionBlobStorageRepository _regionBlobStorage;
        private readonly IRegionFilesBlobStorageRepository _regionFilesBlobStorageRepository;
        private readonly ICityService _cityService;
        private readonly IUniqueIdService _uniqueId;
        private readonly UserManager<User> _userManager;

        public RegionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IRegionFilesBlobStorageRepository regionFilesBlobStorageRepository,
            IRegionBlobStorageRepository regionBlobStorage,
            ICityService cityService,
            IUniqueIdService uniqueId, UserManager<User> userManager)
        {
            _regionFilesBlobStorageRepository = regionFilesBlobStorageRepository;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _regionBlobStorage = regionBlobStorage;
            _cityService = cityService;
            _uniqueId = uniqueId;
            _userManager = userManager;
        }

        public async Task AddRegionAsync(RegionDTO region)
        {
            if (await CheckCreated(region.RegionName))
            {
                throw new InvalidOperationException();
            }

            await _repoWrapper.Region.CreateAsync(_mapper.Map<RegionDTO, DataAccessRegion.Region>(region));

            await _repoWrapper.SaveAsync();
        }

        private async Task<bool> CheckCreated(string name)
        {
            return await _repoWrapper.Region.GetFirstOrDefaultAsync(
                predicate: a => a.RegionName == name) != null;
        }

        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard);
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionDTO>>(filteredRegions);
        }

        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _regionBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        public async Task<RegionDTO> GetRegionByIdAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(
                predicate: r => r.ID == regionId,
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));

            return _mapper.Map<DataAccessRegion.Region, RegionDTO>(region);
        }

        public async Task<RegionProfileDTO> GetRegionProfileByIdAsync(int regionId, User user)
        {
            var region = await GetRegionByIdAsync(regionId);
            var regionProfile = _mapper.Map<RegionDTO, RegionProfileDTO>(region);
            var userRoles = await _userManager.GetRolesAsync(user);

            var cities = await _cityService.GetCitiesByRegionAsync(regionId);
            regionProfile.Cities = cities;
            regionProfile.City = region.City;
            regionProfile.CanEdit = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.OkrugaHead);

            return regionProfile;
        }
        public async Task DeleteRegionByIdAsync(int regionId)
        {
            _repoWrapper.Region.Delete(await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId && d.Status != RegionsStatusType.RegionBoard));
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task AddFollowerAsync(int regionId, int cityId)
        {
            var region = (await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId));
            var city = (await _repoWrapper.City.GetFirstAsync(d => d.ID == cityId));

            city.Region = region;
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<CityDTO>> GetMembersAsync(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(cities);
        }

        public async Task<RegionProfileDTO> GetRegionByNameAsync(string Name, User user)
        {
            var regionProfile = _mapper.Map<DataAccessRegion.Region, RegionProfileDTO>(await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name));
            var userRoles = await _userManager.GetRolesAsync(user);
            regionProfile.CanEdit = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.OkrugaHead);
            return regionProfile;
        }

        public async Task<RegionDTO> GetRegionByNameAsync(string Name) { 
            return _mapper.Map<DataAccessRegion.Region, RegionDTO>(await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name));
        }

        public async Task EditRegionAsync(int regId, RegionDTO region)
        {
            var ChangedRegion = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regId);

            ChangedRegion.Logo = region.Logo;
            ChangedRegion.City = region.City;
            ChangedRegion.Link = region.Link;
            ChangedRegion.Email = region.Email;
            ChangedRegion.OfficeNumber = region.OfficeNumber;
            ChangedRegion.PhoneNumber = region.PhoneNumber;
            ChangedRegion.PostIndex = region.PostIndex;
            ChangedRegion.RegionName = region.RegionName;
            ChangedRegion.Description = region.Description;
            ChangedRegion.Street = region.Street;
            ChangedRegion.HouseNumber = region.HouseNumber;

            _repoWrapper.Region.Update(ChangedRegion);
            await _repoWrapper.SaveAsync();
        }


        public async Task<RegionDocumentDTO> AddDocumentAsync(RegionDocumentDTO documentDTO)
        {
            var fileBase64 = documentDTO.BlobName.Split(',')[1];
            var extension = $".{documentDTO.FileName.Split('.').LastOrDefault()}";
            var fileName = $"{_uniqueId.GetUniqueId()}{extension}";
            await _regionFilesBlobStorageRepository.UploadBlobForBase64Async(fileBase64, fileName);
            documentDTO.BlobName = fileName;

            var document = _mapper.Map<RegionDocumentDTO, RegionDocuments>(documentDTO);
            _repoWrapper.RegionDocument.Attach(document);
            await _repoWrapper.RegionDocument.CreateAsync(document);
            await _repoWrapper.SaveAsync();

            return documentDTO;
        }

        public async Task<IEnumerable<RegionDocumentDTO>> GetRegionDocsAsync(int regionId)
        {
            var documents = await _repoWrapper.RegionDocument.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDTO>>(documents);
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            return await _regionFilesBlobStorageRepository.GetBlobBase64Async(fileName);
        }


        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repoWrapper.RegionDocument
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _regionFilesBlobStorageRepository.DeleteBlobAsync(document.BlobName);

            _repoWrapper.RegionDocument.Delete(document);
            await _repoWrapper.SaveAsync();
        }



        public async Task RedirectMembers(int prevRegId, int nextRegId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == prevRegId);
            foreach (var city in cities)
            {
                city.RegionId = nextRegId;
                _repoWrapper.City.Update(city);
            }
            await _repoWrapper.SaveAsync();
        }

        public async Task EndAdminsDueToDate()
        {
            var admins = await _repoWrapper.RegionAdministration.GetAllAsync();

            foreach (var admin in admins)
            {
                if (DateTime.Compare((DateTime)admin.EndDate, DateTime.Now) < 0)
                {
                    admin.Status = false;
                    _repoWrapper.RegionAdministration.Update(admin);

                }
            }
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RegionForAdministrationDTO>> GetRegions()
        {
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionForAdministrationDTO>>((await _repoWrapper.Region.GetAllAsync()).Where(r => r.Status != RegionsStatusType.RegionBoard));
        }
    }
}
