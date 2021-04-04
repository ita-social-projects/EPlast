using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EmailSending
{
    public class EmailContentService : IEmailContentService
    {
        private readonly IUserService _userService;

        public EmailContentService(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc />
        public EmailModel GetAuthFacebookRegisterEmail()
        {
            return new EmailModel
            {
                Title = "Адміністрація сайту EPlast",
                Subject = "Повідомлення про реєстрацію",
                Message = "Ви зареєструвались в системі EPlast використовуючи свій Facebook-акаунт. "
            };
        }

        /// <inheritdoc />
        public EmailModel GetAuthGoogleRegisterEmail()
        {
            return new EmailModel
            {
                Title = "Адміністрація сайту EPlast",
                Subject = "Повідомлення про реєстрацію",
                Message = "Ви зареєструвались в системі EPlast використовуючи свій Google-акаунт. "
            };
        }

        /// <inheritdoc />
        public EmailModel GetAuthGreetingEmail(string citiesUrl)
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Вітаємо у системі!",
                Message = $"Ви успішно активували свій акаунт!\nНе забудьте доєднатись до осередку, перейшовши за :  <a href='{citiesUrl}'>посиланням</a> "
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetAuthJoinToCityReminderEmailAsync(string citiesUrl, string userId)
        {
            var userGender = await _userService.GetUserGenderAsync(userId);
            var friend = userGender switch
            {
                UserGenders.Male => "Друже",
                UserGenders.Female => "Подруго",
                _ => "Друже/подруго"
            };

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Нагадування про приєднання до станиці",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>{friend}, просимо тебе доєднатись до пластового осередку впродовж цього тижня."
                          + $"<br>Зробити це можна, перейшовши за <a href='{citiesUrl}'>посиланням</a>."
                          + "<br>Профілі без осередку блокуються системою автоматично.</p>"
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetAuthRegisterEmail(string confirmationLink)
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Підтвердження реєстрації",
                Message = $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetAuthResetPasswordEmail(string confirmationLink)
        {
            return new EmailModel
            {
                Title = "Адміністрація сайту EPlast",
                Subject = "Скидування пароля",
                Message = $"Для скидування пароля перейдіть за : <a href='{confirmationLink}'>посиланням</a>"
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetCanceledUserEmailAsync(User vaucheeUser, User vaucherUser)
        {
            var vaucheeUserGender = await _userService.GetUserGenderAsync(vaucheeUser.Id);
            var friend = vaucheeUserGender switch
            {
                UserGenders.Male => "Друже",
                UserGenders.Female => "Подруго",
                _ => "Друже/подруго"
            };

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Зміна статусу поручення",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>{friend}, повідомляємо, що користувач {vaucherUser.FirstName} {vaucherUser.LastName} "
                          + "скасував своє поручення за тебе."
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetCityApproveEmail(string cityUrl, string cityName, bool isApproved)
        {
            var status = isApproved ? "прийнято до" : "було виключено зі";
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Зміна статусу членства у станиці",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>Друже / подруго, повідомляємо, що тебе {status} станиці <a href='{cityUrl}'>{cityName}</a>."
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetConfirmedUserEmailAsync(User vaucheeUser, User vaucherUser)
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

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Ти отримав Пластове поручення!",
                Message = "<h3>СКОБ!</h3>"
                            + $"<p>Вітаємо, ти {got} поручення у своєму профілі від {friend} {vaucherUser.FirstName} {vaucherUser.LastName}."
                            + "Виконуй усі завдання Пластового Чек-листа(мобільного додатку Старт Пласт)"
                            + " та отримай ступінь “Дійсного члена організації”!<p/>"
                            + "<p>Ми радіємо Твоїм успіхам!</p>"
                            + "Опісля зібрання всіх поручень, повідом відповідального в осередку чи голову "
                            + "осередку про виконання всіх вимог для дійсного членства, щоб отримати право на "
                            + "складання Пластової присяги."
                            + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
                            + "При виникненні питань просимо звертатись на скриньку volunteering@plast.org.ua"
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetGreetingForNewPlastMemberEmailAsync(string userId)
        {
            var userGender = await _userService.GetUserGenderAsync(userId);
            var friend = userGender switch
            {
                UserGenders.Male => "Друже",
                UserGenders.Female => "Подруго",
                _ => "Друже/подруго"
            };

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Випробувальний термін завершився!",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>{friend}, сьогодні завершився твій випробувальний період в Пласт!"
                          + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
            };
        }
    }
}
