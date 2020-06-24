using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
