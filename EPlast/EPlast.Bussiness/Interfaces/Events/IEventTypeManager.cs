using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Events
{
    public interface IEventTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
