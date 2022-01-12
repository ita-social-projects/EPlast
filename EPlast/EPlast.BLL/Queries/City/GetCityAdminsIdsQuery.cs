using MediatR;

namespace EPlast.BLL.Queries.City
{
    public class GetCityAdminsIdsQuery : IRequest<string>
    {
        public int CityId { get; set; }

        public GetCityAdminsIdsQuery(int cityId)
        {
            CityId = cityId;
        }
    }
}
