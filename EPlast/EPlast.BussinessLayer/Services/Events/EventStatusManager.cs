using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventStatusManager : IEventStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public int GetStatusId(string statusName)
        {
            int statusId = _repoWrapper.EventStatus.FindByCondition(e => e.EventStatusName == statusName).First().ID;
            return statusId;
        }
    }
}
