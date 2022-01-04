using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using System;
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
                Message = $"Ви успішно активували свій акаунт!\nНе забудьте доєднатись до осередку, перейшовши за <a href='{citiesUrl}'>посиланням</a>"
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
                          + $"<p>{friend}, просимо Тебе доєднатись до пластового осередку впродовж наступних двох тижнів."
                          + $"<br>Зробити це можна, перейшовши за <a href='{citiesUrl}'>посиланням</a>."
                          + "<br>Усі профілі без осередку блокуються системою автоматично.</p>"
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
                Message = $"Підтвердіть реєстрацію, перейшовши за <a href='{confirmationLink}'>посиланням</a>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetAuthResetPasswordEmail(string confirmationLink)
        {
            return new EmailModel
            {
                Title = "Адміністрація сайту EPlast",
                Subject = "Скидання пароля",
                Message = $"Для скидання пароля перейдіть за <a href='{confirmationLink}'>посиланням</a>"
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
                          + "скасував своє поручення за Тебе."
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetCityApproveEmailAsync(string userId, string cityUrl, string cityName)
        {
            var userGender = await _userService.GetUserGenderAsync(userId);
            var steppedIn = userGender switch
            {
                UserGenders.Male => "вступив",
                UserGenders.Female => "вступила",
                _ => "вступив/-ла"
            };

            var plastSupporter = userGender switch
            {
                UserGenders.Male => "Пласт прихильник",
                UserGenders.Female => "Пластунка прихильниця",
                _ => "Пласт прихильник/Пластунка прихильниця"
            };

            var done = userGender switch
            {
                UserGenders.Male => "виконав",
                UserGenders.Female => "виконала",
                _ => "виконав/-ла"
            };

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Вітаємо Тебе у Пластовому осередку",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>Вітаємо, тепер Ти частина пластового осередку <a href='{cityUrl}'>{cityName}</a>."
                          + $"<p>Ти офіційно {steppedIn} до Пласту. Підтвердження профілю осередком є офіційною заявою вступу до організації.</p>"
                          + "<p>Ми радіємо, що Ти з нами!</p>"
                          + $"<p>Ти отримуєш свій перший пластовий ступінь “{plastSupporter}”.</p>"
                          + $"<p>Ти {done} перші завдання Пластового чек-листа (мобільного додатку СтартПласт)."
                          + "<br/>Продовжуй своє знайомство з організацією та виконуй наступні завдання.</p>"
                          + "<p>Для того, щоб стати дійсним членом нашої організації Тобі потрібно виконати всі завдання у додатку.</p>"
                          + "<p>Попереду Тебе чекає захоплива подорож разом з нами.</p>"
                          + "<p>Знаходь нових друзів, дізнавайся багато цікавого та корисного!<p/>"
                          + "<p>Зверни увагу!<p/>"
                          + "<p>Тобі обов'язково потрібно:<p/>"
                          + "<ul><li>Пройти випробувальний термін (він розпочинається з моменту потвердження Твоєї заявки пластовим осередком);</li>"
                          + "<li>Зібрати поручення від 3-х дійсних членів організації та голови осередку;</li>"
                          + "<li>Познайомитись з осередком та взяти активну участь в заходах свого осередку;</li>"
                          + "<li>Підвищити свої навички та знання зголосившись на вишкіл виховників/адміністраторів.</li></ul>"
                          + "<p>Бажаємо Тобі успіхів!</p>"
                          + "<p>Пласт — це велика гра!</p>"
                          + "<p>Вперед до перемог!</p>"
                          + "<p>При виникненні питань просимо написати листа на електронну адресу volunteering@plast.org.ua</p>"
            };
        }

        /// <inheritdoc />
        public async Task<EmailModel> GetCityExcludeEmailAsync(string userId, string cityUrl, string cityName)
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
                Subject = "Зміна статусу членства у станиці",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>{friend}, повідомляємо, що Тебе було виключено зі станиці <a href='{cityUrl}'>{cityName}</a>."
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
                            + $"<p>Вітаємо, Ти {got} поручення у своєму профілі від {friend} {vaucherUser.FirstName} {vaucherUser.LastName}. "
                            + "Виконуй усі завдання Пластового Чек-листа (мобільного додатку Старт Пласт)"
                            + " та отримай ступінь “Дійсного члена організації”!</p>"
                            + "<p>Ми радіємо Твоїм успіхам!</p>"
                            + "Опісля зібрання всіх поручень, повідом відповідального в осередку чи голову "
                            + "осередку про виконання всіх вимог для дійсного членства, щоб отримати право на "
                            + "складання Пластової присяги."
                            + "<p>Будь тією зміною, яку хочеш бачити у світі!</p>"
                            + "При виникненні питань просимо звертатись на електронну адресу volunteering@plast.org.ua"
            };
        }

        /// <inheritdoc />
        public EmailModel GetCityAdminAboutNewPlastMemberEmail(string userFirstName, string userLastName, DateTime? userBirthday)
        {
            var governingBodyName = userBirthday?.AddYears(35) - DateTime.Now <= TimeSpan.Zero
                ? "КБ УСП"
                : "КБ УПС";

            return new EmailModel
            {
                Title = "EPlast",
                Subject = "У члена вашої станиці завершився випробувальний термін",
                Message = $"<p>У користувача {userFirstName} {userLastName} завершився випробувальний термін. "
                          + $"Подай його справу до керівного органу: {governingBodyName}.</p>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetGreetingForNewPlastMemberEmailAsync(string cityName)
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Вітаємо з заваршенням випробувального терміну!",
                Message = "<p>Вітаємо! У тебе закінчився випробувальний термін. Переконайся, " +
                          "що усі вимоги виконано та звернись до відповідального по роботі з " +
                          $"прихильниками у своєму осередку: {cityName}.</p>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetCityRemoveFollowerEmail(string cityUrl, string cityName)
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Зміна статусу заявки у станицю",
                Message = $"<p>На жаль, Тебе було виключено з прихильників станиці: <a href='{cityUrl}'>{cityName}</a>."
            };
        }

        public EmailModel GetCityToSupporterRoleOnApproveEmail()
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Зміна ролі",
                Message = "<p>Тобі надано нову роль: 'Прихильник'.</p>"
            };
        }

        public async Task<EmailModel> GetCityAdminAboutNewFollowerEmailAsync(string userId, string userFirstName, string userLastName, bool isReminder)
        {
            var userGender = await _userService.GetUserGenderAsync(userId);

            var volunteered = userGender switch
            {
                UserGenders.Male => "зголосився",
                UserGenders.Female => "зголосилась",
                _ => "зголосився/зголосилась"
            };

            var follower = userGender switch
            {
                UserGenders.Male => "прихильника",
                UserGenders.Female => "прихильниці",
                _ => "прихильника/прихильниці"
            };

            var title = isReminder
                ? "Нагадування підтвердити профіль нового волонтера в системі ePlast"
                : "Підтвердіть профіль нового волонтера в системі ePlast";

            return new EmailModel
            {
                Title = "EPlast",
                Subject = $"{title}",
                Message = "<h3>СКОБ!</h3>"
                          + $"<p>До Твоєї станиці {volunteered} волонтер {userFirstName} {userLastName}."
                          + "<p>Бажаємо цікавих знайомств та легкої адаптації :) Просимо переглянути профіль "
                          + "користувача, а тоді підтвердити профіль волонтера, таким чином надавши ступінь "
                          + $"'{follower}'. Або аргументовано його не підтвердити."
                          + "Дякуємо Тобі за роботу.</p>"
                          + "<p>Гарного дня.</p>"
                          + "<p>При виникненні питань просимо звертатись на електронну адресу volunteering@plast.org.ua</p>"
            };
        }

        /// <inheritdoc />
        public EmailModel GetUserRenewalConfirmationEmail(string cityName)
        {
            return new EmailModel
            {
                Title = "EPlast",
                Subject = "Відновлення статусу",
                Message = "<h3>СКОБ!</h3>"
                          + "<p>Доступ до Вашого аккаунту відновлено.</p>"
                          + $"<p>Відтепер Ви є прихильником станиці {cityName}.</p>"
                          + "<p>Ласкаво просимо до Пласту!</p>"
            };
        }

        /// <inheritdoc />
        public UserNotification GetGreetingForNewPlastMemberMessageAsync(string userId, string cityName, int notificationType, int cityId)
        {
            return new UserNotification()
            {
                OwnerUserId = userId,
                NotificationTypeId = notificationType,
                SenderLink = $"cities/{cityId}",
                SenderName = cityName,
                CreatedAt = DateTime.Now,
                Message = "Вітаємо! У тебе закінчився випробувальний термін. " +
                          "Переконайся, що усі вимоги виконано та звернись до " +
                          "відповідального по роботі з прихильниками у своєму " +
                          "осередку:"
            };
        }
    }
}