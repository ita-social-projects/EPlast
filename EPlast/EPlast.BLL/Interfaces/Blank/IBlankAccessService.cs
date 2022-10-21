using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Blank
{
    public interface IBlankAccessService
    {
        Task<bool> CanViewBlankTab(User user, string focusUserId);
        Task<bool> CanAddBiography(User user, string focusUserId);
        Task<bool> CanViewBiography(User user, string focusUserId);
        Task<bool> CanDownloadBiography(User user, string focusUserId);
        Task<bool> CanDeleteBiography(User user, string focusUserId);
        Task<bool> CanViewAddDownloadDeleteExtractUPU(User user, string focusUserId);
        Task<bool> CanAddAchievement(User user, string focusUserId);
        Task<bool> CanViewListOfAchievements(User user, string focusUserId);
        Task<bool> CanViewAchievement(User user, string focusUserId);
        Task<bool> CanDownloadAchievement(User user, string focusUserId);
        Task<bool> CanDeleteAchievement(User user, string focusUserId);
        Task<bool> CanGenerateFile(User user, string focusUserId);
    }
}
