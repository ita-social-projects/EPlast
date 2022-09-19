using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IConfirmedUsersService
    {
        Task CreateAsync(User vaucherUser, string vaucheeId, ApproveType approveType);
        Task DeleteAsync(User vaucherUser, int confirmedUserId);
    }
}
