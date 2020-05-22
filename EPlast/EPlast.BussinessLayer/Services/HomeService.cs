using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class HomeService : IHomeService
    {

        public Task ConfirmEmail(IEmailConfirmation emailConfirmation, ContactDTO contactDTO)
        {
            return emailConfirmation.SendEmailAsync("eplastdmnstrtr@gmail.com",
                "Питання користувачів",
                $"Контактні дані користувача : Електронна пошта {contactDTO.Email}, " +
                $"Ім'я {contactDTO.Name}," +
                $"Телефон {contactDTO.PhoneNumber}  " +
                $"Опис питання : {contactDTO.FeedBackDescription}",
                contactDTO.Email);
        }
    }
}
