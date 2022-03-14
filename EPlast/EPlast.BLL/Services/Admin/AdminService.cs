using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.FormerMember;

namespace EPlast.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        public readonly IFormerMemberService _formerMemberService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminService(IRepositoryWrapper repoWrapper,
                            IFormerMemberService formerMemberService,
                            UserManager<User> userManager,
                            IMapper mapper,
                            RoleManager<IdentityRole> roleManager)
        {
            _repoWrapper = repoWrapper;
            _formerMemberService = formerMemberService;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public async Task ChangeAsync(string userId)
        {
            await _formerMemberService.MakeUserFormerMeberAsync(userId);
        }
        public async Task ChangeCurrentRoleAsync(string userId, string role)
        {
            const string supporter = Roles.Supporter;
            const string plastun = Roles.PlastMember;
            const string interested = Roles.Interested;
            const string formerMember = Roles.FormerPlastMember;
            const string registeredUser = Roles.RegisteredUser;
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            switch (role)
            {
                case supporter:
                case plastun:
                case interested:
                case registeredUser:
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
                    else if (roles.Contains(formerMember))
                    {
                        await _userManager.RemoveFromRoleAsync(user, formerMember);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, registeredUser);
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
            if (user != null && !roles.Contains(Roles.Admin))
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
                await _userManager.AddToRoleAsync(user, Roles.Supporter);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityDTO>> GetCityRegionAdminsOfUserAsync(string userId)
        {
            var cities = await _repoWrapper.City.
                GetAllAsync(predicate: c => c.CityMembers.FirstOrDefault(c => c.UserId == userId) != null,
                            include: x => x.Include(i => i.Region).ThenInclude(r => r.RegionAdministration).ThenInclude(a => a.AdminType)
                                           .Include(c => c.CityAdministration).ThenInclude(c => c.AdminType));
            foreach (var region in cities.Select(x => x.Region))
            {
                region.RegionAdministration = region.RegionAdministration.Where(r =>
                {
                    if ((r.AdminType.AdminTypeName == Roles.OkrugaHead || r.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy) && (r.EndDate > DateTime.Now || r.EndDate == null))
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
        public async Task<IEnumerable<ShortUserInformationDTO>> GetUsersByRolesAsync(string rolesString, bool include, Func<IEnumerable<User>, IEnumerable<string>, bool, Task<IEnumerable<ShortUserInformationDTO>>> filterRoles)
        {
            var rolesGroups = rolesString.Split('|');
            var users = await _repoWrapper.User.GetAllAsync();
            var filteredUsers = new List<ShortUserInformationDTO>();
            foreach (var rolesGroup in rolesGroups)
            {
                var roles = rolesGroup.Split(',').OrderByDescending(x => x);
                filteredUsers.AddRange(await filterRoles(users, roles, include));
            }
            return filteredUsers;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ShortUserInformationDTO>> FilterByAnyRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include)
        {
            var filteredUsers = new List<ShortUserInformationDTO>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var intersectedRoles = userRoles.Intersect(roles).OrderByDescending(x => x);
                if (intersectedRoles.Any() == include)
                {
                    filteredUsers.Add(_mapper.Map<User, ShortUserInformationDTO>(user));
                }
            }
            return filteredUsers;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ShortUserInformationDTO>> FilterByAllRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include)
        {
            var filteredUsers = new List<ShortUserInformationDTO>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var intersectedRoles = userRoles.Intersect(roles).OrderByDescending(x => x);
                if (intersectedRoles.SequenceEqual(roles) == include)
                {
                    filteredUsers.Add(_mapper.Map<User, ShortUserInformationDTO>(user));
                }
            }
            return filteredUsers;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ShortUserInformationDTO>> FilterByExactRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include)
        {
            var filteredUsers = new List<ShortUserInformationDTO>();

            foreach (var user in users)
            {
                var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
                if (roles.SequenceEqual(userRoles.OrderByDescending(x => x)) == include)
                {
                    filteredUsers.Add(_mapper.Map<User, ShortUserInformationDTO>(user));
                }
            }
            return filteredUsers;
        }

        /// <inheritdoc />
        public IEnumerable<IdentityRole> GetRolesExceptAdmin()
        {
            var admin = _roleManager.Roles.Where(i => i.Name == Roles.Admin);
            var allRoles = _roleManager.Roles.Except(admin).OrderBy(i => i.Name);
            return allRoles;
        }

        /// <inheritdoc />
        public async Task<Tuple<IEnumerable<UserTableDTO>, int>> GetUsersTableAsync(TableFilterParameters tableFilterParameters, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            string strAndClubs = null;
            string strRegions = tableFilterParameters.Regions == null ? null : string.Join(",", tableFilterParameters.Regions.ToArray());
            string strCities = tableFilterParameters.Cities == null ? null : string.Join(",", tableFilterParameters.Cities.ToArray());
            string strClubs = tableFilterParameters.Clubs == null ? null : string.Join(",", tableFilterParameters.Clubs.ToArray());
            if (!roles.Contains(Roles.Admin)){
                FilterTableParametersByRole filterTableParametersByRole = TableFilterParameters_byRole(roles, userId).Result;
                strAndClubs = filterTableParametersByRole.AndClubs;
                strRegions = filterTableParametersByRole.Regions;
                strCities = filterTableParametersByRole.Cities;
                strClubs = filterTableParametersByRole.Clubs;
            }
           
            string strDegrees = tableFilterParameters.Degrees == null ? null : string.Join(",", tableFilterParameters.Degrees.ToArray());
            string strRoles = tableFilterParameters.FilterRoles == null ? null : string.Join(", ", tableFilterParameters.FilterRoles.ToArray());
            var tuple = await _repoWrapper.AdminType.GetUserTableObjects(tableFilterParameters.Page,
                tableFilterParameters.PageSize, tableFilterParameters.Tab, strRegions, strCities, strClubs, strDegrees,
                tableFilterParameters.SortKey, tableFilterParameters.SearchData, strRoles, strAndClubs);
            var users = tuple.Item1;
            var rowCount = tuple.Item2;

            return new Tuple<IEnumerable<UserTableDTO>, int>(_mapper.Map<IEnumerable<UserTableObject>, IEnumerable<UserTableDTO>>(users), rowCount);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ShortUserInformationDTO>> GetUsersAsync()
        {
            var lowerRoles = new List<string>
            {
                Roles.RegisteredUser,
                Roles.Supporter,
                Roles.FormerPlastMember,
                Roles.Interested
            };
            var users = await _repoWrapper.User.GetAllAsync();
            var usersDtos = new List<ShortUserInformationDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var isInLowerRole = roles.Intersect(lowerRoles).Any();
                var shortUser = _mapper.Map<User, ShortUserInformationDTO>(user);
                shortUser.IsInLowerRole = isInLowerRole;
                usersDtos.Add(shortUser);
            }
            return usersDtos;
        }

        public async Task UpdateUserDatesByChangeRoleAsyncAsync(string userId, string role)
        {
            UserMembershipDates userMembershipDates = await _repoWrapper.UserMembershipDates
                           .GetFirstOrDefaultAsync(umd => umd.UserId == userId);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var cityMember = await _repoWrapper.CityMembers
                 .GetFirstOrDefaultAsync(u => u.UserId == userId, m => m.Include(u => u.User));
            if (role == Roles.Supporter && cityMember.IsApproved && userMembershipDates.DateEntry == default)
            {
                userMembershipDates.DateEntry = DateTime.Now;
            }
            //This user is former member, and we change his role to registered, that's why we reset his EndDate
            else if (role == Roles.RegisteredUser && roles.Count == 0)
            {
                userMembershipDates.DateEnd = default;
            }
            else
            {
                DateTime time = default;
                userMembershipDates.DateEntry = userMembershipDates.DateEntry != time ? userMembershipDates.DateEntry : DateTime.Now;
            }
            _repoWrapper.UserMembershipDates.Update(userMembershipDates);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<ShortUserInformationDTO>> GetShortUserInfoAsync(string searchString)
        {
            var users = await _repoWrapper.User.GetAllAsync(u =>
                u.FirstName.Contains(searchString) || u.LastName.Contains(searchString));

            return users.Select(user => _mapper.Map<User, ShortUserInformationDTO>(user)).ToList();
        }

        public Task<int> GetUsersCountAsync()
        {
            return _repoWrapper.AdminType.GetUsersCountAsync();
        }

        public async Task<FilterTableParametersByRole> TableFilterParameters_byRole (IList<string> roles, string userId)
        {
            bool Cities = roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy) || roles.Contains(Roles.PlastMember);
            bool Regions = roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy);
            bool Clubs = roles.Contains(Roles.KurinHead) || roles.Contains(Roles.KurinHeadDeputy);

            FilterTableParametersByRole filterTableParametersByRole = new FilterTableParametersByRole();
            if (Regions)
            {
                filterTableParametersByRole.Regions = (await _repoWrapper.RegionAdministration.GetSingleAsync(r => r.UserId == userId && r.Status)).RegionId.ToString();              
            }
            if (Cities && !Regions)
            {
                filterTableParametersByRole.Cities = (await _repoWrapper.CityMembers.GetSingleAsync(r => r.UserId == userId && r.IsApproved)).CityId.ToString();
            }
           
            if (Clubs)
            {
                filterTableParametersByRole.AndClubs = (await _repoWrapper.ClubAdministration.GetSingleAsync(r => r.UserId == userId && r.Status)).ClubId.ToString();
            }

            return filterTableParametersByRole;
        }
        public async Task<bool> IsCityMember(string userId)
        {
            return (await _repoWrapper.CityMembers.GetAllAsync(c => c.UserId == userId && c.IsApproved)).Any();
        }

        public async Task<IEnumerable<ShortUserInformationDTO>> GetUsersForGoverningBodiesAsync()
        {
            var adminRoles = new List<string>
            {
                Roles.GoverningBodyHead,
                Roles.GoverningBodySecretary,
                Roles.GoverningBodySectorHead,
                Roles.GoverningBodySectorSecretary
            };
            var users = await _repoWrapper.User.GetAllAsync();
            var usersDtos = new List<ShortUserInformationDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(Roles.PlastMember))
                    {
                        var IsInDeputyRole = roles.Intersect(adminRoles).Any();
                        var shortUser = _mapper.Map<User, ShortUserInformationDTO>(user);
                        shortUser.IsInDeputyRole = IsInDeputyRole;
                        usersDtos.Add(shortUser);
                    }
            }
            return usersDtos;
        }
    }
}
