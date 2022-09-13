using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.CityClub;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccessCity = EPlast.DataAccess.Entities;

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

        public CityService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IWebHostEnvironment env,
            ICityBlobStorageRepository cityBlobStorage,
            ICityAccessService cityAccessService,
            UserManager<DataAccessCity.User> userManager
        )
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _cityBlobStorage = cityBlobStorage;
            _cityAccessService = cityAccessService;
            _userManager = userManager;
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
        public async Task<IEnumerable<CityDto>> GetAllCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(await GetAllAsync(cityName));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityDto>> GetCitiesByRegionAsync(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(c => c.RegionId == regionId && c.IsActive);
            foreach (var city in cities)
            {
                var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(
                                  predicate: c => c.CityId == city.ID);
                city.CityMembers = cityMembers.ToList();
            }

            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(cities);
        }

        /// <inheritdoc />
        public async Task<CityDto> GetByIdAsync(int cityId)
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
            return _mapper.Map<DataAccessCity.City, CityDto>(city);
        }

        /// <inheritdoc />
        public async Task<CityDto> GetCityByIdAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: c => c.ID == cityId,
                include: source => source
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User));
            return _mapper.Map<DataAccessCity.City, CityDto>(city);
        }

        public CityAdministrationDto GetCityHead(CityDto city)
        {
            var cityHead = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead && a.Status);
            return cityHead;
        }

        public CityAdministrationDto GetCityHeadDeputy(CityDto city)
        {
            var cityHeadDeputy = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                    && a.Status);
            return cityHeadDeputy;
        }

        public List<CityAdministrationDto> GetCityAdmins(CityDto city)
        {
            var cityAdmins = city.CityAdministration
                .Where(a => a.Status)
                .ToList();
            return cityAdmins;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityUserDto>> GetCityUsersAsync(int cityId)
        {
            var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(d => d.CityId == cityId && d.IsApproved,
                include: source => source
                    .Include(t => t.User));
            var users = cityMembers.Select(x => x.User);
            return _mapper.Map<IEnumerable<DataAccessCity.User>, IEnumerable<CityUserDto>>(users);
        }

        /// <inheritdoc />
        public async Task<CityProfileDto> GetCityProfileAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            const int membersToShow = 9;
            const int followersToShow = 6;
            const int adminsToShow = 6;
            const int documentToShow = 6;
            var cityHead = GetCityHead(city);
            var cityHeadDeputy = GetCityHeadDeputy(city);
            var cityAdmins = city.CityAdministration
                .Where(a => a.Status)
                .Take(adminsToShow)
                .ToList();
            city.AdministrationCount = city.CityAdministration == null ? 0
                : city.CityAdministration.Count(a => a.Status);
            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .Take(membersToShow)
                .ToList();
            city.MemberCount = city.CityMembers
                .Count(m => m.IsApproved);
            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .Take(followersToShow)
                .ToList();
            city.FollowerCount = city.CityMembers
                .Count(m => !m.IsApproved);
            var cityDoc = city.CityDocuments.Take(documentToShow).ToList();
            city.DocumentsCount = city.CityDocuments.Count();

            var cityProfileDto = new CityProfileDto
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

        /// <inheritdoc />
        public async Task<CityProfileDto> GetCityProfileAsync(int cityId, DataAccessCity.User user)
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
        public async Task<CityProfileDto> GetCityMembersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var members = city.CityMembers
                .Where(m => m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Members = members
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<bool> PlastMemberCheck(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(user, Roles.PlastMember))
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<CityProfileDto> GetCityFollowersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var followers = city.CityMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Followers = followers
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<CityProfileDto> GetCityAdminsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityHead = GetCityHead(city);
            var cityHeadDeputy = GetCityHeadDeputy(city);
            var cityAdmins = GetCityAdmins(city);

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Admins = cityAdmins,
                Head = cityHead,
                HeadDeputy = cityHeadDeputy
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        public async Task<string> GetCityAdminsIdsAsync(int cityId)
        {
            var city = await GetCityByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityHead = GetCityHead(city);
            var cityHeadDeputy = GetCityHeadDeputy(city);

            var cityHeadId = cityHead != null ? cityHead.UserId : "No Id";
            var cityHeadDeputyId = cityHeadDeputy != null ? cityHeadDeputy.UserId : "No Id";

            return $"{cityHeadId},{cityHeadDeputyId}";
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityAdministrationGetDto>> GetAdministrationAsync(int cityId)
        {
            var admins = await _repoWrapper.CityAdministration.GetAllAsync(d => d.CityId == cityId && d.Status,
                include: source => source
                    .Include(t => t.User)
                    .Include(t => t.City)
                    .Include(t => t.AdminType));
            var admin = _mapper.Map<IEnumerable<DataAccessCity.CityAdministration>, IEnumerable<CityAdministrationGetDto>>(admins);
            return admin;
        }

        /// <inheritdoc />
        public async Task<CityProfileDto> GetCityDocumentsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }

            var cityDoc = DocumentsSorter<CityDocumentsDto>.SortDocumentsBySubmitDate(city.CityDocuments);

            var cityProfileDto = new CityProfileDto
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
        public async Task<CityProfileDto> EditAsync(int cityId)
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

            var cityProfileDto = new CityProfileDto
            {
                City = city,
                Admins = cityAdmins,
                Members = members,
                Followers = followers
            };

            return cityProfileDto;
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task EditAsync(CityProfileDto model, IFormFile file)
        {
            await UploadPhotoAsync(model.City, file);
            var city = await CreateCityAndRegionAsync(model);

            _repoWrapper.City.Attach(city);
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task EditAsync(CityDto model)
        {
            await UploadPhotoAsync(model);
            var city = await CreateCityAsync(model);

            _repoWrapper.City.Attach(city);
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task<int> CreateAsync(CityProfileDto model, IFormFile file)
        {
            await UploadPhotoAsync(model.City, file);
            var city = await CreateCityAndRegionAsync(model);

            _repoWrapper.City.Attach(city);
            await _repoWrapper.City.CreateAsync(city);
            await _repoWrapper.SaveAsync();

            return city.ID;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(CityDto model)
        {
            await UploadPhotoAsync(model);
            var city = await CreateCityAsync(model);

            _repoWrapper.City.Attach(city);
            await _repoWrapper.City.CreateAsync(city);
            await _repoWrapper.SaveAsync();

            return city.ID;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityForAdministrationDto>> GetCities()
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => c.IsActive);
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityForAdministrationDto>>(filteredCities);
        }

        // Method is redundant
        private async Task<DataAccessCity.City> CreateCityAndRegionAsync(CityProfileDto model)
        {
            var cityDto = model.City;

            var city = _mapper.Map<CityDto, DataAccessCity.City>(cityDto);
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

        // Method moved to be used with command/handler CreateCity
        private async Task<DataAccessCity.City> CreateCityAsync(CityDto model)
        {
            var city = _mapper.Map<CityDto, DataAccessCity.City>(model);
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.RegionName == city.Region.RegionName);

            city.RegionId = region.ID;
            city.Region = region;

            return city;
        }

        // Method is redundant
        private async Task UploadPhotoAsync(CityDto city, IFormFile file)
        {
            var cityId = city.ID;
            var oldImageName = (await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: i => i.ID == cityId))
                ?.Logo;

            city.Logo = GetChangedPhoto(
                "images\\Cities",
                file,
                oldImageName,
                _env.WebRootPath,
                Guid.NewGuid().ToString()
            );
        }

        // Method moved to be used with command/handler UploadCityPhoto
        private async Task UploadPhotoAsync(CityDto city)
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

                var fileName = $"{Guid.NewGuid()}{extension}";

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
            if (city.CityMembers is null && city.CityAdministration is null)
            {
                city.IsActive = false;
                _repoWrapper.City.Update(city);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task<IEnumerable<DataAccessCity.City>> GetAllActiveAsync(string cityName = null)
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => c.IsActive);
            return string.IsNullOrEmpty(cityName)
                ? filteredCities
                : filteredCities.Where(c => c.Name.ToLower().Contains(cityName.ToLower()));
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task<IEnumerable<DataAccessCity.City>> GetAllNotActiveAsync(string cityName = null)
        {
            var cities = await _repoWrapper.City.GetAllAsync();
            var filteredCities = cities.Where(c => !c.IsActive);
            return string.IsNullOrEmpty(cityName)
                ? filteredCities
                : filteredCities.Where(c => c.Name.ToLower().Contains(cityName.ToLower()));
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task<IEnumerable<CityDto>> GetAllActiveCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(await GetAllActiveAsync(cityName));
        }

        /// <inheritdoc />
        public async Task<Tuple<IEnumerable<CityObjectDto>, int>> GetAllCitiesByPageAndIsArchiveAsync(int page, int pageSize, string name, bool isArchive)
        {
            var tuple = await _repoWrapper.City.GetCitiesObjects(page, pageSize, name, isArchive);
            var cities = tuple.Item1;
            //get images from blob storage
            foreach (var city in cities)
            {
                try
                {
                    //If string is null or empty then image is default, and it is not stored in Blob storage :)
                    if (!string.IsNullOrEmpty(city.Logo))
                    {
                        city.Logo = await _cityBlobStorage.GetBlobBase64Async(city.Logo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot get image from blob storage because {ex}");
                }
            }
            var rows = tuple.Item2;
            return new Tuple<IEnumerable<CityObjectDto>, int>(_mapper.Map<IEnumerable<DataAccessCity.CityObject>, IEnumerable<CityObjectDto>>(cities), rows);
        }

        /// <inheritdoc />
        /// Method is redundant
        public async Task<IEnumerable<CityDto>> GetAllNotActiveCitiesAsync(string cityName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDto>>(await GetAllNotActiveAsync(cityName));
        }

        /// <inheritdoc />
        public async Task UnArchiveAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(c => c.ID == cityId && !c.IsActive);
            city.IsActive = true;
            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        public async Task<int> GetCityIdByUserIdAsync(string userId)
        {
            var city = await _repoWrapper.CityMembers.GetFirstAsync(user=>user.UserId == userId);
            return city.CityId;
        }
    }
}

