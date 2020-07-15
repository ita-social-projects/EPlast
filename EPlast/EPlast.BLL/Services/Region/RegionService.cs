using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region
{
    public class RegionService : IRegionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        //private readonly IRegionBlobStorageRepository _regionBlobStorage;
        private readonly ICityService _cityService;

        public RegionService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            ICityService cityService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _cityService = cityService;
        }

        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            var regions = await _repoWrapper.Region.GetAllAsync();

            return _mapper.Map<IEnumerable<DataAccessCity.Region>, IEnumerable<RegionDTO>>(regions);
        }

        public async Task<RegionDTO> GetRegionByIdAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.ID == regionId);

            return _mapper.Map<DataAccessCity.Region, RegionDTO>(region);
        }

        public async Task<RegionProfileDTO> GetRegionProfileByIdAsync(int regionId)
        {
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.ID == regionId);
            var regionProfile = _mapper.Map<DataAccessCity.Region, RegionProfileDTO>(region);

            var cities = await _cityService.GetCitiesByRegionAsync(regionId);
            regionProfile.Cities = cities;

            return regionProfile;
        }
    }
}
