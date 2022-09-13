using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityByIdWthFullInfoQuery : IRequest<CityDto>
    {
        public int CityId { get; set; }

        public GetCityByIdWthFullInfoQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
