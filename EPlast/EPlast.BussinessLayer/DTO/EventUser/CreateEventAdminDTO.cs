using System.ComponentModel.DataAnnotations;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class CreateEventAdminDTO
    {
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public UserDTO User { get; set; }
    }
}
