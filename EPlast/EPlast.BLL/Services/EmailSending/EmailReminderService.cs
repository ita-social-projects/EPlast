using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Resources;

namespace EPlast.BLL.Services.EmailSending
{
    public class EmailReminderService : IEmailReminderService
    {
        private readonly IAuthEmailService _authEmailServices;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public EmailReminderService(IRepositoryWrapper repoWrapper,
                                    IAuthEmailService authEmailServices,
                                    IMapper mapper,
                                    UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _authEmailServices = authEmailServices;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> JoinCityReminderAsync()
        {
            try
            {
                (await GetLonelyUsersAsync()).ToList().ForEach(async (user) => await _authEmailServices.SendEmailJoinToCityReminderAsync(user.Email));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private async Task<IEnumerable<User>> GetLonelyUsersAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(u => u.EmailConfirmed);
            var lonelyUsers = new List<User>();
            foreach (var user in users)
            {
                bool isLonelyUser = await IsLonelyUserAsync(user);
                bool isAdmin = await IsAdminAsync(user);
                if (isLonelyUser && !isAdmin)
                {
                    lonelyUsers.Add(user);
                }
            }
            return (lonelyUsers);
        }

        private async Task<CityDTO> GetUserCityAsync(User user)
        {
            var cityMember = await _repoWrapper.CityMembers.GetFirstOrDefaultAsync(x => x.UserId == user.Id,
                                                                                   s => s.Include(c => c.City));
            DataAccess.Entities.City city = null;
            if (cityMember != null) city = cityMember.City;
            var cityDTO = _mapper.Map<DataAccess.Entities.City, CityDTO>(city);
            return (cityDTO);
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return (userRoles.Contains(Roles.Admin));
        }

        private async Task<bool> IsLonelyUserAsync(User user)
        {
            return ((await GetUserCityAsync(user)) == null);
        }
    }
}
