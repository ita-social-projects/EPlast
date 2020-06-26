using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubMembersService
    {
        Task<ClubMembersDTO> ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);
        Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId);
    }
}