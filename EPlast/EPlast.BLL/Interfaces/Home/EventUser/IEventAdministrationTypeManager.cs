using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventAdministrationTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
    }
}
