namespace EPlast.BLL.DTO.EventUser
{
    public class EventAdministrationDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserInfoDto User { get; set; }
    }
}
