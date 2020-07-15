using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EventUser
{
    public class EventAdministrationTypeManager : IEventAdministrationTypeManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventAdministrationTypeManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> GetTypeIdAsync(string typeName)
        {
            var type = await _repoWrapper.EventAdministrationType
                .GetFirstAsync(predicate: e => e.EventAdministrationTypeName == typeName);

            return type.ID;
        }
    }
}
