using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class UnconfirmedCityMemberRepository : RepositoryBase<UnconfirmedCityMember>, IUnconfirmedCityMemberRepository
    {
        public UnconfirmedCityMemberRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
