using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Decision;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public interface IDecesionRepository : IRepositoryBase<Decesion>
    {
        IEnumerable<DecisionTableObject> GetDecisions(string searchData, int page, int pageSize);
    }
}
