using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Events
{
    public interface IEventTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
