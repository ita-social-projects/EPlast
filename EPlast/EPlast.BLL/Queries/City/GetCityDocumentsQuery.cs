using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityDocumentsQuery : IRequest<CityProfileDto>
    {
        public int CityId { get; set; }

        public GetCityDocumentsQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
