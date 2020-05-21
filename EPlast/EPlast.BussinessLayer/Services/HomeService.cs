using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class HomeService : IHomeService
    {
        HomeService()
        {
        }

        public Task ConfirmEmail(IEmailConfirmation emailConfirmation, ContactDTO contactsViewModel)
        {
            return emailConfirmation.SendEmailAsync("eplastdmnstrtr@gmail.com",
                "Питання користувачів",
                $"Контактні дані користувача : Електронна пошта {contactsViewModel.Email}, " +
                $"Ім'я {contactsViewModel.Name}," +
                $"Телефон {contactsViewModel.PhoneNumber}  " +
                $"Опис питання : {contactsViewModel.FeedBackDescription}",
                contactsViewModel.Email);
        }
    }
}
