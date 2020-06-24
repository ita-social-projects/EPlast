using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO.Club;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubMembersService
    {
        Task<ClubMembersDTO> ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);
        Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId);
    }
}