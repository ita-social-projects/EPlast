using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services
{
    public class AdminService:IAdminService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public AdminService(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public IEnumerable<IdentityRole> GetRolesExceptAdmin()
        {
            var admin = _roleManager.Roles.Where(i => i.Name == "Admin");
            var allRoles = _roleManager.Roles.Except(admin).OrderBy(i => i.Name).ToList();
            return allRoles;
        }

        public async Task Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles).Except(new List<string> { "Admin" });
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "Прихильник");
            }
        }

        public async Task DeleteUser(string userId)
        {
            User user = _repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault();
            var roles = await _userManager.GetRolesAsync(user);
            if (user != null && !roles.Contains("Admin"))
            {
                _repoWrapper.User.Delete(user);
                _repoWrapper.Save();
            }
        }

        public async Task<IEnumerable<UserTableDTO>> UsersTable()
        {
            var users = _repoWrapper.User
                    .Include(x => x.UserProfile, x => x.UserPlastDegrees, x => x.UserProfile.Gender)
                    .ToList();

            var cities = _repoWrapper.City
                .Include(x => x.Region)
                .ToList();
            var clubMembers = _repoWrapper.ClubMembers.Include(x => x.Club)
                                                      .ToList();
            var cityMembers = _repoWrapper.CityMembers.Include(x => x.City)
                                                      .ToList();
            List<UserTableDTO> userTable = new List<UserTableDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var cityName = cityMembers.Where(x => x.User.Id.Equals(user.Id) && x.EndDate == null)
                                          .Select(x => x.City.Name)
                                          .LastOrDefault() ?? string.Empty;

                userTable.Add(new UserTableDTO
                {
                    User = _mapper.Map<User,UserDTO>(user),
                    ClubName = clubMembers.Where(x => x.UserId.Equals(user.Id) && x.IsApproved == true)
                                          .Select(x => x.Club.ClubName).LastOrDefault() ?? string.Empty,
                    CityName = cityName,
                    RegionName = !cityName.Equals(string.Empty) ? cities.Where(x => x.Name.Equals(cityName))
                                       .FirstOrDefault()
                                       .Region
                                       .RegionName : string.Empty,
                    UserPlastDegreeName = user.UserPlastDegrees.Count != 0 ? user.UserPlastDegrees.Where(x => x.UserId == user.Id && x.DateFinish == null)
                                                               .FirstOrDefault()
                                                               .UserPlastDegreeType
                                                               .GetDescription() : string.Empty,
                    UserRoles = string.Join(", ", roles)
                });
            }
            return userTable;
        }
            
    }
}
