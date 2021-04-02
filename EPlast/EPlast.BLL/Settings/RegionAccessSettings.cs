using EPlast.BLL.Services.Region.RegionAccess.RegionAccessGetters;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using EPlast.Resources;

namespace EPlast.BLL.Settings
{
    public class RegionAccessSettings
    {
        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;

        private readonly IRepositoryWrapper _repositoryWrapper;

        public RegionAccessSettings(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Dictionary<string, IRegionAccessGetter> RegionAccessGetters
        {
            get
            {
                return new Dictionary<string, IRegionAccessGetter>
                {
                    { AdminRoleName,  new RegionAccessForAdminGetter(_repositoryWrapper) },
                    { RegionAdminRoleName, new RegionAccessForRegionAdminGetter(_repositoryWrapper) }
                };
            }
        }

    }
}
