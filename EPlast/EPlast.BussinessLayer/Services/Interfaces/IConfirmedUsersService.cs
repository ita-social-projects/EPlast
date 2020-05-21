using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IConfirmedUsersService
    {
        void Create(ClaimsPrincipal user, string userId, bool isClubAdmin = false, bool isCityAdmin = false);
        void Delete(int confirmedUserId);
    }
}
