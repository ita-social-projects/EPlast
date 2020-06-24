
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Club
{
    public interface IClubMembersService
    {
        Task ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);
        Task AddFollowerAsync(int index, string userId);
    }
}
