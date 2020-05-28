using System.ComponentModel.DataAnnotations;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels.EventUser
{
    public class EventAdministrationViewModel
    {
        [Required(ErrorMessage = "Ви повинні обрати адміністрацію події")]
        public string UserID { get; set; }
        public UserViewModel User { get; set; }
    }
}
