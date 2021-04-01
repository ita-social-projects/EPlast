using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class ConfirmedUsersService : IConfirmedUsersService
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailsContentService _emailsContentService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public ConfirmedUsersService(IRepositoryWrapper repoWrapper,
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

        public async Task CreateAsync(User vaucherUser, string vaucheeId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            var confirmedUser = new ConfirmedUser { UserID = vaucheeId, ConfirmDate = DateTime.Now, isClubAdmin = isClubAdmin, isCityAdmin = isCityAdmin };
            var approver = new Approver { UserID = await _userManager.GetUserIdAsync(vaucherUser), ConfirmedUser = confirmedUser };
            confirmedUser.Approver = approver;
            await _repoWrapper.ConfirmedUser.CreateAsync(confirmedUser);
            await _repoWrapper.SaveAsync();
            await SendEmailConfirmedNotificationAsync(await _userManager.FindByIdAsync(vaucheeId), vaucherUser);
        }

        public async Task DeleteAsync(User vaucherUser, int confirmedUserId)
        {
            var confirmedUser = await _repoWrapper.ConfirmedUser.GetFirstOrDefaultAsync(x => x.ID == confirmedUserId);
            var vaucheeUser = await _userManager.FindByIdAsync(confirmedUser.UserID);
            _repoWrapper.ConfirmedUser.Delete(confirmedUser);
            await _repoWrapper.SaveAsync();
            await SendEmailCanceledNotificationAsync(vaucheeUser, vaucherUser);
        }

        private async Task<bool> SendEmailCanceledNotificationAsync(User vaucheeUser, User vaucherUser)
        {
            var email = await _emailsContentService.GetCanceledUserEmailAsync(vaucheeUser, vaucherUser);
            return await _emailSendingService.SendEmailAsync(vaucheeUser.Email, email.Subject, email.Message, email.Title);
        }

        private async Task<bool> SendEmailConfirmedNotificationAsync(User vaucheeUser, User vaucherUser)
        {
            var email = await _emailsContentService.GetConfirmedUserEmailAsync(vaucheeUser, vaucherUser);
            return await _emailSendingService.SendEmailAsync(vaucheeUser.Email, email.Subject, email.Message, email.Title);
        }
    }
}
