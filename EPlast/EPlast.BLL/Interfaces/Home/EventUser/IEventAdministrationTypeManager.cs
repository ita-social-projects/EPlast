using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.EventUser
{
    public interface IEventAdministrationTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
