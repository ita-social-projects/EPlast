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

        private readonly Dictionary<string, IRegionAccessGetter> _RegionAccessGetters;
        public RegionAccessService(RegionAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _RegionAccessGetters = settings.RegionAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _RegionAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    var cities = await _RegionAccessGetters[key].GetRegion(user.Id);
                    return _mapper.Map<IEnumerable<DatabaseEntities.Region>, IEnumerable<RegionDTO>>(cities);
                }
            }
            return Enumerable.Empty<RegionDTO>();
        }

        public async Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int RegionId)
        {
            var cities = await this.GetRegionsAsync(claimsPrincipal);
            return cities.Any(c => c.ID == RegionId);
        }
    }
}
