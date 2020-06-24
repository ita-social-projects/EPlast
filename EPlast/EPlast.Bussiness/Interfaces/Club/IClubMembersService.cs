
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Club
{
    public interface IClubMembersService
    {
        Task ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);
        Task AddFollowerAsync(int index, string userId);
    }
}
