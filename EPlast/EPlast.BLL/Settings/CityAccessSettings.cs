using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using EPlast.Resources;

namespace EPlast.BLL.Settings
{
    public class CityAccessSettings
    {
        private const string AdminRoleName = Roles.Admin;
        private const string GoverningBodyAdminRoleName = Roles.GoverningBodyAdmin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;
        private const string RegionAdminDeputyRoleName = Roles.OkrugaHeadDeputy;
        private const string CityAdminRoleName = Roles.CityHead;
        private const string CityAdminDeputyRoleName = Roles.CityHeadDeputy;


        private readonly IRepositoryWrapper _repositoryWrapper;

        public CityAccessSettings(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Dictionary<string, ICItyAccessGetter> CitiAccessGetters
        {
            get
            {
                return new Dictionary<string, ICItyAccessGetter>
                {
                    { AdminRoleName,  new CityAccessForAdminGetter(_repositoryWrapper) },
                    { GoverningBodyAdminRoleName,  new CityAccessForAdminGetter(_repositoryWrapper) },
                    { RegionAdminRoleName, new CItyAccessForRegionAdminGetter(_repositoryWrapper) },
                    { RegionAdminDeputyRoleName, new CItyAccessForRegionAdminGetter(_repositoryWrapper) },
                    { CityAdminRoleName, new CityAccessForCityAdminGetter(_repositoryWrapper) },
                    { CityAdminDeputyRoleName, new CityAccessForCityAdminGetter(_repositoryWrapper) }
                };
            }
        }
    }
}