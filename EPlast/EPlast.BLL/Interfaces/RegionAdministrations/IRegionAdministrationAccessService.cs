using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.RegionAdministrations
{
    public interface IRegionAdministrationAccessService
    {
        public bool CanRemoveRegionAdmin(Dictionary<string, bool> defaultAccesses,RegionAdministrationDto regionAdministration, User user);

        public bool CanEditRegionAdmin(Dictionary<string, bool> defaultAccesses, RegionAdministrationDto regionAdministration, User user);
    }
}
