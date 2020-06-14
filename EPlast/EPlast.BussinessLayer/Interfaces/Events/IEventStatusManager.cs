using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
