using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO
{
    public class ApproverDto
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public UserDto User { get; set; }
        public ConfirmedUserDto ConfirmedUser { get; set; }
    }
}
