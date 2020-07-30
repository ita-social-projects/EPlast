using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class ParticipantStatusManager : IParticipantStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ParticipantStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        /// <inheritdoc />
        public async Task<int> GetStatusIdAsync(string statusName)
        {
            var status = await _repoWrapper.ParticipantStatus
                .GetFirstAsync(predicate: p => p.ParticipantStatusName == statusName);

            return status.ID;
        }
    }
}
