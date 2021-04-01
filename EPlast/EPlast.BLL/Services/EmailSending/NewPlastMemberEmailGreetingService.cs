using EPlast.BLL.Interfaces;
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
        private readonly IEmailsContentService _emailsContentService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public NewPlastMemberEmailGreetingService(IRepositoryWrapper repoWrapper,
                                           UserManager<User> userManager,
                                           IEmailSendingService emailSendingService,
                                           IEmailsContentService emailsContentService,
                                           IUserService userService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
            _emailsContentService = emailsContentService;
            _userService = userService;
        }

        public async Task NotifyNewPlastMembersAsync()
        {
            (await GetNewPlastMembersAsync()).ToList().ForEach(async (user) => await SendEmailGreetingForNewPlastMemberAsync(user.Email, user.Id));
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
                TimeSpan halfOfYear = new TimeSpan(182, 0, 0, 0);
                if (_repoWrapper.ConfirmedUser.FindByCondition(x => x.UserID == user.Id).Any(q => q.isClubAdmin))
                {
                    timeToJoinPlast.Subtract(halfOfYear);
                }
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(user.Id);
                    await _userManager.AddToRoleAsync(us, role);
                    newPlastuns.Add(user);
                }
            }
            return (newPlastuns);
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return (userRoles.Contains("Admin"));
        }

        private async Task<bool> SendEmailGreetingForNewPlastMemberAsync(string userEmail, string userId)
        {
            var email = await _emailsContentService.GetGreetingForNewPlastMemberEmailAsync(userId);
            return await _emailSendingService.SendEmailAsync(userEmail, email.Subject, email.Message, email.Title);
        }
    }
}
