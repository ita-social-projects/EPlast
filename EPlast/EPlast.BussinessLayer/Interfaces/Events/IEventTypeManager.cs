using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
