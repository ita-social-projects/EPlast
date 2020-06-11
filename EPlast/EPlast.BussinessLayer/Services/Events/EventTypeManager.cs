using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventTypeManager : IEventTypeManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventTypeManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> GetTypeIdAsync(string typeName)
        {
            var type = await _repoWrapper.EventType
                .GetFirstAsync(predicate: et => et.EventTypeName == typeName);

            return type.ID;
        }
    }
}
