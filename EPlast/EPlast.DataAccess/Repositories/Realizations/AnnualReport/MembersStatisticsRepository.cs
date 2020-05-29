using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class MembersStatisticsRepository : RepositoryBase<MembersStatistic>, IMembersStatisticsRepository
    {
        public MembersStatisticsRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}