using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
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
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public NewPlastMemberEmailGreetingService(IRepositoryWrapper repoWrapper,
                                           UserManager<User> userManager,
                                           IEmailSendingService emailSendingService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
        }

        public async Task NotifyNewPlastMembersAsync()
        {
            (await GetNewPlastMembersAsync()).ToList().ForEach(async (user) => await SendEmailGreetingForNewPlastMemberAsync(user.Email));
        }

        private async Task<IEnumerable<User>> GetNewPlastMembersAsync()
        {
            var role = "Пластун";
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

        private async Task<bool> SendEmailGreetingForNewPlastMemberAsync(string userEmail)
        {
            var email = userEmail;
            var subject = "Випробувальний термін завершився!";
            var message = "<h3>СКОБ!</h3>"
                        + "<p>Друже/подруго, сьогодні завершився твій випробувальний період в Пласт!"
                        + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>";
            var title = "EPlast";
            return await _emailSendingService.SendEmailAsync(email, subject, message, title);
        }
    }
}
