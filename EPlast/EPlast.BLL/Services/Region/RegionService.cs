using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccessRegion = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region
{
    public class RegionService : IRegionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IRegionBlobStorageRepository _regionBlobStorage;
        private readonly IRegionFilesBlobStorageRepository _regionFilesBlobStorageRepository;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public RegionService(
            IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IRegionFilesBlobStorageRepository regionFilesBlobStorageRepository,
            IRegionBlobStorageRepository regionBlobStorage,
            IMediator mediator,
            UserManager<User> userManager
        )
        {
            _regionFilesBlobStorageRepository = regionFilesBlobStorageRepository;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _regionBlobStorage = regionBlobStorage;
            _mediator = mediator;
            _userManager = userManager;
        }
        public async Task ArchiveRegionAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId && d.IsActive);
            var followers = await _repoWrapper.RegionFollowers.GetAllAsync(d => d.RegionId == regionId);
            if (region.Cities is null && region.RegionAdministration is null && !followers.Any())
            {
                region.IsActive = false;
                _repoWrapper.Region.Update(region);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public async Task AddRegionAsync(RegionDto region)
        {
            await _repoWrapper.Region.CreateAsync(_mapper.Map<RegionDto, DataAccessRegion.Region>(region));

            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard);
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionDto>>(filteredRegions);
        }
        public async Task<IEnumerable<RegionDto>> GetAllActiveRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
               include: source => source
                   .Include(r => r.RegionAdministration)
                       .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard && r.IsActive);
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionDto>>(filteredRegions);
        }
        public async Task<Tuple<IEnumerable<RegionObjectsDto>, int>> GetAllRegionsByPageAndIsArchiveAsync(int page, int pageSize, string regionName, bool isArchive)
        {
            var tuple = await _repoWrapper.Region.GetRegionsObjects(page, pageSize, regionName, isArchive);
            var regions = tuple.Item1;
            //get images from blob storage
            foreach (var region in regions)
            {
                try
                {
                    //If string is null or empty then image is default, and it is not stored in Blob storage :)
                    if (!string.IsNullOrEmpty(region.Logo))
                    {
                        region.Logo = await _regionBlobStorage.GetBlobBase64Async(region.Logo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot get image from blob storage because {ex}");
                }
            }
            var rows = tuple.Item2;
            return new Tuple<IEnumerable<RegionObjectsDto>, int>(_mapper.Map<IEnumerable<RegionObject>, IEnumerable<RegionObjectsDto>>(regions), rows);
        }

        public async Task<IEnumerable<RegionDto>> GetAllNotActiveRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
              include: source => source
                  .Include(r => r.RegionAdministration)
                      .ThenInclude(t => t.AdminType));
            var filteredRegions = regions.Where(r => r.Status != RegionsStatusType.RegionBoard && !r.IsActive);
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionDto>>(filteredRegions);
        }

        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _regionBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        public async Task<RegionDto> GetRegionByIdAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(
                predicate: r => r.ID == regionId,
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));

            return _mapper.Map<DataAccessRegion.Region, RegionDto>(region);
        }

        public async Task<RegionProfileDto> GetRegionProfileByIdAsync(int regionId, User user)
        {
            var region = await GetRegionByIdAsync(regionId);
            try
            {
                //If string is null or empty then image is default, and it is not stored in Blob storage :)
                if (!string.IsNullOrEmpty(region.Logo))
                {
                    region.Logo = await _regionBlobStorage.GetBlobBase64Async(region.Logo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not get image from blob storage because {ex}");
            }
            var regionProfile = _mapper.Map<RegionDto, RegionProfileDto>(region);
            var userRoles = await _userManager.GetRolesAsync(user);
            var documents = await _repoWrapper
                .RegionDocument
                .GetAllAsync(d => d.RegionId == regionId);
            var query = new GetCitiesByRegionQuery(regionId);
            var cities = await _mediator.Send(query);
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

        public async Task<IEnumerable<CityDto>> GetMembersAsync(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDto>>(cities);
        }

        public async Task<IEnumerable<RegionFollowerDto>> GetFollowersAsync(int regionId)
        {
            var followers = await _repoWrapper.RegionFollowers.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<RegionFollowers>, IEnumerable<RegionFollowerDto>>(followers);
        }

        public async Task<RegionFollowerDto> GetFollowerAsync(int followerId)
        {
            var follower = await _repoWrapper.RegionFollowers.GetFirstOrDefaultAsync(d => d.ID == followerId);
            return _mapper.Map<RegionFollowers, RegionFollowerDto>(follower);
        }

        public async Task<int> CreateFollowerAsync(RegionFollowerDto model)
        {
            var follower = _mapper.Map<RegionFollowerDto, RegionFollowers>(model);
            await _repoWrapper.RegionFollowers.CreateAsync(follower);
            await _repoWrapper.SaveAsync();
            return follower.ID;
        }

        public async Task RemoveFollowerAsync(int followerId)
        {
            var follower = await _repoWrapper.RegionFollowers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);

            _repoWrapper.RegionFollowers.Delete(follower);
            await _repoWrapper.SaveAsync();
        }

        public async Task<RegionProfileDto> GetRegionByNameAsync(string Name, User user)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name);
            region.Documents = (await _repoWrapper.RegionDocument.GetAllAsync(d => d.RegionId == region.ID))?.ToList();
            var regionProfile = _mapper.Map<DataAccessRegion.Region, RegionProfileDto>(region);
            var userRoles = await _userManager.GetRolesAsync(user);
            regionProfile.CanEdit = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.OkrugaHead) || userRoles.Contains(Roles.OkrugaHeadDeputy);
            return regionProfile;
        }

        public async Task<RegionDto> GetRegionByNameAsync(string Name)
        {
            return _mapper.Map<DataAccessRegion.Region, RegionDto>(await _repoWrapper.Region.GetFirstAsync(d => d.RegionName == Name));
        }

        private async Task<string> UploadPhotoAsyncFromBase64(int regionId, string imageBase64)
        {
            var oldImageName = (await _repoWrapper.Region.GetFirstOrDefaultAsync(x => x.ID == regionId)).Logo;
            if (!string.IsNullOrWhiteSpace(imageBase64))
            {
                var base64Parts = imageBase64.Split(',');
                var ext = base64Parts[0].Split(new[] { '/', ';' }, 3)[1];
                var fileName = $"{Guid.NewGuid()}.{ext}";
                await _regionBlobStorage.UploadBlobForBase64Async(base64Parts[1], fileName);
                if (!string.IsNullOrEmpty(oldImageName))
                {
                    try
                    {
                        await _regionBlobStorage.DeleteBlobAsync(oldImageName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot delete image because {ex}");
                    }
                }
                return fileName;
            }
            else
            {
                return null;
            }
        }

        public async Task EditRegionAsync(int regId, RegionDto region)
        {
            var ChangedRegion = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regId);
            ChangedRegion.Logo = await UploadPhotoAsyncFromBase64(regId, region.Logo);
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

        private void ValidateFileName(RegionDocumentDto documentDTO, out string[] splittedName)
        {
            var allowedExtensions = new List<string>() { "pdf", "doc", "docx" };

            var dotIndex = documentDTO.FileName.LastIndexOf('.');
            if (dotIndex == -1)
            {
                throw new ArgumentException(@"The file must have 'pdf', 'doc' or 'docx' extension");
            }

            var fileName = documentDTO.FileName[..dotIndex].Trim();
            if (fileName == string.Empty)
            {
                throw new ArgumentException("The file name cannot be empty");
            }

            var extension = documentDTO.FileName[(dotIndex + 1)..];
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException(@"The extension must be 'pdf', 'doc' or 'docx' format");
            }
            splittedName = new[] { fileName, extension };
        }
        public async Task<RegionDocumentDto> AddDocumentAsync(RegionDocumentDto documentDTO)
        {
            var fileBase64 = documentDTO.BlobName.Split(',')[1];
            ValidateFileName(documentDTO, out string[] splittedName);
            var fileName = $"{Guid.NewGuid()}.{splittedName.Last()}";
            await _regionFilesBlobStorageRepository.UploadBlobForBase64Async(fileBase64, fileName);
            documentDTO.BlobName = fileName;

            var document = _mapper.Map<RegionDocumentDto, RegionDocuments>(documentDTO);
            _repoWrapper.RegionDocument.Attach(document);
            await _repoWrapper.RegionDocument.CreateAsync(document);
            await _repoWrapper.SaveAsync();

            return documentDTO;
        }

        public async Task<IEnumerable<RegionDocumentDto>> GetRegionDocsAsync(int regionId)
        {
            var documents = await _repoWrapper.RegionDocument.GetAllAsync(d => d.RegionId == regionId);
            var documentDtos = _mapper.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDto>>(documents);
            return DocumentsSorter<RegionDocumentDto>.SortDocumentsBySubmitDate(documentDtos);
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
        public async Task<IEnumerable<RegionForAdministrationDto>> GetRegions()
        {
            return _mapper.Map<IEnumerable<DataAccessRegion.Region>, IEnumerable<RegionForAdministrationDto>>((await _repoWrapper.Region
                .GetAllAsync()).Where(r => r.Status != RegionsStatusType.RegionBoard && r.IsActive));
        }

        /// <inheritdoc />
        public IEnumerable<RegionNamesDto> GetActiveRegionsNames()
        {
            var regions = _repoWrapper.Region
                .GetActiveRegionsNames();
            return _mapper.Map<IQueryable<DataAccessRegion.RegionNamesObject>, IEnumerable<RegionNamesDto>>(regions);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RegionUserDto>> GetRegionUsersAsync(int regionId)
        {
            var city = await _repoWrapper.CityMembers.GetAllAsync(d => d.City.RegionId == regionId && d.IsApproved,
                include: source => source
                    .Include(t => t.User));
            var users = city.Select(x => x.User);
            return _mapper.Map<IEnumerable<DataAccessRegion.User>, IEnumerable<RegionUserDto>>(users);
        }
        public async Task UnArchiveRegionAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstAsync(d => d.ID == regionId && !d.IsActive);
            region.IsActive = true;
            _repoWrapper.Region.Update(region);
            await _repoWrapper.SaveAsync();

        }

        /// <inheritdoc />
        public async Task<bool> CheckIfRegionNameExistsAsync(string name)
        {
            var result = await _repoWrapper.Region.GetFirstOrDefaultAsync(x => x.RegionName == name);
            return result != null;
        }
    }
}
