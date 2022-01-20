using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services
{
    public class NewPlastMemberEmailGreetingService : INewPlastMemberEmailGreetingService
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public NewPlastMemberEmailGreetingService(IRepositoryWrapper repoWrapper,
                                           UserManager<User> userManager,
                                           IEmailSendingService emailSendingService,
                                           IEmailContentService emailContentService,
                                           INotificationService notificationService,
                                           IUserService userService,
                                           IMediator mediator)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _notificationService = notificationService;
            _userService = userService;
            _mediator = mediator;
        }

        public async Task NotifyNewPlastMembersAndCityAdminsAsync()
        {
            var users = await GetNewPlastMembersAsync();
            foreach (var user in users)
            {
                await SendEmailGreetingForNewPlastMemberAsync(user.User.Email, user.City.Name);
                await NotifyCityAdminsAsync(user.UserId);
                await SendMessageGreetingForNewPlastMemberAsync(user.UserId, user.City.Name);
            }
        }

        private async Task<IEnumerable<CityMembers>> GetNewPlastMembersAsync()
        {
            const string role = Roles.PlastMember;
            var cityMembers = await _repoWrapper.CityMembers.GetAllAsync(x=>x.User.EmailConfirmed,
                c => c.Include(v => v.City)
                    .Include(x=>x.User));

            var newPlastuns = new List<CityMembers>();

            foreach (var member in cityMembers)
            {
                if (await _userManager.IsInRoleAsync(member.User, role) || await IsAdminAsync(member.User)) continue;

                var timeToJoinPlast = member.User.RegistredOn.AddYears(1) - DateTime.Now;
                var halfOfYear = new TimeSpan(182, 0, 0, 0);
                if (_repoWrapper.ConfirmedUser.FindByCondition(x => x.UserID == member.UserId).Any(q => q.isClubAdmin))
                {
                    timeToJoinPlast = timeToJoinPlast.Subtract(halfOfYear);
                }
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(member.UserId);
                    await _userManager.AddToRoleAsync(us, role);
                    await _userManager.RemoveFromRoleAsync(us, Roles.Supporter);
                    newPlastuns.Add(member);
                }
            }
            return newPlastuns;
        }

        public async Task NotifyCityAdminsAsync(string newPlastMemberId)
        {
            var newPlastMember = await _userService.GetUserAsync(newPlastMemberId);
            var query = new GetCityAdminsQuery(newPlastMember.CityMembers.First().CityId);
            var cityProfile = await _mediator.Send(query);
            var cityHead = cityProfile.Head.User;
            await SendEmailCityAdminAboutNewPlastMemberAsync(cityHead.Email, newPlastMember.FirstName,
                newPlastMember.LastName, newPlastMember.UserProfile.Birthday);
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Contains(Roles.Admin);
        }

        private async Task<bool> SendEmailGreetingForNewPlastMemberAsync(string userEmail, string cityName)
        {
            var emailContent = _emailContentService.GetGreetingForNewPlastMemberEmailAsync(cityName);
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

        private async Task SendMessageGreetingForNewPlastMemberAsync(string userId, string cityName)
        {
            var query = new GetCityIdByUserIdQuery(userId);
            var cityId = await _mediator.Send(query);
            var notificationType = (await _notificationService.GetAllNotificationTypesAsync()).First().Id;
            var messageContent = _emailContentService.GetGreetingForNewPlastMemberMessageAsync(userId, cityName, notificationType, cityId);
            await _repoWrapper.UserNotifications.CreateAsync(messageContent);
            await _repoWrapper.SaveAsync();
        }
    }
}