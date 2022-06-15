using EPlast.BLL.DTO.City;
using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityAdminsQuery : IRequest<CityAdministrationViewModelDTO>
    {
        public int CityId { get; set; }

        public GetCityAdminsQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
