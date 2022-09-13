using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityProfileBasicQuery : IRequest<CityProfileDto>
    {
        public int CityId { get; set; }

        public GetCityProfileBasicQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
