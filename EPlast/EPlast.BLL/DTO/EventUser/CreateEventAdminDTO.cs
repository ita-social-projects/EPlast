using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.EventUser
{
    public class CreateEventAdminDto
    {
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public int EventAdministrationTypeID { get; set; }

        public UserDto User { get; set; }
    }
}
