using System.Collections.Generic;

namespace EPlast.BussinessLayer.AccessManagers.Interfaces
{
    public interface ICityAccessManagerSettings
    {
        Dictionary<string, ICitiesGetter> GetCitiesGetters();
    }
}