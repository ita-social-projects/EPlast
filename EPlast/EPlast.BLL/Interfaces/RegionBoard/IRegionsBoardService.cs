using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.RegionBoard
{
    public interface IRegionsBoardService
    {
        Dictionary<string, bool> GetUserAccess(string userId);
    }
}
