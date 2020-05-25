using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventTypeManager : IEventTypeManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventTypeManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public int GetTypeId(string typeName)
        {
            int typeId = _repoWrapper.EventType.FindByCondition(et => et.EventTypeName == typeName).First().ID;
            return typeId;
        }
    }
}
