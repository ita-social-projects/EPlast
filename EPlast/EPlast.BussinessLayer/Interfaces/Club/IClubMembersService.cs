
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubMembersService
    {
        Task ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);
        Task AddFollowerAsync(int index, string userId);
    }
}
