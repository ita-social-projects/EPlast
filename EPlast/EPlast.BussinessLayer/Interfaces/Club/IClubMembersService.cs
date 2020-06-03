
namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubMembersService
    {
        void ToggleIsApprovedInClubMembers(int memberId, int clubId);
        void AddFollower(int index, string userId);
    }
}
