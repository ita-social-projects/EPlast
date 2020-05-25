using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Events
{
    public class ParticipantStatusManager : IParticipantStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ParticipantStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public int GetStatusId(string statusName)
        {
            int statusId = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == statusName).First().ID;
            return statusId;
        }
    }
}
