using EPlast.BLL.Interfaces;
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
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public ConfirmedUsersService(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IEmailSendingService emailSendingService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
        }

        public async Task CreateAsync(User vaucherUser, string vaucheeId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            var confirmedUser = new ConfirmedUser { UserID = vaucheeId, ConfirmDate = DateTime.Now, isClubAdmin = isClubAdmin, isCityAdmin = isCityAdmin };
            var approver = new Approver { UserID = await _userManager.GetUserIdAsync(vaucherUser), ConfirmedUser = confirmedUser };
            confirmedUser.Approver = approver;
            await _repoWrapper.ConfirmedUser.CreateAsync(confirmedUser);
            await _repoWrapper.SaveAsync();
            await SendEmailConfirmedNotificationAsync(await _userManager.FindByIdAsync(vaucheeId), vaucherUser, true);
        }

        public async Task DeleteAsync(User vaucherUser, int confirmedUserId)
        {
            var confirmedUser = await _repoWrapper.ConfirmedUser.GetFirstOrDefaultAsync(x => x.ID == confirmedUserId);
            var vaucheeUser = await _userManager.FindByIdAsync(confirmedUser.UserID);
            _repoWrapper.ConfirmedUser.Delete(confirmedUser);
            await _repoWrapper.SaveAsync();
            await SendEmailConfirmedNotificationAsync(vaucheeUser, vaucherUser, false);
        }

        private async Task<bool> SendEmailConfirmedNotificationAsync(User vaucheeUser, User vaucherUser, bool confirmed)
        {
            var caseMessage = confirmed ? "поручився за тебе." : "скасував своє поручення за тебе.";
            var message = "<h3>СКОБ!</h3>"
                          + $"<p>Друже / подруго, повідомляємо, що користувач {vaucherUser.FirstName} {vaucherUser.LastName} "
                          + caseMessage
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>";
            var sendResult = await _emailSendingService.SendEmailAsync(vaucheeUser.Email, "Зміна статусу поручення", message, "EPlast");
            return sendResult;
        }
    }
}
