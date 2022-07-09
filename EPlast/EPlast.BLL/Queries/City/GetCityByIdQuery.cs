using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityByIdQuery : IRequest<CityDto>
    {
        public int CityId { get; set; }

        public GetCityByIdQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
