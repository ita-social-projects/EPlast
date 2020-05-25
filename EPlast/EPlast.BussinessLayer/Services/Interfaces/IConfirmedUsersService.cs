using System;
using System.Security.Claims;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IConfirmedUsersService
    {
        void Create(ClaimsPrincipal user, string userId, bool isClubAdmin = false, bool isCityAdmin = false);
        void Delete(int confirmedUserId);
    }
}
