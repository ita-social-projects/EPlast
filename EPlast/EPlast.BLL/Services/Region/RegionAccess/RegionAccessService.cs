using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Region.RegionAccess
{
    public class RegionAccessService : IRegionAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, IRegionAccessGetter> _regionAccessGetters;

        public RegionAccessService(RegionAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _regionAccessGetters = settings.RegionAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _regionAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    var regions = await _regionAccessGetters[key].GetRegionAsync(user.Id);
                    return _mapper.Map<IEnumerable<DatabaseEntities.Region>, IEnumerable<RegionDTO>>(regions);
                }
            }
            return Enumerable.Empty<RegionDTO>();
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User claimsPrincipal, int regionId)
        {
            var regions = await GetRegionsAsync(claimsPrincipal);
            return regions.Any(c => c.ID == regionId);
        }
    }
}
