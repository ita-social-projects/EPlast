using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IUserProfileAccessService
    {
        Task<bool> CanViewFullProfile(User user, string focusUserId);
        Task<bool> CanApproveAsHead(User user, string focusUserId, string role);
        Task<bool> CanEditUserProfile(User user, string focusUserId);
    }
}
