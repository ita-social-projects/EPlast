using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.RegionBoard
{
    public interface IRegionsBoardService
    {
        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);
    }
}
