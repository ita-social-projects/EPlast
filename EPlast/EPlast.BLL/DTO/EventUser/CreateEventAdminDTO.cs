using System.ComponentModel.DataAnnotations;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.EventUser
{
    public class CreateEventAdminDTO
    {
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public int EventAdministrationTypeID { get; set; }

        public UserDTO User { get; set; }
    }
}
