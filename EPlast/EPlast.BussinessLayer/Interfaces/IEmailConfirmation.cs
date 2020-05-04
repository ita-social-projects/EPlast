using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IEmailConfirmation
    {
        Task SendEmailAsync(string email, string subject, string message, string title);
    }
}
