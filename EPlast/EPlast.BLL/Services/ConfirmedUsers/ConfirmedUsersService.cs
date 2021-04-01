using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using EPlast.Resources;

namespace EPlast.BLL.Services
{
    public class ConfirmedUsersService : IConfirmedUsersService
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public ConfirmedUsersService(IRepositoryWrapper repoWrapper,
            UserManager<User> userManager,
            IEmailSendingService emailSendingService,
            IUserService userService)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _emailSendingService = emailSendingService;
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
            var vaucheeUserGender = await _userService.GetUserGenderAsync(vaucheeUser.Id);
            var friend = vaucheeUserGender switch
            {
                UserGenders.Male => "Друже",
                UserGenders.Female => "Подруго",
                _ => "Друже/подруго"
            };
            var title = "EPlast";
            var subject = "Зміна статусу поручення";
            var message = "<h3>СКОБ!</h3>"
                          + $"<p>{friend}, повідомляємо, що користувач {vaucherUser.FirstName} {vaucherUser.LastName} "
                          + "скасував своє поручення за тебе."
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>";
            var sendResult = await _emailSendingService.SendEmailAsync(vaucheeUser.Email, subject, message, title);
            return sendResult;
        }

        private async Task<bool> SendEmailConfirmedNotificationAsync(User vaucheeUser, User vaucherUser)
        {
            var vaucheeUserGender = await _userService.GetUserGenderAsync(vaucheeUser.Id);
            var vaucherUserGender = await _userService.GetUserGenderAsync(vaucherUser.Id);
            var got = vaucheeUserGender switch
            {
                UserGenders.Male => "отримав",
                UserGenders.Female => "отримала",
                _ => "отримав/-ла"
            };
            var friend = vaucherUserGender switch
            {
                UserGenders.Male => "друга",
                UserGenders.Female => "подруги",
                _ => "друга/подруги"
            };
            var title = "EPlast";
            var subject = "Ти отримав Пластове поручення!";
            var message = "<h3>СКОБ!</h3>"
                          + $"<p>Вітаємо, ти {got} поручення у своєму профілі від {friend} {vaucherUser.FirstName} {vaucherUser.LastName}."
                          + "Виконуй усі завдання Пластового Чек-листа(мобільного додатку Старт Пласт)"
                          + " та отримай ступінь “Дійсного члена організації”!<p/>"
                          + "<p>Ми радіємо Твоїм успіхам!</p>"
                          + "Опісля зібрання всіх поручень, повідом відповідального в осередку чи голову "
                          + "осередку про виконання всіх вимог для дійсного членства, щоб отримати право на "
                          + "складання Пластової присяги."
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
                          + "При виникненні питань просимо звертатись на скриньку volunteering@plast.org.ua";
            var sendResult = await _emailSendingService.SendEmailAsync(vaucheeUser.Email, subject, message, title);
            return sendResult;
        }
    }
}
