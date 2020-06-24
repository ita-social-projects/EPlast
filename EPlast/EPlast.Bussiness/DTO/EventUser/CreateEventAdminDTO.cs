using System.ComponentModel.DataAnnotations;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;

namespace EPlast.BusinessLogicLayer.DTO.EventUser
{
    public class CreateEventAdminDTO
    {
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
