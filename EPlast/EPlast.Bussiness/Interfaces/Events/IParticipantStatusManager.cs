using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Events
{
    public interface IParticipantStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
