using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region.RegionAccess
{
    public class RegionAccessService : IRegionAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, IRegionAccessGetter> _regionAccessGetters;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public RegionAccessService(RegionAccessSettings settings, UserManager<DatabaseEntities.User> userManager,
            IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _regionAccessGetters = settings.RegionAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(DatabaseEntities.User claimsPrincipal)
        {
            var roles = await _userManager.GetRolesAsync(claimsPrincipal);
            var key = _regionAccessGetters.Keys.FirstOrDefault(x => roles.Contains(x));
            if(key != null)
            {
                var regions = await _regionAccessGetters[key].GetRegionAsync(claimsPrincipal.Id);
                return _mapper.Map<IEnumerable<DatabaseEntities.Region>, IEnumerable<RegionDTO>>(regions);
            }
            return Enumerable.Empty<RegionDTO>();
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User claimsPrincipal, int regionId)
        {
            var regions = await GetRegionsAsync(claimsPrincipal);
            return regions.Any(c => c.ID == regionId);
        }

        public async Task<IEnumerable<RegionForAdministrationDTO>> GetAllRegionsIdAndName(DatabaseEntities.User user)
        {
            IEnumerable<RegionForAdministrationDTO> options = Enumerable.Empty<RegionForAdministrationDTO>();
            var roles = await _userManager.GetRolesAsync(user);
            var reports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync();
            IEnumerable<(int regionId, int year)> regionsId = reports.Select(x => (x.RegionId, x.Date.Year)).ToList();
            var key = _regionAccessGetters.Keys.FirstOrDefault(x => roles.Contains(x));
            if(key != null)
            {
                options = _mapper.Map<IEnumerable<DatabaseEntities.Region>, IEnumerable<RegionForAdministrationDTO>>(
                    await _regionAccessGetters[key].GetRegionAsync(user.Id));
            }
            foreach (var item in options)
            {
                item.YearsHasReport = regionsId.Where(x => x.regionId == item.ID).Select(x => x.year).ToList();
            }

            return options;
        }
    }
}
