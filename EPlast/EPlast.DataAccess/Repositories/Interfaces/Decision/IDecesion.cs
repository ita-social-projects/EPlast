using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Decision;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public interface IDecesionRepository : IRepositoryBase<Decesion>
    {
        Task<IEnumerable<DecisionTableObject>> GetDecisions(string searchData, int page, int pageSize);
    }
}
