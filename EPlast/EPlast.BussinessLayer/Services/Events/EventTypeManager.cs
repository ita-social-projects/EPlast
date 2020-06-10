using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
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
                .FindByCondition(et => et.EventTypeName == typeName)
                .FirstAsync();
            return type.ID;
        }
    }
}
