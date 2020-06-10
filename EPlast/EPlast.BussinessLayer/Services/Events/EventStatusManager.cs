using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventStatusManager : IEventStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> GetStatusIdAsync(string statusName)
        {
            var status = await _repoWrapper.EventStatus
                .FindByCondition(e => e.EventStatusName == statusName)
                .FirstAsync();
            return status.ID;
        }
    }
}
