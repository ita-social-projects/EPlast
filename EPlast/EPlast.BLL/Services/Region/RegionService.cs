using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region
{
    public class RegionService : IRegionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IRegionBlobStorageRepository _regionBlobStorage;
        private readonly IRegionFilesBlobStorageRepository _regionFilesBlobStorageRepository;
        private readonly ICityService _cityService;
        private readonly IAdminTypeService _adminTypeService;
        private readonly UserManager<User> _userManager;

        public RegionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IRegionFilesBlobStorageRepository regionFilesBlobStorageRepository,
            IRegionBlobStorageRepository regionBlobStorage,
            ICityService cityService,
            IAdminTypeService adminTypeService,
            UserManager<User> userManager)
        {
            _regionFilesBlobStorageRepository = regionFilesBlobStorageRepository;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _regionBlobStorage = regionBlobStorage;
            _cityService = cityService;
            _adminTypeService = adminTypeService;
            _userManager = userManager;
        }

        public async Task AddRegionAsync(RegionDTO region)
        {

            var newRegion = _mapper.Map<RegionDTO, DataAccessCity.Region>(region);

            await _repoWrapper.Region.CreateAsync(newRegion);
            await _repoWrapper.SaveAsync();
        }


        public async Task AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO)
        {
            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(regionAdministrationDTO.AdminTypeId);
            var newRegionAdmin = new RegionAdministration()
            {
                StartDate = regionAdministrationDTO.StartDate ?? DateTime.Now,
                EndDate = regionAdministrationDTO.EndDate,
                AdminTypeId = adminType.ID,
                RegionId = regionAdministrationDTO.RegionId,
                UserId = regionAdministrationDTO.UserId
            };

            var oldAdmin = await _repoWrapper.RegionAdministration.
                GetFirstOrDefaultAsync(d => d.AdminTypeId == newRegionAdmin.AdminTypeId 
                && d.RegionId == newRegionAdmin.RegionId && d.Status);

            var newUser = await _userManager.FindByIdAsync(newRegionAdmin.UserId);
            
            var role = adminType.AdminTypeName == "Голова Округу" ? "Голова Округу" : "Діловод Округу";
            await _userManager.AddToRoleAsync(newUser, role);

            if (oldAdmin != null)
            {
                if (DateTime.Now < newRegionAdmin.EndDate || newRegionAdmin.EndDate == null)
                {
                    newRegionAdmin.Status = true;
                    oldAdmin.Status = false;
                }
                else
                {
                    newRegionAdmin.Status = false;
                }
                var oldUser = await _userManager.FindByIdAsync(oldAdmin.UserId);
                await _userManager.RemoveFromRoleAsync(oldUser, role);
                _repoWrapper.RegionAdministration.Update(oldAdmin);
                await _repoWrapper.SaveAsync();
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                newRegionAdmin.Status = true;
                await _repoWrapper.SaveAsync();
                await _repoWrapper.RegionAdministration.CreateAsync(newRegionAdmin);
                await _repoWrapper.SaveAsync();
            }
        }


        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));

            return _mapper.Map<IEnumerable<DataAccessCity.Region>, IEnumerable<RegionDTO>>(regions);
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

            return _mapper.Map<DataAccessCity.Region, RegionDTO>(region);
        }

        public async Task<RegionProfileDTO> GetRegionProfileByIdAsync(int regionId)
        {
            var region = await GetRegionByIdAsync(regionId);
            var regionProfile = _mapper.Map<RegionDTO, RegionProfileDTO>(region);

            var cities = await _cityService.GetCitiesByRegionAsync(regionId);
            regionProfile.Cities = cities;
            regionProfile.City = region.City;

            return regionProfile;
        }

        public async Task DeleteRegionByIdAsync(int regionId)
        {
            var region = (await _repoWrapper.Region.GetFirstAsync(d=>d.ID == regionId));
            _repoWrapper.Region.Delete(region);
            await _repoWrapper.SaveAsync();
        }


        /// <inheritdoc />
        public async Task AddFollowerAsync(int regionId, int cityId)
        {
            var region = (await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId));
            var city = (await _repoWrapper.City.GetFirstAsync(d=>d.ID==cityId));

            city.Region = region;
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<CityDTO>> GetMembersAsync(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(cities);
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationAsync(int regionId)
        {
            var admins =await  _repoWrapper.RegionAdministration.GetAllAsync(d => d.RegionId == regionId && d.Status,
                include: source => source
                    .Include(t => t.User)
                        .Include(t => t.Region)
                        .Include(t => t.AdminType));
            return _mapper.Map< IEnumerable < RegionAdministration >,IEnumerable< RegionAdministrationDTO>> (admins);
        }

        public async Task<RegionDTO> GetRegionByNameAsync(string Name)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name);
            return _mapper.Map<DataAccessCity.Region, RegionDTO>(region);
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

        public async Task<int> GetAdminType(string name)
        {
            var type = await _repoWrapper.AdminType.GetFirstAsync(a=>a.AdminTypeName==name);
            return type.ID;
        }

        public async Task DeleteAdminByIdAsync(int Id)
        {
            var Admin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.ID == Id);
            var adminType = await _adminTypeService.GetAdminTypeByIdAsync(Admin.AdminTypeId);

            var user = await _userManager.FindByIdAsync(Admin.UserId);
            var role = adminType.AdminTypeName == "Голова Округу" ? "Голова Округу" : "Діловод Округу";
            await _userManager.RemoveFromRoleAsync(user, role);

            _repoWrapper.RegionAdministration.Delete(Admin);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId&& a.Status,
                include:source=>source
                .Include(r=>r.User)
                .Include(r=>r.Region)
                .Include(r=>r.AdminType));
            return _mapper.Map< IEnumerable < RegionAdministration > ,IEnumerable <RegionAdministrationDTO>>(secretaries);
        }


        public async Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId)
        {
            var secretaries = await _repoWrapper.RegionAdministration.GetAllAsync(a => a.UserId == userId && !a.Status,
                include: source => source
                 .Include(r => r.User)
                 .Include(r => r.Region)
                 .Include(r => r.AdminType));
            return _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(secretaries);
        }

        public async Task<RegionDocumentDTO> AddDocumentAsync(RegionDocumentDTO documentDTO)
         {
             var fileBase64 = documentDTO.BlobName.Split(',')[1];
             var extension = "." + documentDTO.FileName.Split('.').LastOrDefault();
             var fileName = Guid.NewGuid() + extension;
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
            var documents =await  _repoWrapper.RegionDocument.GetAllAsync(d => d.RegionId == regionId);
            return  _mapper.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDTO>> (documents);
        }


        public async Task<string> DownloadFileAsync(string fileName)
        {
            var fileBase64 = await _regionFilesBlobStorageRepository.GetBlobBase64Async(fileName);

            return fileBase64;
        }


        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repoWrapper.RegionDocument
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _regionFilesBlobStorageRepository.DeleteBlobAsync(document.BlobName);

            _repoWrapper.RegionDocument.Delete(document);
            await _repoWrapper.SaveAsync();
        }

        public async Task<RegionAdministrationDTO> GetHead(int regionId)
        {
            var head = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(d => d.RegionId == regionId && d.AdminType.AdminTypeName == "Голова Округу" && DateTime.Compare((DateTime)d.EndDate, DateTime.Now)>0&& d.Status,
                include: source => source
                .Include(
                d => d.User));

            return _mapper.Map<RegionAdministration, RegionAdministrationDTO>(head);
        }

        public async Task RedirectMembers(int prevRegId, int nextRegId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == prevRegId);
            foreach(var city in cities)
            {
                city.RegionId = nextRegId;
                _repoWrapper.City.Update(city);

            }
            await _repoWrapper.SaveAsync();

        }

        public async Task EndAdminsDueToDate()
        {
            var admins = await _repoWrapper.RegionAdministration.GetAllAsync();

            foreach(var admin in admins)
            {
                if(DateTime.Compare((DateTime)admin.EndDate,DateTime.Now) < 0)
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
            var regions = await _repoWrapper.Region.GetAllAsync();
            return _mapper.Map<IEnumerable<DataAccessCity.Region>, IEnumerable<RegionForAdministrationDTO>>(regions);
        }

        public async Task<IEnumerable<AdminTypeDTO>> GetAllAdminTypes()
        {
            var types =await  _repoWrapper.AdminType.GetAllAsync();
            return _mapper.Map<IEnumerable<AdminType>, IEnumerable<AdminTypeDTO>>(types);
        }
    }
}
