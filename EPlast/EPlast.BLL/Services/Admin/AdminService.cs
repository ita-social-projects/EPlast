using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly ICityParticipantsService _cityParticipants;
        private readonly IClubParticipantsService _clubParticipants;
        private readonly IMapper _mapper;
        private readonly IRegionAdministrationService _regionService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminService(IRepositoryWrapper repoWrapper,
                            UserManager<User> userManager,
                            IMapper mapper,
                            RoleManager<IdentityRole> roleManager,
                            IClubParticipantsService clubParticipants,
                            IRegionAdministrationService regionService,
                            ICityParticipantsService cityParticipants)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _clubParticipants = clubParticipants;
            _regionService = regionService;
            _cityParticipants = cityParticipants;
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

            if (cityMember != null)
            {
                await _cityParticipants.RemoveMemberAsync(cityMember);
            }

            var clubMember = await _repoWrapper.ClubMembers.GetFirstOrDefaultAsync(m => m.UserId == userId);
            if (clubMember != null)
            {
                await _clubParticipants.RemoveMemberAsync(clubMember);
            }

            var regionAdmin = await _repoWrapper.RegionAdministration.GetFirstOrDefaultAsync(a => a.UserId == userId);
            if (regionAdmin != null)
            {
                await _regionService.DeleteAdminByIdAsync(regionAdmin.ID);
            }

            await _userManager.AddToRoleAsync(user, "Колишній член пласту");
        }

        public async Task ChangeCurrentRoleAsync(string userId, string role)
        {
            const string supporter = "Прихильник";
            const string plastun = "Пластун";
            const string interested = "Зацікавлений";
            const string formerMember = "Колишній член пласту";
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            switch (role)
            {
                case supporter:
                case plastun:
                case interested:
                    if (roles.Contains(supporter))
                    {
                        await _userManager.RemoveFromRoleAsync(user, supporter);
                    }
                    else if (roles.Contains(plastun))
                    {
                        await _userManager.RemoveFromRoleAsync(user, plastun);
                    }
                    else if (roles.Contains(interested))
                    {
                        await _userManager.RemoveFromRoleAsync(user, interested);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, formerMember);
                    }
                    await UpdateUserDatesByChangeRoleAsyncAsync(userId, role);
                    await _repoWrapper.SaveAsync();
                    await _userManager.AddToRoleAsync(user, role);
                    break;
                case formerMember:
                    await ChangeAsync(userId);
                    break;
            }
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

        /// <inheritdoc />
        public async Task EditAsync(string userId, IEnumerable<string> roles)
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
        public async Task<IEnumerable<CityDTO>> GetCityRegionAdminsOfUserAsync(string userId)
        {
            var cities = await _repoWrapper.City.
                GetAllAsync(predicate: c => c.CityMembers.FirstOrDefault(c => c.UserId == userId) != null,
                            include: x => x.Include(i => i.Region).ThenInclude(r => r.RegionAdministration).ThenInclude(a => a.AdminType)
                                           .Include(c => c.CityAdministration).ThenInclude(c => c.AdminType));

            foreach (var city in cities)
            {
                city.Region.RegionAdministration = city.Region.RegionAdministration.Where(r =>
                {
                    if (r.AdminType.AdminTypeName == "Голова Округу" && (r.EndDate > DateTime.Now || r.EndDate == null))
                    {
                        r.Region = null;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }).ToList();
            }

            var citiesDTO = _mapper.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(cities);

            foreach (var city in citiesDTO)
            {
                city.Region.Administration = _mapper.Map<IEnumerable<RegionAdministration>, IEnumerable<RegionAdministrationDTO>>(cities.First(c => c.ID == city.ID).Region.RegionAdministration);
            }
            return citiesDTO;
        }

        /// <inheritdoc />
        public IEnumerable<IdentityRole> GetRolesExceptAdmin()
        {
            var admin = _roleManager.Roles.Where(i => i.Name == "Admin");
            var allRoles = _roleManager.Roles.Except(admin).OrderBy(i => i.Name);
            return allRoles;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserTableDTO>> GetUsersTableAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(
                predicate: null,
                include: i => i.Include(x => x.UserProfile)
                        .ThenInclude(x => x.Gender)
                    .Include(x => x.UserPlastDegrees)
                        .ThenInclude(x => x.PlastDegree)
                    .Include(x => x.UserProfile)
                            .ThenInclude(x => x.UpuDegree));
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
                    UserRoles = string.Join(", ", roles),
                    UPUDegree = user.UserProfile.UpuDegree.Name,
                    Email = user.UserName
                });
            }
            return userTable;
        }

        public async Task UpdateUserDatesByChangeRoleAsyncAsync(string userId, string role)
        {
            UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates
                           .GetFirstOrDefaultAsync(umd => umd.UserId == userId);
            var cityMember = await _repoWrapper.CityMembers
                 .GetFirstOrDefaultAsync(u => u.UserId == userId, m => m.Include(u => u.User));
            if (role == "Прихильник" && cityMember.IsApproved)
            {
                userMembershipDates.DateEntry = DateTime.Now;
            }
            else if (role != "Пластун")
            {
                userMembershipDates.DateEntry = default;
            }
            else
            {
                DateTime time = default;
                userMembershipDates.DateEntry = userMembershipDates.DateEntry != time ? userMembershipDates.DateEntry : DateTime.Now;
            }
            _repoWrapper.UserMembershipDates.Update(userMembershipDates);
            await _repoWrapper.SaveAsync();
        }
    }
}
