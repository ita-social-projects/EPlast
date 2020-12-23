using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.BLL.Services.Club.ClubAccess
{
    public class ClubAccessService:IClubAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, IClubAccessGetter> _clubAccessGetters;

        public ClubAccessService(ClubAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _clubAccessGetters = settings.ClubAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubDTO>> GetClubsAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _clubAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    var cities = await _clubAccessGetters[key].GetClubs(user.Id);
                    return _mapper.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubDTO>>(cities);
                }
            }
            return Enumerable.Empty<ClubDTO>();
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user, int ClubId)
        {
            var cities = await this.GetClubsAsync(user);
            return cities.Any(c => c.ID == ClubId);
        }

    }
}
