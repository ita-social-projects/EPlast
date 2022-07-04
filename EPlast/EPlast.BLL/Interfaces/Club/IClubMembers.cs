using EPlast.BLL.DTO.Club;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubMember
    {
        public string UserId { get; set; }
        public ClubUserDto User { get; set; }
    }
}
