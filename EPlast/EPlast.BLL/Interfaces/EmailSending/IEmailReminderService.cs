using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IEmailReminderService
    {
        Task<bool> JoinCityReminderAsync();

        Task RemindCityAdminsToApproveFollowers();

    }
}
