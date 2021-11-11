using AutoMapper;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserProfileAccessService : IUserProfileAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IUserService _userService;
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private async Task<bool> IsAdminAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(Roles.Admin) || roles.Contains(Roles.GoverningBodyHead);
        }

        private async Task<bool> IsSameClubAsync(User user, string focusUserId)
        {
            var focusUser = await _userService.GetUserAsync(focusUserId);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var currentUserClub = await _repositoryWrapper.Club.GetFirstOrDefaultAsync(x => x.ClubMembers.Equals(focusUser.ClubMembers));
            var focusUserClub = await _repositoryWrapper.Club.GetFirstOrDefaultAsync(x => x.ClubMembers.Equals(currentUser.ClubMembers));
            if (focusUserClub.ID == currentUserClub.ID)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> IsSameCityAsync(User user, string focusUserId)
        {
            var focusUser = await _userService.GetUserAsync(focusUserId);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var currentUserCity = await _repositoryWrapper.City.GetFirstOrDefaultAsync(x => x.CityMembers.Equals(focusUser.CityMembers));
            var focusUserCity = await _repositoryWrapper.City.GetFirstOrDefaultAsync(x => x.CityMembers.Equals(currentUser.CityMembers));
            if (focusUserCity.ID == currentUserCity.ID)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> IsSameRegionAsync(User user, string focusUserId)
        {
            var focusUser = await _userService.GetUserAsync(focusUserId);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var currentUserRegion = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(x => x.City.Equals(focusUser.CityMembers));
            var focusUserRegion = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(x => x.City.Equals(currentUser.CityMembers));
            if (focusUserRegion.ID == currentUserRegion.ID)
            {
                return true;
            }
            return false;
        }

            public UserProfileAccessService(IRepositoryWrapper repositoryWrapper, IUserService userService, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<bool> ApproveAsCityHead(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (await IsAdminAsync(user))
            {
                return true;
            }
            if ((roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy)) && await IsSameCityAsync(user, focusUserId))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ApproveAsClubHead(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if ((roles.Contains(Roles.KurinHead) || roles.Contains(Roles.KurinHeadDeputy)) && await IsSameClubAsync(user, focusUserId))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> EditUserProfile(User user, string focusUserId)
        {
            if (await IsAdminAsync(user))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ViewFullProfile(User user, string focusUserId)
        {
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if( await IsSameCityAsync(user,focusUserId) || await IsSameClubAsync(user,focusUserId))
            {
                return true;
            }
            var roles = await _userManager.GetRolesAsync(user);
            if ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && await IsSameRegionAsync(user,focusUserId))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ViewShortProfile(User user, string focusUserId)
        {
            return !await ViewFullProfile(user, focusUserId);
        }
    }
}
