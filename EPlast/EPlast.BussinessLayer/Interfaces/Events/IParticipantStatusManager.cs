using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IParticipantStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
