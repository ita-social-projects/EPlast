using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using EPlast.Resources;

namespace EPlast.BLL.Settings
{
    public class CityAccessSettings
    {
        private const string AdminRoleName = Roles.Admin;
        private const string RegionAdminRoleName = Roles.OkrugaHead;
        private const string CityAdminRoleName = Roles.CityHead;
        private const string ClubAdminRoleName = Roles.KurinHead;

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
                    { RegionAdminRoleName, new CItyAccessForRegionAdminGetter(_repositoryWrapper) },
                    { CityAdminRoleName, new CityAccessForCityAdminGetter(_repositoryWrapper) }
                };
            }
        }
    }
}