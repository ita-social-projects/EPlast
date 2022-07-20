using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Volunteer
{
    public interface IVolunteerInformingService
    {
        public Task SendNewVolunteerNotificationToAdministratorsAsync(string volunteerId);
        public Task SendNewVolunteerEmailToAdministratorsAsync(string volunteerId);
    }
}
