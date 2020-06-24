using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IParticipantStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
