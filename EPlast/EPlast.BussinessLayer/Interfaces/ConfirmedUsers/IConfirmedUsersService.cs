using System.Security.Claims;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IConfirmedUsersService
    {
        void CreateAsync(ClaimsPrincipal user, string userId, bool isClubAdmin = false, bool isCityAdmin = false);
        void DeleteAsync(int confirmedUserId);
    }
}
