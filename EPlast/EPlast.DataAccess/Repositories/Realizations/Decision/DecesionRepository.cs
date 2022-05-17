using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Decision;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class DecesionRepository : RepositoryBase<Decesion>, IDecesionRepository
    {
        public DecesionRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<DecisionTableObject>> GetDecisions(string searchData, int page, int pageSize)
        {
            return await Task.Run(() => EPlastDBContext.Set<DecisionTableObject>().FromSqlRaw(
                "dbo.getDecisionsInfo  @searchData = {0}, @PageIndex = {1}, @PageSize = {2}", searchData, page,
                pageSize));
        }
    }
}
