using EPlast.Bussiness.Services.City.CityAccess.CityAccessGetters;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;

namespace EPlast.Bussiness.Settings
{
    public class CityAccessSettings
    {
        private const string AdminRoleName = "Admin";
        private const string RegionAdminRoleName = "Голова Округу";
        private const string CityAdminRoleName = "Голова Станиці";

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