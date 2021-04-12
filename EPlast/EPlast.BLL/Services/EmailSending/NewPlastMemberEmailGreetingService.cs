using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class NewPlastMemberEmailGreetingService : INewPlastMemberEmailGreetingService
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly ICityService _cityService;
        private readonly IUserService _userService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public NewPlastMemberEmailGreetingService(IRepositoryWrapper repoWrapper,
                                           UserManager<User> userManager,
                                           IEmailSendingService emailSendingService,
                                           IEmailContentService emailContentService,
                                           ICityService cityService,
                                           IUserService userService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _cityService = cityService;
            _userService = userService;
        }

        public async Task NotifyNewPlastMembersAndCityAdminsAsync()
        {
            var tasks = new List<Task>();
            var users = await GetNewPlastMembersAsync();
            foreach (var user in users)
            {
                tasks.Add(SendEmailGreetingForNewPlastMemberAsync(user.Email, user.Id));
                tasks.Add(NotifyCityAdminsAsync(user.Id));
            }

            await Task.WhenAll(tasks);
        }

        private async Task<IEnumerable<User>> GetNewPlastMembersAsync()
        {
            var role = Roles.PlastMember;
            var allUsers = await _repoWrapper.User.GetAllAsync(u => u.EmailConfirmed);
            var newPlastuns = new List<User>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, role) || await IsAdminAsync(user)) continue;

                var timeToJoinPlast = user.RegistredOn.AddYears(1) - DateTime.Now;
                var halfOfYear = new TimeSpan(182, 0, 0, 0);
                if (_repoWrapper.ConfirmedUser.FindByCondition(x => x.UserID == user.Id).Any(q => q.isClubAdmin))
                {
                    timeToJoinPlast = timeToJoinPlast.Subtract(halfOfYear);
                }
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(user.Id);
                    await _userManager.AddToRoleAsync(us, role);
                    newPlastuns.Add(user);
                }
            }
            return newPlastuns;
        }

        public async Task NotifyCityAdminsAsync(string newPlastMemberId)
        {
            var newPlastMember = await _userService.GetUserAsync(newPlastMemberId);
            var cityProfile = await _cityService.GetCityAdminsAsync(newPlastMember.CityMembers.First().CityId);

            var cityHead = cityProfile.Head.User;
            await SendEmailCityAdminAboutNewPlastMemberAsync(cityHead.Email, newPlastMember.FirstName,
                newPlastMember.LastName, newPlastMember.UserProfile.Birthday);
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Contains(Roles.Admin);
        }

        private async Task<bool> SendEmailGreetingForNewPlastMemberAsync(string userEmail, string userId)
        {
            var emailContent = await _emailContentService.GetGreetingForNewPlastMemberEmailAsync(userId);
            return await _emailSendingService.SendEmailAsync(userEmail, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }

        private async Task<bool> SendEmailCityAdminAboutNewPlastMemberAsync(string cityAdminEmail, string userFirstName,
            string userLastName, DateTime? userBirthday)
        {
            var emailContent =
                _emailContentService.GetCityAdminAboutNewPlastMemberEmail(userFirstName, userLastName, userBirthday);
            return await _emailSendingService.SendEmailAsync(cityAdminEmail, emailContent.Subject, emailContent.Message,
                emailContent.Title);
        }
    }
}
