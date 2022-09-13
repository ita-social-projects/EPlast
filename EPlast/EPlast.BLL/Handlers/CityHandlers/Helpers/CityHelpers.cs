using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO.City;
using EPlast.Resources;

namespace EPlast.BLL.Handlers.CityHandlers.Helpers
{
    public static class CityHelpers
    {
        public static CityAdministrationDto GetCityHead(IEnumerable<CityAdministrationDto> admins)
        {
            var cityHead = admins
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead);
            return cityHead;
        }

        public static CityAdministrationDto GetCityHeadDeputy(IEnumerable<CityAdministrationDto> admins)
        {
            var cityHeadDeputy = admins
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy);
            return cityHeadDeputy;
        }

        public static List<CityAdministrationDto> GetCityAdmins(IEnumerable<CityAdministrationDto> admins)
        {
            var cityAdmins = admins
                .ToList();
            return cityAdmins;
        }
    }
}
