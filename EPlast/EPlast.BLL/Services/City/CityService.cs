using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Services.CityClub;
using DataAccessCity = EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services
{
    public class CityService : CityClubBase, ICityService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ICityBlobStorageRepository _cityBlobStorage;
        private readonly ICityAccessService _cityAccessService;
        private readonly UserManager<DataAccessCity.User> _userManager; 
        private readonly IUniqueIdService _uniqueId;

        public CityService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IWebHostEnvironment env,
            ICityBlobStorageRepository cityBlobStorage,
            ICityAccessService cityAccessService,
            UserManager<DataAccessCity.User> userManager,
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _cityBlobStorage = cityBlobStorage;
            _cityAccessService = cityAccessService;
            _userManager = userManager;
            _uniqueId = uniqueId;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DataAccessCity.City>> GetAllAsync(string cityName = null)
        {
            var cities = await _repoWrapper.City.GetAllAsync();

            return string.IsNullOrEmpty(cityName)
                ? cities
                : cities.Where(c => c.Name.ToLower().Contains(cityName.ToLower()));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityDTO>> GetAllCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(await GetAllAsync(cityName));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityDTO>> GetCitiesByRegionAsync(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(c => c.RegionId == regionId && c.IsActive);
            foreach (var city in cities)
            {
                var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(
                                  predicate: c => c.CityId == city.ID);
                city.CityMembers = cityMembers.ToList();
            }

            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(cities);
        }

        /// <inheritdoc />
        public async Task<CityDTO> GetByIdAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: c => c.ID == cityId,
                include: source => source
                    .Include(l => l.CityDocuments)
                        .ThenInclude(d => d.CityDocumentType)
                    .Include(r => r.Region)
                    .Include(c => c.CityAdministration)
                        .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                        .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                        .ThenInclude(u => u.User));
            return _mapper.Map<DataAccessCity.City, CityDTO>(city);
        }

        public CityAdministrationDTO GetCityHead(CityDTO city)
        {
            var cityHead = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead && a.Status);
            return cityHead;
        }

        public CityAdministrationDTO GetCityHeadDeputy(CityDTO city)
        {
            var cityHeadDeputy = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                    && a.Status);
            return cityHeadDeputy;
        }

        public List<CityAdministrationDTO> GetCityAdmins(CityDTO city)
        {
            var cityAdmins = city.CityAdministration
                .Where(a => a.Status)
                .Take(6)
                .ToList();
            return cityAdmins;
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityProfileAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityHead = GetCityHead(city);
            var cityHeadDeputy = GetCityHeadDeputy(city);
            var cityAdmins = GetCityAdmins(city);
            city.AdministrationCount = city.CityAdministration == null ? 0
                : city.CityAdministration.Count(a => a.Status);
            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .Take(9)
                .ToList();
            city.MemberCount = city.CityMembers
                .Count(m => m.IsApproved);
            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .Take(6)
                .ToList();
            city.FollowerCount = city.CityMembers
                .Count(m => !m.IsApproved);
            var cityDoc = city.CityDocuments.Take(6).ToList();
            city.DocumentsCount = city.CityDocuments.Count();
            
            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy,
                Members = members,
                Followers = followers,
                Admins = cityAdmins,
                Documents = cityDoc,
            };

            return cityProfileDto;
        }
        public async Task<IEnumerable<CityUserDTO>> GetCityUsersAsync(int cityId)
        {
            var city = await _repoWrapper.CityMembers.GetAllAsync(d => d.CityId == cityId,
                include: source => source
                    .Include(t => t.User));
            var users = city.Select(x => x.User);
            return _mapper.Map<IEnumerable<DataAccessCity.User>, IEnumerable<CityUserDTO>>(users);
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityProfileAsync(int cityId, DataAccessCity.User user)
        {
            var cityProfileDto = await GetCityProfileAsync(cityId);
            var userId = await _userManager.GetUserIdAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            cityProfileDto.City.CanCreate = userRoles.Contains(Roles.Admin);
            cityProfileDto.City.CanEdit = await _cityAccessService.HasAccessAsync(user, cityId);
            cityProfileDto.City.CanJoin = (await _repoWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.User.Id == userId && u.CityId == cityId)) == null;

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityMembersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Members = members
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityFollowersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Followers = followers
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityAdminsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityHead = GetCityHead(city);
            var cityHeadDeputy = GetCityHeadDeputy(city);
            var cityAdmins = GetCityAdmins(city);

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Admins = cityAdmins,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> GetCityDocumentsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityDoc = DocumentsSorter<CityDocumentsDTO>.SortDocumentsBySubmitDate(city.CityDocuments);

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Documents = cityDoc.ToList()
            };

            return cityProfileDto;
        }


        /// <inheritdoc />
        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _cityBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == cityId);

            if (city.Logo != null)
            {
                await _cityBlobStorage.DeleteBlobAsync(city.Logo);
            }

            _repoWrapper.City.Delete(city);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<CityProfileDTO> EditAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityAdmins = city.CityAdministration
                .ToList();
            var members = city.CityMembers
                .Where(p => cityAdmins.All(a => a.UserId != p.UserId))
                .Where(m => m.IsApproved)
                .ToList();
            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDTO
            {
                City = city,
                Admins = cityAdmins,
                Members = members,
                Followers = followers
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task EditAsync(CityProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.City, file);
            var city = await CreateCityAndRegionAsync(model);

            _repoWrapper.City.Attach(city);
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task EditAsync(CityDTO model)
        {
            await UploadPhotoAsync(model);
            var city = await CreateCityAsync(model);

            _repoWrapper.City.Attach(city);
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(CityProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.City, file);
            var city = await CreateCityAndRegionAsync(model);

            _repoWrapper.City.Attach(city);
            await _repoWrapper.City.CreateAsync(city);
            await _repoWrapper.SaveAsync();

            return city.ID;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(CityDTO model)
        {
            await UploadPhotoAsync(model);
            var city = await CreateCityAsync(model);

            _repoWrapper.City.Attach(city);
            await _repoWrapper.City.CreateAsync(city);
            await _repoWrapper.SaveAsync();

            return city.ID;
        }


        /// <inheritdoc />
        public async Task<IEnumerable<CityForAdministrationDTO>> GetCities()
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => c.IsActive);
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityForAdministrationDTO>>(filteredCities);
        }

        private async Task<DataAccessCity.City> CreateCityAndRegionAsync(CityProfileDTO model)
        {
            var cityDto = model.City;

            var city = _mapper.Map<CityDTO, DataAccessCity.City>(cityDto);
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.RegionName == city.Region.RegionName);

            if (region == null)
            {
                region = new DataAccessCity.Region
                {
                    RegionName = city.Region.RegionName
                };

                await _repoWrapper.Region.CreateAsync(region);
                await _repoWrapper.SaveAsync();
            }

            city.RegionId = region.ID;
            city.Region = region;

            return city;
        }

        private async Task<DataAccessCity.City> CreateCityAsync(CityDTO model)
        {
            var city = _mapper.Map<CityDTO, DataAccessCity.City>(model);
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.RegionName == city.Region.RegionName);

            city.RegionId = region.ID;
            city.Region = region;

            return city;
        }

        private async Task UploadPhotoAsync(CityDTO city, IFormFile file)
        {
            var cityId = city.ID;
            var oldImageName = (await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: i => i.ID == cityId))
                ?.Logo;

            city.Logo = GetChangedPhoto("images\\Cities", file, oldImageName, _env.WebRootPath, _uniqueId.GetUniqueId().ToString());
        }

        private async Task UploadPhotoAsync(CityDTO city)
        {
            var oldImageName = (await _repoWrapper.City.GetFirstOrDefaultAsync(i => i.ID == city.ID))?.Logo;
            var logoBase64 = city.Logo;

            if (!string.IsNullOrWhiteSpace(logoBase64) && logoBase64.Length > 0)
            {
                var logoBase64Parts = logoBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

                await _cityBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                city.Logo = fileName;
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                await _cityBlobStorage.DeleteBlobAsync(oldImageName);
            }
        }

        public async Task ArchiveAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == cityId && c.IsActive);
            city.IsActive = false;
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<DataAccessCity.City>> GetAllActiveAsync(string cityName = null)
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => c.IsActive);
            return string.IsNullOrEmpty(cityName)
                ? filteredCities
                : filteredCities.Where(c => c.Name.ToLower().Contains(cityName.ToLower()));
        }

        public async Task<IEnumerable<DataAccessCity.City>> GetAllNotActiveAsync(string cityName = null)
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => !c.IsActive);
            return string.IsNullOrEmpty(cityName)
                ? filteredCities
                : filteredCities.Where(c => c.Name.ToLower().Contains(cityName.ToLower()));
        }

        public async Task<IEnumerable<CityDTO>> GetAllActiveCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(await GetAllActiveAsync(cityName));
        }

        public async Task<IEnumerable<CityDTO>> GetAllNotActiveCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(await GetAllNotActiveAsync(cityName));
        }

        public async Task UnArchiveAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == cityId && !c.IsActive);
            city.IsActive = true;
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }
    }
}

