using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityByIdWthFullInfoQuery : IRequest<CityDTO>
    {
        public int CityId { get; set; }

        public GetCityByIdWthFullInfoQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
