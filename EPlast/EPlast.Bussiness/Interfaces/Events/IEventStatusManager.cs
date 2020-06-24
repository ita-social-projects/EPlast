using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Events
{
    public interface IEventStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
