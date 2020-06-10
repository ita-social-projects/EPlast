using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class ParticipantStatusManager : IParticipantStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ParticipantStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> GetStatusIdAsync(string statusName)
        {
            var status = await _repoWrapper.ParticipantStatus
                .FindByCondition(p => p.ParticipantStatusName == statusName)
                .FirstAsync();
            return status.ID;
        }
    }
}
