using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface INewPlastMemberEmailGreetingService
    {
        Task NotifyNewPlastMembersAndCityAdminsAsync();
    }
}
