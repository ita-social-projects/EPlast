using EPlast.BLL.DTO.Club;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubMembersService
    {
        /// <summary>
        /// Change member status in club
        /// </summary>
        /// <param name="clubId">Id of club</param>
        /// <param name="memberId">Person memberId</param>
        /// <returns>Updated ClubMembersDTO</returns>
        Task<ClubMembersDTO> ToggleIsApprovedInClubMembersAsync(int memberId, int clubId);

        /// <summary>
        /// Add follower to club
        /// </summary>
        /// <param name="clubId">Id of club</param>
        /// <param name="userId">Person userId</param>
        /// <returns>Updated ClubMembersDTO</returns>
        Task<ClubMembersDTO> AddFollowerAsync(int clubId, string userId);

        /// <summary>
        /// Removes member from the club
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        Task RemoveMemberAsync(int memberId);
    }
}