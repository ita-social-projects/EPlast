using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface IUserProfileAccessService
    {
        Task<bool> ViewShortProfile(User user, string focusUserId);
        Task<bool> ViewFullProfile(User user, string focusUserId);
        Task<bool> ApproveAsClubHead(User user, string focusUserId);
        Task<bool> ApproveAsCityHead(User user, string focusUserId);
        Task<bool> EditUserProfile(User user, string focusUserId);
    }
}
