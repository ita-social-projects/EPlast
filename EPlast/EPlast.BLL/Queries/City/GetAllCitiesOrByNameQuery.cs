using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetAllCitiesOrByNameQuery : IRequest<IEnumerable<CityDTO>>
    {
        public string CityName{ get; set; }

        public GetAllCitiesOrByNameQuery(string cityName)
        {
            CityName = cityName;
        }
    }
}
