using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO.City;
using EPlast.Resources;

namespace EPlast.BLL.Handlers.CityHandlers.Helpers
{
    public static class CityHelpers
    {
        public static CityAdministrationDTO GetCityHead(CityDTO city)
        {
            var cityHead = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHead && a.Status);
            return cityHead;
        }

        public static CityAdministrationDTO GetCityHeadDeputy(CityDTO city)
        {
            var cityHeadDeputy = city.CityAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.CityHeadDeputy
                                     && a.Status);
            return cityHeadDeputy;
        }

        public static List<CityAdministrationDTO> GetCityAdmins(CityDTO city)
        {
            var cityAdmins = city.CityAdministration
                .Where(a => a.Status)
                .ToList();
            return cityAdmins;
        }
    }
}
