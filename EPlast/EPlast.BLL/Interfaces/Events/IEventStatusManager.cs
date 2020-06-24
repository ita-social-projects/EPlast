using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}
