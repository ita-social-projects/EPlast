using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface INewPlastMemberEmailGreeting
    {
        Task<bool> NotifyNewPlastMembersAsync();
    }
}
