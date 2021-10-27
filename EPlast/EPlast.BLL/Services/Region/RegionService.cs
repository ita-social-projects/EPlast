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
using EPlast.BLL.DTO;
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
        public async Task ArchiveRegionAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId && d.IsActive);
            region.IsActive = false;
            _repoWrapper.Region.Update(region);
            await _repoWrapper.SaveAsync();
        }

        public async Task AddRegionAsync(RegionDTO region)
        {
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
        public async Task<IEnumerable<RegionDTO>> GetAllActiveRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
               include: source => source
                   .Include(r => r.RegionAdministration)
                       .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard && r.IsActive);
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionDTO>>(filteredRegions);
        }
        public async Task<Tuple<IEnumerable<RegionObjectsDTO>, int>> GetAllRegionsByPageAndIsArchiveAsync(int page, int pageSize, string regionName, bool isArchive)
        {
            var tuple = await _repoWrapper.Region.GetRegionsObjects(page, pageSize, regionName, isArchive);
            var regions = tuple.Item1;
            var rows = tuple.Item2;
            return new Tuple<IEnumerable<RegionObjectsDTO>, int>(_mapper.Map<IEnumerable<RegionObject>, IEnumerable<RegionObjectsDTO>>(regions), rows);
        }

        public async Task<IEnumerable<RegionDTO>> GetAllNotActiveRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
              include: source => source
                  .Include(r => r.RegionAdministration)
                      .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard && !r.IsActive );
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
            var documents = await _repoWrapper
                .RegionDocument
                .GetAllAsync(d => d.RegionId == regionId);

            var cities = await _cityService.GetCitiesByRegionAsync(regionId);
            regionProfile.Cities = cities;
            regionProfile.City = region.City;
            regionProfile.CanEdit = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.OkrugaHead) || userRoles.Contains(Roles.OkrugaHeadDeputy);
            regionProfile.Documents = documents.Take(6);
            regionProfile.DocumentsCount = documents.Count();

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

        public async Task<IEnumerable<RegionFollowerDTO>> GetFollowersAsync(int regionId)
        {
            var followers = await _repoWrapper.RegionFollowers.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<RegionFollowers>, IEnumerable<RegionFollowerDTO>>(followers);
        }

        public async Task<RegionFollowerDTO> GetFollowerAsync(int followerId)
        {
            var follower = await _repoWrapper.RegionFollowers.GetFirstOrDefaultAsync(d => d.ID == followerId);
            return _mapper.Map<RegionFollowers, RegionFollowerDTO>(follower);
        }

        public async Task CreateFollowerAsync(RegionFollowerDTO model)
        {
            await _repoWrapper.RegionFollowers.CreateAsync(_mapper.Map<RegionFollowerDTO, RegionFollowers>(model));
            await _repoWrapper.SaveAsync();
        }

        public async Task RemoveFollowerAsync(int followerId)
        {
            var follower = await _repoWrapper.RegionFollowers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);

            _repoWrapper.RegionFollowers.Delete(follower);
            await _repoWrapper.SaveAsync();
        }

        public async Task<RegionProfileDTO> GetRegionByNameAsync(string Name, User user)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name);
            region.Documents = (await _repoWrapper.RegionDocument.GetAllAsync(d => d.RegionId == region.ID))?.ToList();
            var regionProfile = _mapper.Map<DataAccessRegion.Region, RegionProfileDTO>(region);
            var userRoles = await _userManager.GetRolesAsync(user);
            regionProfile.CanEdit = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.OkrugaHead) || userRoles.Contains(Roles.OkrugaHeadDeputy);
            return regionProfile;
        }

        public async Task<RegionDTO> GetRegionByNameAsync(string Name)
        {
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
            var documentDtos = _mapper.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDTO>>(documents);
            return DocumentsSorter<RegionDocumentDTO>.SortDocumentsBySubmitDate(documentDtos);
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

        public async Task ContinueAdminsDueToDate()
        {
            var admins = await _repoWrapper.RegionAdministration.GetAllAsync(x => x.Status);

            foreach (var admin in admins)
            {
                if (admin.EndDate != null && DateTime.Compare((DateTime)admin.EndDate, DateTime.Now) < 0)
                {
                    admin.EndDate = admin.EndDate.Value.AddYears(1);
                    _repoWrapper.RegionAdministration.Update(admin);
                }
            }
            await _repoWrapper.SaveAsync();

        }

        /// <inheritdoc />
        public async Task<IEnumerable<RegionForAdministrationDTO>> GetRegions()
        {
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionForAdministrationDTO>>((await _repoWrapper.Region
                .GetAllAsync()).Where(r => r.Status != RegionsStatusType.RegionBoard && r.IsActive));
        }
        /// <inheritdoc />
        public IEnumerable<RegionNamesDTO> GetActiveRegionsNames()
        {
            var regions = _repoWrapper.Region
                .GetActiveRegionsNames();
            var names = new List<DataAccessRegion.Region>();
            return _mapper.Map<IQueryable<DataAccessRegion.RegionNamesObject>, IEnumerable<RegionNamesDTO>>(regions);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RegionUserDTO>> GetRegionUsersAsync(int regionId)
        {
            var city = await _repoWrapper.CityMembers.GetAllAsync(d => d.City.RegionId == regionId && d.IsApproved, 
                include: source => source
                    .Include(t => t.User));
            var users = city.Select(x => x.User);
            return _mapper.Map<IEnumerable<DataAccessRegion.User>, IEnumerable<RegionUserDTO>>(users);
        }
        public async Task UnArchiveRegionAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId && !d.IsActive);
            region.IsActive = true;
            _repoWrapper.Region.Update(region);
            await _repoWrapper.SaveAsync();

        }

        public async Task<bool> CheckIfRegionNameExistsAsync(string name)
        {
            var result = await _repoWrapper.Region.GetFirstOrDefaultAsync(x => x.RegionName == name);
            return result != null;
        }
    }
}
