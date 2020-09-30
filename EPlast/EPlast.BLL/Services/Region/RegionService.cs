using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region
{
    public class RegionService : IRegionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IRegionBlobStorageRepository _regionBlobStorage;
        private readonly ICityService _cityService;

        public RegionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IRegionBlobStorageRepository regionBlobStorage,
            ICityService cityService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _regionBlobStorage = regionBlobStorage;
            _cityService = cityService;
        }

        public async Task AddRegion(RegionDTO region)
        {

            var newRegion = _mapper.Map<RegionDTO, DataAccessCity.Region>(region);

            await _repoWrapper.Region.CreateAsync(newRegion);
            await _repoWrapper.SaveAsync();
        }


        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync(
                include: source => source
                    .Include(r => r.RegionAdministration)
                        .ThenInclude(t => t.AdminType));

            return _mapper.Map<IEnumerable<DataAccessCity.Region>, IEnumerable<RegionDTO>>(regions);
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

            return regionProfile;
        }

        public async Task DeleteRegionByIdAsync(int regionId)
        {
            var region = (await _repoWrapper.Region.GetFirstAsync(d=>d.ID == regionId));
            _repoWrapper.Region.Delete(region);
            await _repoWrapper.SaveAsync();
        }


        /// <inheritdoc />
        public async Task AddFollowerAsync(int RegionId, int cityId)
        {
            var region = (await _repoWrapper.Region.GetFirstAsync(d => d.ID == RegionId));
            var city = (await _repoWrapper.City.GetFirstAsync(d=>d.ID==cityId));

            city.Region = region;
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<CityDTO>> GetMembers(int regionId)
        {
            var cities = await _repoWrapper.City.GetAllAsync(d => d.RegionId == regionId);
            return _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(cities);
        }

        public async Task<IEnumerable<RegionAdministrationDTO>> GetAdministration(int regionId)
        {
            var admins =await  _repoWrapper.RegionAdministration.GetAllAsync(d => d.RegionId == regionId,
                include: source => source
                    .Include(t => t.User)
                        .Include(t => t.Region)
                        .Include(t => t.AdminType));
            return _mapper.Map< IEnumerable < RegionAdministration >,IEnumerable< RegionAdministrationDTO>> (admins);
        }

        
    }
}
