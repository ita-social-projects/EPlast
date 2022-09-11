using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;

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
