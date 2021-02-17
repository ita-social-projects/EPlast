using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IConfirmedUsersService
    {
        Task CreateAsync(User vaucherUser, string vaucheeId, bool isClubAdmin = false, bool isCityAdmin = false);
        Task DeleteAsync(User vaucherUser, int confirmedUserId);
    }
}
