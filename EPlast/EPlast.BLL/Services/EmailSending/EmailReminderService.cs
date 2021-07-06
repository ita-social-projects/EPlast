using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EmailSending
{
    public class EmailReminderService : IEmailReminderService
    {
        private readonly IAuthEmailService _authEmailServices;
        private readonly IEmailContentService _emailContentService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public EmailReminderService(IRepositoryWrapper repoWrapper,
                                    IAuthEmailService authEmailServices,
                                    IEmailContentService emailContentService,
                                    IEmailSendingService emailSendingService,
                                    IMapper mapper,
                                    UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _authEmailServices = authEmailServices;
            _emailContentService = emailContentService;
            _emailSendingService = emailSendingService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> JoinCityReminderAsync()
        {
            try
            {
                (await GetLonelyUsersAsync()).ToList().ForEach(async (user) => await _authEmailServices.SendEmailJoinToCityReminderAsync(user.Email, user.Id));
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

        private async Task<List<User>> GetNewCityFollowersAsync()
        {
            var followers = await _repoWrapper.User.GetAllAsync(u =>
                    u.EmailConfirmed && u.CityMembers.Any() && !u.CityMembers.First().IsApproved,
                i => i.Include(u => u.CityMembers));
            var newFollowers = new List<User>();
            foreach (var follower in followers)
            {
                var isRegisteredUser = await _userManager.IsInRoleAsync(follower, Roles.RegisteredUser);
                if (isRegisteredUser)
                {
                    newFollowers.Add(follower);
                }
            }

            return newFollowers;
        }

        public async Task RemindCityAdminsToApproveFollowers()
        {
            var newFollowers = await GetNewCityFollowersAsync();
            foreach (var newFollower in newFollowers)
            {
                var cityAdmin = await GetCityAdminAsync(newFollower);
                var cityAdminDeputy = await GetCityAdminDeputyAsync(newFollower);
                var emailContent = await _emailContentService.GetCityAdminAboutNewFollowerEmailAsync(newFollower.Id,
                    newFollower.FirstName, newFollower.LastName, true);
                await _emailSendingService.SendEmailAsync(cityAdmin.User.Email, emailContent.Subject,
                    emailContent.Message, emailContent.Title);
                await _emailSendingService.SendEmailAsync(cityAdminDeputy.User.Email, emailContent.Subject,
                    emailContent.Message, emailContent.Title);
            }
        }

        private async Task<CityAdministration> GetCityAdminAsync(User user)
        {
            var cityAdministration = await _repoWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == user.CityMembers.First().CityId,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));
            var cityHead = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));

            return cityHead;
        }

        private async Task<CityAdministration> GetCityAdminDeputyAsync(User user)
        {
            var cityAdministration = await _repoWrapper.CityAdministration
                .GetAllAsync(i => i.CityId == user.CityMembers.First().CityId,
                    i => i
                        .Include(c => c.AdminType)
                        .Include(a => a.User));
            var cityHeadDeputy = cityAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                                                  && (DateTime.Now < a.EndDate || a.EndDate == null));

            return cityHeadDeputy;
        }
    }
}
