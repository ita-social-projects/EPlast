using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ICityMembersService _cityMembers;
        private readonly IClubMembersService _clubMembers;
        private readonly IRegionService _regionService;

        public AdminService(IRepositoryWrapper repoWrapper, 
            UserManager<User> userManager, 
            IMapper mapper, 
            RoleManager<IdentityRole> roleManager,
            ICityMembersService cityMembers,
            IClubMembersService clubMembers,
            IRegionService regionService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _cityMembers = cityMembers;
            _clubMembers = clubMembers;
            _regionService = regionService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IdentityRole>> GetRolesExceptAdminAsync()
        {
            var admin = _roleManager.Roles.Where(i => i.Name == "Admin");
            var allRoles = await _roleManager.Roles.
                Except(admin).
                OrderBy(i => i.Name).
                ToListAsync();
            return allRoles;
        }

        /// <inheritdoc />
        public async Task EditAsync(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.
                Except(roles).
                Except(new List<string> { "Admin" });
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "Прихильник");
            }
        }

        /// <inheritdoc />
        public async Task ChangeAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            }
            
            var cityMember = await _repoWrapper.CityMembers.GetFirstOrDefaultAsync(m => m.UserId == userId);

            if(cityMember != null)
            {
                await _cityMembers.RemoveMemberAsync(cityMember);
            }

            var clubMember = await _repoWrapper.ClubMembers.GetFirstOrDefaultAsync(m => m.UserId == userId);
            if (clubMember != null)
            {
                await _clubMembers.RemoveMemberAsync(clubMember);
            }

            var regionAdmin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.UserId == userId);
            if(regionAdmin != null)
            {
                await _regionService.DeleteAdminByIdAsync(regionAdmin.ID);
            }

            await _userManager.AddToRoleAsync(user, "Колишній член пласту");
        }

        /// <inheritdoc />
        public async Task DeleteUserAsync(string userId)
        {
            User user = await _repoWrapper.User.GetFirstOrDefaultAsync(x => x.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            if (user != null && !roles.Contains("Admin"))
            {
                _repoWrapper.User.Delete(user);
                await _repoWrapper.SaveAsync();
            }
        }

        public async Task ChangeCurrentRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            switch (role)
            {
                case "Прихильник":
                case "Пластун":
                case "Зацікавлений":
                    if (roles.Contains("Прихильник"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Прихильник");
                    }
                    else if (roles.Contains("Пластун"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Пластун");
                    }
                    else if(roles.Contains("Зацікавлений"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Зацікавлений");
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Колишній член пласту");
                    }
                    await _userManager.AddToRoleAsync(user, role);
                    break;
                case "Колишній член пласту":
                    await ChangeAsync(userId);
                    break;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserTableDTO>> UsersTableAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(x => x.EmailConfirmed,
                include:
                    i => i.Include(x => x.UserProfile)
                            .ThenInclude(x => x.Gender)
                        .Include(x => x.UserPlastDegrees)
                            .ThenInclude(x => x.PlastDegree));
            var cities = await _repoWrapper.City.
                GetAllAsync(null, x => x.Include(i => i.Region));
            var clubMembers = await _repoWrapper.ClubMembers.
                GetAllAsync(null, x => x.Include(i => i.Club));
            var cityMembers = await _repoWrapper.CityMembers.
                GetAllAsync(null, x => x.Include(i => i.City));
            List<UserTableDTO> userTable = new List<UserTableDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var cityName = cityMembers.Where(x => x.UserId.Equals(user.Id) && x.EndDate == null)
                                          .Select(x => x.City.Name)
                                          .LastOrDefault() ?? string.Empty;

                userTable.Add(new UserTableDTO
                {
                    User = _mapper.Map<User, ShortUserInformationDTO>(user),
                    ClubName = clubMembers.Where(x => x.UserId.Equals(user.Id) && x.IsApproved)
                                          .Select(x => x.Club.Name).LastOrDefault() ?? string.Empty,
                    CityName = cityName,
                    RegionName = !string.IsNullOrEmpty(cityName) ? cities
                        .FirstOrDefault(x => x.Name.Equals(cityName))
                        ?.Region.RegionName : string.Empty,

                    UserPlastDegreeName = user.UserPlastDegrees.Count != 0 ? user.UserPlastDegrees
                        .FirstOrDefault(x => x.UserId == user.Id && x.DateFinish == null)
                        ?.PlastDegree.Name : string.Empty,
                    UserRoles = string.Join(", ", roles)
                });
            }
            return userTable;
        }

    }
}
