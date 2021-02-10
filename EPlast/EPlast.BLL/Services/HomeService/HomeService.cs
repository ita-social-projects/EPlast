using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly IEmailSendingService _emailSendingService;

        public HomeService(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
        }

        public Task SendEmailAdmin(ContactsDto contactDTO)
        {
            return _emailSendingService.SendEmailAsync(
                "eplastdmnstrtr@gmail.com",
                "Питання користувачів",
                $"Контактні дані користувача : Електронна пошта {contactDTO.Email}, " +
                $"Ім'я {contactDTO.Name}," +
                $"Телефон {contactDTO.PhoneNumber}  " +
                $"Опис питання : {contactDTO.FeedBackDescription}",
                contactDTO.Email);
        }
    }
}
