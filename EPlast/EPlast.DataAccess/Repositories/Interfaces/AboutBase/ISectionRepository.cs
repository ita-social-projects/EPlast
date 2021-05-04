using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface ISectionRepository : IRepositoryBase<Section>
    {
        Task CreateAsync();
    }
}
