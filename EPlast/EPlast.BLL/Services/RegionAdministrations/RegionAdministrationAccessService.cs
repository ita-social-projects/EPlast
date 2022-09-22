using System.Collections.Generic;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.RegionAdministrations
{
    public class RegionAdministrationAccessService : IRegionAdministrationAccessService
    {
        public bool CanRemoveRegionAdmin(Dictionary<string, bool> defaultAccesses, RegionAdministrationDto regionAdministration, User user)
        {
            if (!defaultAccesses["EditRegion"])
            {
                return false;
            }

            if (regionAdministration.AdminType.AdminTypeName != Roles.OkrugaHead)
            {
                return true;
            }

            return defaultAccesses["RemoveRegionHead"];
        }

        public bool CanEditRegionAdmin(Dictionary<string, bool> defaultAccesses, RegionAdministrationDto regionAdministration, User user)
        {
            if (!defaultAccesses["EditRegion"])
            {
                return false;
            }

            if (regionAdministration.AdminType.AdminTypeName != Roles.OkrugaHead)
            {
                return true;
            }

            return defaultAccesses["EditRegionHead"];
        }
    }
}
