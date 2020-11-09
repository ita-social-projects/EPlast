using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.BLL.Services.Club.ClubAccess
{
    public class ClubAccessService:IClubAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, IClubAccessGetter> _ClubAccessGetters;

        public ClubAccessService(ClubAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _ClubAccessGetters = settings.ClubAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubDTO>> GetClubsAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _ClubAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    var cities = await _ClubAccessGetters[key].GetClubs(user.Id);
                    return _mapper.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(cities);
                }
            }
            return Enumerable.Empty<ClubDTO>();
        }

        public async Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int ClubId)
        {
            var cities = await this.GetClubsAsync(claimsPrincipal);
            return cities.Any(c => c.ID == ClubId);
        }

    }
}
