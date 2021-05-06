using EPlast.BLL.DTO.Club;

namespace EPlast.BLL.Interfaces.Club
{
    public interface ClubUser
    {
        public string UserId { get; set; }
        public ClubUserDTO User { get; set; }
    }
}
