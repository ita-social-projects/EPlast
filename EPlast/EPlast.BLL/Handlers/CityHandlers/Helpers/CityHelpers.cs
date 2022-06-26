using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO.City;
using EPlast.Resources;

namespace EPlast.BLL.Handlers.CityHandlers.Helpers
{
    public static class CityHelpers
    {
        public static CityAdministrationDTO GetCityHead(IEnumerable<CityAdministrationDTO> admins)
        {
            var cityHead = admins
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead);
            return cityHead;
        }

        public static CityAdministrationDTO GetCityHeadDeputy(IEnumerable<CityAdministrationDTO> admins)
        {
            var cityHeadDeputy = admins
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy);
            return cityHeadDeputy;
        }

        public static List<CityAdministrationDTO> GetCityAdmins(IEnumerable<CityAdministrationDTO> admins)
        {
            var cityAdmins = admins
                .ToList();
            return cityAdmins;
        }
    }
}
