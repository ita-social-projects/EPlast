using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityFollowersQuery : IRequest<CityProfileDTO>
    {
        public int CityId { get; set; }

        public GetCityFollowersQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
