using System.Collections.Generic;
using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityUsersQuery : IRequest<IEnumerable<CityUserDto>>
    {
        public int CityId{ get; set; }

        public GetCityUsersQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
