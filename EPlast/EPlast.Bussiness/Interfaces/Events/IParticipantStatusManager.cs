using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Events
{
    public interface IParticipantStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
