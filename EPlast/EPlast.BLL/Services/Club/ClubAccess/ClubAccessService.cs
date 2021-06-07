using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.BLL.Services.Club.ClubAccess
{
    public class ClubAccessService:IClubAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, IClubAccessGetter> _clubAccessGetters;

        public ClubAccessService(IRepositoryWrapper repositoryWrapper, ClubAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
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

        public async Task<IEnumerable<ClubForAdministrationDTO>> GetAllClubsIdAndName(DatabaseEntities.User user)
        {
            IEnumerable<ClubForAdministrationDTO> options = Enumerable.Empty<ClubForAdministrationDTO>();
            var roles = await _userManager.GetRolesAsync(user);
            var clubsId =
                (await _repositoryWrapper.ClubAnnualReports.GetAllAsync(predicate: x => x.Date.Year == DateTime.Now.Year))
                .Select(x => x.ClubId).ToList();
            foreach (var key in _clubAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    options = _mapper.Map<IEnumerable<DatabaseEntities.Club>, IEnumerable<ClubForAdministrationDTO>>(
                        await _clubAccessGetters[key].GetClubs(user.Id));
                }
            }
            foreach (var item in options)
            {
                item.HasReport = clubsId.Any(x => x == item.ID);
            }

            return options;
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user, int ClubId)
        {
            var clubs = await this.GetClubsAsync(user);
            return clubs.Any(c => c.ID == ClubId);
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in roles)
            {
                if (Roles.HeadsAndAdmin.Contains(key))
                    return true;
            }
            return false;
        }

    }
}
