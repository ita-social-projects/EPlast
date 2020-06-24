using System.ComponentModel.DataAnnotations;
using EPlast.Bussiness.DTO.UserProfiles;

namespace EPlast.Bussiness.DTO.EventUser
{
    public class CreateEventAdminDTO
    {
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
